namespace MyProject.Service {
    public interface EmailService {


        /// <summary>
        /// 发送邮件和附件
        /// </summary>
        /// <param name="reciverEmail">接受者</param>
        /// <param name="myAlia">发送者别名</param>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="attachments">附件</param>
        /// <param name="HtmlFormat">是否为HTML格式，默认为false</param>
        /// <returns>true表示发送成功</returns>
        public bool sendEmailWithAttach(string reciverEmail,string myAlia, string title, 
            string content,  IFormFile attachment, bool HtmlFormat = false);

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="reciverEmail">接收者</param>
        /// <param name="myAlia">发送者别名</param>
        /// <param name="title">文件标题</param>
        /// <param name="content">文件内容</param>
        /// <param name="HtmlFormat">是否为HTML格式，默认为false</param>
        /// <returns>true表示发送成功</returns>
        public bool sendEmail(string reciverEmail, string myAlia, string title,
            string content, bool HtmlFormat = false);
    }
}
