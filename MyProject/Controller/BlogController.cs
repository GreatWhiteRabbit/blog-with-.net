using Microsoft.AspNetCore.Mvc;
using MyProject.Entity;
using MyProject.Service;
using MyProject.Util;


namespace MyProject.Controller {
    [ApiController]
    [Route("blog")]
    public class BlogController : ControllerBase{

        private Result result= new Result();

        private readonly BlogService blogService;

        public BlogController(BlogService blogService) {
               this.blogService = blogService;
        }

        // 添加博客
        [HttpPost]
        public Result AddBlog(Blog blog) {
           bool add_success = blogService.addBlog(blog);
            if(add_success) {
                return result.Ok("添加成功");
            }
            else {
                return result.failed(Util.StatusCode.ServerError,"添加失败");
            }
            
        }

        // 根据ID获取blog
        [HttpGet("{id}")]
        public Result getBlogById(int id) {
            
           Blog blog = blogService.getBlogById(id);
            if(blog != null) {
                return result.Ok(blog);
            }
            else {
                return result.failed(Util.StatusCode.NotFound, "结果不存在");
            }
        }

        // 根据page和size获取博客列表
        [HttpGet("{page}/{size}")]
        public Result getBlogWithPageAndSize(int page, int size) {
            List<Blog> blogList =  blogService.getBlogList(page, size);
            if(blogList != null ) {
                return result.Ok(blogList);
            }
            else {
                return result.failed(Util.StatusCode.BadRequest, "请求错误");
            }
        }

        // 根据ID删除博客
        [HttpDelete("{id}")]
        public Result deleteBlogById(int id) {
            bool success = blogService.removeBlog(id);
            if(success) {
                return result.Ok("删除成功");
            }
            else {
                return result.failed(Util.StatusCode.ServerError,"删除失败");
            }
        }

        // 根据ID修改博客
        [HttpPost("{id}")]
        public Result updateBlogById(int id, Blog blog) {
            bool success = blogService.updateBlog(id, blog);
            if(success) {
                return result.Ok("修改成功");
            }
            else {
                return result.failed(Util.StatusCode.ServerError, "修改失败");
            }
        }
    }
}
