using Microsoft.AspNetCore.Mvc;
using MyProject.Entity;
using MyProject.Service;
using MyProject.Util;

namespace MyProject.Controller {
    [ApiController]
    [Route("reply")]
    public class ReplyController : ControllerBase{

        private readonly ReplyService replyService;

        private readonly Result result = new Result();

        public ReplyController(ReplyService replyService) {
            this.replyService = replyService;
        }

        // 根据博客ID获取留言列表
        [HttpGet("{id}")]
        public Result getReplyList(int id) {
            List<Reply> replyList =  replyService.getReplyByBlogId(id);
            if(replyList.Count != 0) {
                return result.Ok(replyList);
            }
            else {
                return result.failed(Util.StatusCode.NotFound, "暂无留言，快去发表言论吧");
            }
        }
        // 插入留言
        [HttpPost]
        public Result addReply(Reply reply) {
           
            bool success = replyService.addReply(reply);
            if(success) {
                return result.Ok("添加成功");
            }
            else {
                return result.failed(Util.StatusCode.ServerError, "发表留言失败");
            }
        }

        // 根据ID删除留言
        [HttpDelete("{id}")]
        public Result deleteReplyById(int id) { 
            bool success =  replyService.deleteReplyById(id);
            if(success) {
                return result.Ok("删除失败");
            }
            else {
                return result.failed(Util.StatusCode.ServerError, "删除失败");
            }
        }

        // 根据ID修改留言
        [HttpPost("{id}")]
        public Result updateReplyById(int id, Reply reply) {
            bool success = replyService.updateReply(id, reply);
            if(success) {
                return result.Ok("修改成功");
            }
            else {
                return result.failed(Util.StatusCode.ServerError, "修改失败");
            }
        }
    }
}
