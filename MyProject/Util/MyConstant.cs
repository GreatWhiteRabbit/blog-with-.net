namespace MyProject.Util {

    /// <summary>
    /// 保存常量的类
    /// </summary>
    public class MyConstant {
        /// <summary>
        /// 存在Redis中的博客的key
        /// </summary>
        public static readonly string BlogKey = "blog";

        /// <summary>
        /// 存在Redis中的留言的key
        /// </summary>
        public static readonly string ReplyKey = "reply";

        /// <summary>
        /// 博客List的key
        /// </summary>
        public static readonly string BlogListKey = "blogList";

        /// <summary>
        /// 留言List的key
        /// </summary>
        public static readonly string ReplyListKey = "replyList";
    }
}
