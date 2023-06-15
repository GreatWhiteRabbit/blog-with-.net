using Microsoft.AspNetCore.Mvc;
using MyProject.Service;
using MyProject.Util;

namespace MyProject.Controller {
    [ApiController]
    [Route("/admin/upload")]
    public class ImageController : ControllerBase {

        private readonly Result result = new Result();

        private readonly ImageService imageService;

        public ImageController(ImageService imageService) {
            this.imageService = imageService;
        }

        // 上传图片并且返回图片的相对路径
        [HttpPost]

        public Result uploadFile(IFormFile file) {
            string url = "";
          int success =  imageService.uploadImage(file, ref url);
            if(success == -1) {
                return result.failed(Util.StatusCode.BadRequest, "文件大小超出限制");
            }
            else if(success == 0) {
                return result.failed(Util.StatusCode.ServerError, "服务器内部错误");
            }
            return result.Ok(url);
        }

    }
}
