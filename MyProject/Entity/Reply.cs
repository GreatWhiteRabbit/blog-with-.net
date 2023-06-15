using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyProject.Entity {
    [Table("reply")]
    public class Reply {
        [Key]
        private int reply_id;

        private int blog_id;
        private string reply_context;
        private DateTime create_time;

        
        public  Blog? blog { get; set; } = null;

       public int Reply_id { 
            get { return reply_id; }
            set { reply_id = value; }
        }

        public int Blog_id {
            get { return blog_id; }
            set { blog_id = value; }
        }
        public string Reply_context {
            get { return reply_context; }
            set { reply_context = value; }
        }
        public DateTime Create_time { 
            get { return create_time; } 
            set { create_time = value; } 
        }
    }
}
