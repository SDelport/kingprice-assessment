using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var entryAssembly = Assembly.GetEntryAssembly();
if (entryAssembly == null || entryAssembly != Assembly.GetExecutingAssembly())
{
    // Running inside a test process - skip configuring the web host.
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserContext>(options =>
{
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);
    string DbPath = System.IO.Path.Join(path, "king-price-assessment.db");

    options.UseSqlite($"Data Source={DbPath}");
});


// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddSingleton<ApiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("https://localhost:7052").AllowAnyMethod();
        });
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigins");


app.Start();
var url = app.Urls.FirstOrDefault();
ApiService.BaseUrl = url;
app.WaitForShutdown();
