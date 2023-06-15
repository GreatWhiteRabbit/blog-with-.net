using MyProject.Entity;

namespace MyProject.Service {
    public interface ReplyService {
        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="reply">留言内容</param>
        /// <returns>true表示添加成功</returns>
        public bool addReply(Reply reply);

        /// <summary>
        /// 修改留言内容
        /// </summary>
        /// <param name="reply">留言内容</param>
        /// <param name="id">需要修改的留言ID</param>
        /// <returns>true表示修改成功</returns>
        public bool updateReply(int id,Reply reply);

        /// <summary>
        /// 根据ID删除留言
        /// </summary>
        /// <param name="id">要删除的留言ID</param>
        /// <returns>true表示删除成功</returns>
        public bool deleteReplyById(int id);

        /// <summary>
        /// 根据blog_id获取所有的留言
        /// </summary>
        /// <param name="blog_id">blog_id</param>
        /// <returns>boke留言列表</returns>
        public List<Reply> getReplyByBlogId(int blog_id);
    }
}
