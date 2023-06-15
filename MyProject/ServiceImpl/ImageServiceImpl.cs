using MyProject.Service;

namespace MyProject.ServiceImpl {
    public class ImageServiceImpl : ImageService {


        private readonly string savaPath = @"E:\c# code\MyProject\MyProject\MyImages\"; // 图片保存路径

        private readonly string accessPath = @"/image/blog"; // 图片访问相对路径

        public int uploadImage(IFormFile file, ref string successUrl) {
            // 获取文件大小
            long length = file.Length;
            long maxSize = 1024 * 1024 * 5; // 文件最大不超过5M

            // 获取文件猴嘴
            string suffix = file.FileName.Split('.')[1];

            if(length > maxSize) {
                return -1;
            }
            // 将文件保存
            try {
                string fileName = setFileName();
                if (fileName == "") return 0;
                fileName = fileName + "." + suffix;
                string saveName =  savaPath + fileName;
                using(var stream = new FileStream(saveName,FileMode.Create)) {
                    file.CopyTo(stream);
                }
                successUrl = accessPath + "/" + fileName;
                return 1;
            } catch(Exception ex) {
                Console.WriteLine(ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 生成文件名称
        /// </summary>
        /// <returns>返回生成的文件名</returns>
        private string setFileName() {
            string fileName = "";
            // 获取当前时间
            string formatTime = "yyyy" + "MM" + "dd" + "hh" + "mm" + "ss";
            string currentTime = DateTime.Now.ToString(formatTime);
            if(randomCharacter(6) == "") {
                return "";
            }
            fileName = currentTime + randomCharacter(6);
            return fileName;
        }
        
        /// <summary>
        /// 生成长度为length的A-Z的字母
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string randomCharacter(int length) {
            string strings = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string random = "";
            int[] nums = new int[length];
            Random rand = new Random();
            for(int i = 0; i < length; i++) {
                nums[i] = rand.Next(1,26);
            }
            for(int i = 0;i < length; i++) {
                random = random + strings[nums[i] - 1];
            }
            return random;
        }
    }
}
