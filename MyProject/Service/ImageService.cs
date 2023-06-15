namespace MyProject.Service {
    public interface ImageService {

        /// <summary>
        /// 上传文件并且返回文件的相对路径
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="successUrl">上传成功的路由</param>
        /// <returns>-1 表示文件超出限制；0 表示文件上传失败；1表示文件上传成功</returns>
        public int uploadImage(IFormFile file, ref string successUrl);
    }
}
