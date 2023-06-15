using Microsoft.EntityFrameworkCore;
using MyProject.Entity;
using MyProject.Service;
using MyProject.Util;


namespace MyProject.ServiceImpl {
    public class ReplyServiceImpl : ReplyService {

        private readonly MyDbContext myDbContext;


        private readonly RedisHelper redisHelper = new RedisHelper();
        public ReplyServiceImpl(MyDbContext dbContext) {
            this.myDbContext = dbContext;
        }

        public bool addReply(Reply reply) { 
            // 获取当前时间
            reply.Create_time = DateTime.Now;
            // 将留言存到数据库中
            myDbContext.ReplyTable.Add(reply);
            // 将留言的ID存入到redis对应的博客留言列表中
            
            if (myDbContext.SaveChanges() == 1) {
                redisHelper.listLeftPush<Reply>(MyConstant.ReplyListKey + reply.Blog_id,reply);
                // 设置过期时间
                redisHelper.setExpire(MyConstant.ReplyListKey + reply.Blog_id, 60 * 60 * 24);
                return true;
            }
            else {
                return false;
            }
        }

        public bool deleteReplyById(int id) {
            // 先删库
            Reply reply = myDbContext.ReplyTable.Single(e => e.Reply_id == id);
            if (reply != null) {
                int blog_id = reply.Blog_id;
                myDbContext.Remove(reply);
                if (myDbContext.SaveChanges() == 1) {
                    // 再删缓存
                  return  redisHelper.deleteKey(MyConstant.ReplyListKey + blog_id);
                }
                return false;
            }
            return false;
        }

        public List<Reply> getReplyByBlogId(int blog_id) {
            
            // 根据blog_id从Redis中获取留言的条数
           int length =(int) redisHelper.getListLength(MyConstant.ReplyListKey + blog_id);
            // length为0说明Redis中没有数据，从MySQL中查找
            if(length == 0) {
              List<Reply> replyList = myDbContext.ReplyTable.ToList<Reply>();
                // 将获取到的值存放的Redis中
                redisHelper.listLeftPushAll(MyConstant.ReplyListKey + blog_id, replyList);
                // 设置过期时间
                redisHelper.setExpire(MyConstant.ReplyListKey + blog_id, 60 * 60 * 24);
                return replyList;
            } 
            // length不为0说明Redis中保存有数据
            else {
               return redisHelper.getListRange<Reply>(MyConstant.ReplyListKey + blog_id, 0, -1);
            }
        }

        public bool updateReply(int id, Reply reply) {
            // 先修改库
            Reply oldReply = myDbContext.ReplyTable.Single(e => e.Reply_id == id);
            if (oldReply != null) {
                int blog_id = oldReply.Blog_id;
                oldReply.Reply_context = reply.Reply_context;
                myDbContext.Update(oldReply);
                if (myDbContext.SaveChanges() == 1) {
                    // 再删除缓存
                    return redisHelper.deleteKey(MyConstant.ReplyListKey + blog_id);
                }
                return false;
            }
            return false;
        }
    }
}
