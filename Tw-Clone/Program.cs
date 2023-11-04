using Microsoft.EntityFrameworkCore;
using Tw_Clone.Models;
using Tw_Clone.Repositories;
using Tw_Clone.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TweetService>();
builder.Services.AddScoped<IEncoderService, EncoderService>();

builder.Services.AddDbContext<TwcloneContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
});



builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<ITweetRepository,TweetRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
