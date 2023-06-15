namespace MyProject.Util {
    public class Result {

        public StatusCode StatusCode { get; set; }
        public bool success { set; get; }
        public Object data { get; set; }

        public Result(StatusCode code, bool success, Object data) {
            this.data = data;
            this.success = success;
            this.StatusCode = code;
        }
        public Result() {

        }

        /// <summary>
        /// 返回响应成功的结果
        /// </summary>
        /// <param name="data">响应成功的结果</param>
        /// <returns>响应成功的结果</returns>
       public Result Ok(Object data ) {
            return new Result(StatusCode.Success, true, data);
       }

        /// <summary>
        /// 返回响应失败的结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="data">响应失败结果</param>
        /// <returns>响应失败的结果</returns>
        public Result failed(StatusCode code, Object data) {
            return new Result(code, false, data);
        }

    }
}
