using Microsoft.EntityFrameworkCore;

namespace MyProject.Entity {
    public class MyDbContext : DbContext{

        public DbSet<Blog> BlogTable { get; set; }
        public DbSet<Reply> ReplyTable { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {

        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Blog>()
                .HasKey("blog_id");
            modelBuilder.Entity<Reply>()
                .HasKey("reply_id");
            modelBuilder.Entity<Blog>()
                 .HasMany(e => e.replyList)
                 .WithOne(e => e.blog)
                 .HasForeignKey(e => e.Blog_id) 
                 .OnDelete(DeleteBehavior.ClientCascade)
                 .IsRequired();
        }

    }
}
