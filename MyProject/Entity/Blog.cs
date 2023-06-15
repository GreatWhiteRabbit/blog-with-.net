using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Entity {
    [Table("blog")]
    public class Blog {
        [Key]
        private int blog_id;
        private string blog_context;
        private DateTime create_time;
        private string blog_describe;
        public List<Reply> replyList { get; } = new List<Reply>();
        public int Blog_id { 
            get { return blog_id; }
            set { blog_id = value; }
        }
        public string Blog_context { 
            get { return blog_context; } 
            set { blog_context = value;}
        }
        public DateTime Create_time { 
            get { return create_time; } 
            set { create_time = value; }
        }
        public string Blog_describe { 
            get { return blog_describe; }
            set { blog_describe = value; }
        }
    }
}
