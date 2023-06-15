using MailKit.Net.Smtp;
using MimeKit;
using MyProject.Service;



namespace MyProject.ServiceImpl {
    public class EmailServiceImpl : EmailService {

        private readonly string userName;
        private readonly string host;
        private readonly string password;
        private readonly int port;

        public EmailServiceImpl() {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json").Build();
            userName = configuration["Email:username"];
            host = configuration["Email:host"];
            password = configuration["Email:password"];
            port = int.Parse(configuration["Email:port"]);
        }

        public bool sendEmail(string reciverEmail, string myAlia, 
            string title, string content, bool HtmlFormat = false) {


            // 创建邮件发送服务
            using (SmtpClient smtpClient = new SmtpClient()) {
                // 邮件发送服务主机和端口
                smtpClient.Connect(host, port);
                // 设置第三方邮件发送地址和授权码
                smtpClient.Authenticate(userName, password);

                // 设置邮件发送信息
                MimeMessage mailMessage = new MimeMessage();

                // 设置发送者邮件地址和昵称
                mailMessage.From.Add(new MailboxAddress(myAlia, userName));
                // 设置接收者邮件地址
                mailMessage.To.Add(new MailboxAddress("", reciverEmail));
                // 设置标题
                mailMessage.Subject = title;
                // 设置内容格式
                TextPart body;
                if (HtmlFormat) {
                    // HTML格式
                    body = new TextPart(MimeKit.Text.TextFormat.Html) {
                        Text = content
                    };
                }
                else {
                    // 普通文本格式
                    body = new TextPart(MimeKit.Text.TextFormat.Text) {
                        Text = content
                    };
                }
                Multipart multipart = new Multipart() {

                };
                multipart.Add(body);
                mailMessage.Body = multipart;

                try {
                    // 发送邮件
                    smtpClient.Send(mailMessage);
                    return true;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        public bool sendEmailWithAttach(string reciverEmail, string myAlia,
            string title, string content, IFormFile attachment, 
            bool HtmlFormat = false) {

            using (SmtpClient smtpClient = new SmtpClient()) {
                // 邮件发送服务主机和端口
                smtpClient.Connect(host, port);
                // 设置第三方邮件发送地址和授权码
                smtpClient.Authenticate(userName, password);

                // 设置邮件发送信息
                MimeMessage mailMessage = new MimeMessage();

                // 设置发送者邮件地址和昵称
                mailMessage.From.Add(new MailboxAddress(myAlia, userName));
                // 设置接收者邮件地址
                mailMessage.To.Add(new MailboxAddress("", reciverEmail));
                // 设置标题
                mailMessage.Subject = title;
                // 设置内容格式
                TextPart body;
                if (HtmlFormat) {
                    // HTML格式
                    body = new TextPart(MimeKit.Text.TextFormat.Html) {
                        Text = content
                    };
                }
                else {
                    // 普通文本格式
                    body = new TextPart(MimeKit.Text.TextFormat.Text) {
                        Text = content
                    };
                }
                
                // 添加附件
                MimePart attach = new MimePart(attachment.ContentType) {
                    Content = new MimeContent(attachment.OpenReadStream(), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName,
                    IsAttachment = true
                };

                Multipart multipart = new Multipart("mixed") {
                    body,
                    attach
                };
                    
                mailMessage.Body = multipart;
                
                try {
                    // 发送邮件
                    smtpClient.Send(mailMessage);
                    return true;
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        
    }
}
