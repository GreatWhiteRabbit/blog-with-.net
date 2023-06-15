namespace MyProject.Util {
    public enum StatusCode {
        /// <summary>
        /// 响应成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 客户端请求错误
        /// </summary>
        BadRequest =400,
        /// <summary>
        /// 权限错误，拒绝请求
        /// </summary>
        Forbidden = 403,
        /// <summary>
        /// 无法找到页面
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// 请求方法不允许
        /// </summary>
        MethodNotAllowed = 405,
        /// <summary>
        /// 服务器内部错误
        /// </summary>
        ServerError = 500,
        /// <summary>
        /// 服务器宕机
        /// </summary>
        ServerShutDown = 503
    }
}
