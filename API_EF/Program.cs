using API_EF.Data;
using API_EF.Models;
using API_EF.Service;
using API_EF.Service.Auth;
using API_EF.Testing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var jwtConfig = new JwtConfiguration();
builder.Configuration.GetSection("Jwt").Bind(jwtConfig);
builder.Services.AddSingleton(jwtConfig);


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"));
});

// using packing Identity to use 2 method ,IDentity and IndentityRole to save information of User about UserName and passWord
// Then Setting auto save infor inot database (Auto create table UserLogin of Method Identity)

builder.Services.AddApplicationIdentity();

//Add Autthenciton for project with 
//Add JWT into project


builder.Services.AddApplicationJwtAuthentication(builder.Configuration);

//AddCookie Authen ciation

builder.Services.AddApplitaionAuthenciation();

//Add Authoiize
builder.Services.AddApplicationAuthorization();

//Add to ussing memorycaches
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

//   testing DI simple  with TestingRandom
builder.Services.AddTransient<IRandomTesting, TestingRandom>();
builder.Services.AddTransient<IAuthServices, AuthServices>();
builder.Services.AddTransient<ICaches, CachesSystem>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Creat scope for database
//await app.SeedDataAsync();

app.MapControllers();
//app.UseMiddleware<BasicAuthentor>("test");

app.Run();
