using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyProject.Entity;
using MyProject.Service;
using MyProject.ServiceImpl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 获取数据库连接字符串
var connectionString = builder.Configuration["MysqlConnectionString"]; // 数据库访问配置文件

// 设置数据库服务版本
var version = new MySqlServerVersion(new Version(8, 0, 28)); // 数据库版本相关信息
builder.Services.AddDbContext<MyDbContext>(opts =>
    opts.UseMySql(connectionString, version)
);

// 注册service
builder.Services.AddScoped<ReplyService, ReplyServiceImpl>();
builder.Services.AddScoped<BlogService, BlogServiceImpl>();
builder.Services.AddScoped<ImageService, ImageServiceImpl>();
builder.Services.AddScoped<EmailService, EmailServiceImpl>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

string savaPath = @"E:\c# code\MyProject\MyProject\MyImages";
string accessPath = @"/image/blog";

// 添加文件访问路径
app.UseStaticFiles(new StaticFileOptions() {
    FileProvider = new PhysicalFileProvider(savaPath), // 文件存放路径
    RequestPath = new PathString(accessPath) // 外界访问路径
}
    );

app.MapControllers();

app.Run();
