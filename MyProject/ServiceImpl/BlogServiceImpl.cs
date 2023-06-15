using MyProject.Entity;
using MyProject.Service;
using MyProject.Util;
using System.Reflection.Metadata;

namespace MyProject.ServiceImpl {
    public class BlogServiceImpl : BlogService {

        private readonly MyDbContext myDbContext;

        private readonly RedisHelper redisHelper = new RedisHelper();

        public BlogServiceImpl(MyDbContext context) {
            myDbContext = context;
        }

        public bool addBlog(Blog blog) {
            // 获取当前时间
            blog.Create_time = DateTime.Now;
            // 将博客存到数据库中
            myDbContext.BlogTable.Add(blog);
            // 将博客的ID存入到redis列表中
            // 将博客插入到Redis中
           if( myDbContext.SaveChanges() == 1) {
                redisHelper.listLeftPush<int>(MyConstant.BlogListKey, blog.Blog_id);
                redisHelper.setString<Blog>(MyConstant.BlogKey + blog.Blog_id, blog);
                // 设置过期时间
                redisHelper.setExpire(MyConstant.BlogKey + blog.Blog_id, 60 * 60 * 24);
                return true;
            }
           else {
                return false;
            }

        }

        public Blog getBlogById(int id) {
            Blog blog;
            // 首先从Redis中获取
             blog = redisHelper.getStringObject<Blog>(MyConstant.BlogKey + id);
            // 从数据库中查找
            if(blog == null) {
                blog = myDbContext.BlogTable.Single(e => e.Blog_id == id);  
                // 如果blog为null，说明数据库中也不存在
               if(blog != null) {
                    // 将查询到的数据存放到Redis中
                    redisHelper.setString(MyConstant.BlogKey + id, blog);
                    // 设置过期时间
                    redisHelper.setExpire(MyConstant.BlogKey + id, 60 * 60 * 24);
                }
                return blog;
            }
            else {
                return blog;
            }
        }

        public List<Blog> getBlogList(int page, int size) {
            List<Blog> blogList = new List<Blog>();
            // 先从Redis中查找
            int length =(int) redisHelper.getListLength(MyConstant.BlogListKey);
            int lastIndex = 0;
            int startIndex = (page - 1) * size;
            if (length == 0) {
                // 从数据库中查找
               List<Blog> getBlog = myDbContext.BlogTable.ToList();
                length = getBlog.Count;
                // 需要获取的数据个数超出容量
                if (startIndex > length) {
                    return blogList;
                }
                // 获取最后一个元素的下标
                if (page * size > length) { lastIndex = length - 1; }
                else { lastIndex = page * size - 1; }
               
                for (int i = 0;i < length; i++) {
                    Blog blog = getBlog[i];
                    // 将下标从startIndex 到 lastIndex中元素添加到blogList中
                    if (i >= startIndex && i <= lastIndex) {
                        blogList.Add(blog);
                    }
                   // 将其余元素存放到Redis中
                    redisHelper.setString(MyConstant.BlogKey + blog.Blog_id, blog);
                    redisHelper.setExpire(MyConstant.BlogKey + blog.Blog_id, 60 * 60 * 24);
                    redisHelper.listLeftPush(MyConstant.BlogListKey, blog.Blog_id);
                }
                return blogList;
            }
            else {
                // 需要获取的数据个数超出容量
                if (startIndex > length) {
                    return blogList;
                }
                // 从Redis中获取list
                List<int> BlogIdList = redisHelper.getListRange<int>(MyConstant.BlogListKey, startIndex, lastIndex);
                // 根据ID获取blog
                for(int i = 0; i <  BlogIdList.Count; i++) {
                    blogList.Add(getBlogById(BlogIdList[i]));
                }
                return blogList;
            }
        }

        public bool removeBlog(int id) {
            // 先删库
          Blog blog =  myDbContext.BlogTable.Single(e => e.Blog_id == id);
            if(blog != null) {
                myDbContext.Remove(blog);
               if( myDbContext.SaveChanges() == 1 ) {
                    // 再删缓存
                     redisHelper.deleteKey(MyConstant.BlogKey + id);
                    redisHelper.deleteKey(MyConstant.BlogListKey);
                }
                return false; 
            }
            return false;
        }

        public bool updateBlog(int id, Blog blog) {
            // 先修改库
            Blog oldBlog = myDbContext.BlogTable.Single( e => e.Blog_id == id);
            if(oldBlog != null) {
                oldBlog.Blog_context = blog.Blog_context;
                oldBlog.Blog_describe = blog.Blog_describe;
                myDbContext.Update(oldBlog);
                if(myDbContext.SaveChanges() == 1) {
                    // 再删除缓存
                    return redisHelper.deleteKey(MyConstant.BlogKey + id);
                }
                return false;
            }
            return false;
        }
    }
}
