using Microsoft.AspNetCore.Mvc;
using MyProject.Entity;
using MyProject.Service;
using MyProject.Util;

namespace MyProject.Controller {

    [ApiController]
    [Route("/admin/email")]
    public class EmailController {
        private readonly EmailService emailService;

        private readonly Result result = new Result();
        public EmailController(EmailService emailService) {
            this.emailService = emailService;
        }

        [HttpPost]
        public Result sendEmail(Email email) {
            bool success = emailService.sendEmail(email.reciverEmail, email.myAlia, email.title,
                email.content, email.HtmlFormat);
            if(success) {
                return result.Ok("邮件发送成功");
            }
            return result.failed(Util.StatusCode.ServerError, "邮件发送失败");
        }
        [HttpPost("attach")]
        public Result sendEmailWithAttach(Email email, IFormFile attach) {
            bool success = emailService.sendEmailWithAttach(email.reciverEmail, email.myAlia, email.title,
                email.content, attach ,email.HtmlFormat);
            if (success) {
                return result.Ok("邮件发送成功");
            }
            return result.failed(Util.StatusCode.ServerError, "邮件发送失败");
        }

    }
}
