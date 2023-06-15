using MyProject.Entity;

namespace MyProject.Service {
    public interface BlogService {

        /// <summary>
        /// 向数据库中插入博客
        /// </summary>
        /// <param name="blog">博客信息</param>
        /// <returns>true表示插入成功</returns>
        public bool addBlog(Blog blog);

        /// <summary>
        /// 根据ID删除博客
        /// </summary>
        /// <param name="id">博客的ID</param>
        /// <returns>true表示删除成功</returns>
        public bool removeBlog(int id);
        
        /// <summary>
        /// 根据ID修改博客
        /// </summary>
        /// <param name="id">需要修改的博客ID</param>
        /// <param name="blog">修改后的博客ID</param>
        /// <returns>是否修改成功</returns>
        public bool updateBlog(int id, Blog blog);

        /// <summary>
        /// 根据ID获取博客
        /// </summary>
        /// <param name="id">需要获取的博客ID</param>
        /// <returns>获取到的博客</returns>
        public Blog getBlogById(int id);

        /// <summary>
        /// 根据页面和页面大小获取博客列表
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="size">页的大小</param>
        /// <returns>博客列表</returns>
        public List<Blog> getBlogList(int page, int size);
    }
}
