using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyProject.Entity;
using MyProject.Service;
using MyProject.ServiceImpl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ��ȡ���ݿ������ַ���
var connectionString = builder.Configuration["MysqlConnectionString"]; // ���ݿ���������ļ�

// �������ݿ����汾
var version = new MySqlServerVersion(new Version(8, 0, 28)); // ���ݿ�汾�����Ϣ
builder.Services.AddDbContext<MyDbContext>(opts =>
    opts.UseMySql(connectionString, version)
);

// ע��service
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

// ����ļ�����·��
app.UseStaticFiles(new StaticFileOptions() {
    FileProvider = new PhysicalFileProvider(savaPath), // �ļ����·��
    RequestPath = new PathString(accessPath) // ������·��
}
    );

app.MapControllers();

app.Run();
