using Microsoft.EntityFrameworkCore;
using MultipleChoice.Areas.Admin.Controllers;
using MultipleChoice.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MultipleChoiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MultipleChoice")));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MultipleChoice.Session";
    /* options.IdleTimeout = TimeSpan.FromMinutes(20);*/
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.AreaPageViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
        options.AreaPageViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
        options.AreaPageViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    })
    .AddApplicationPart(typeof(TeacherController).Assembly)
    .AddControllersAsServices();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "adminArea",
        pattern: "{area=admin}/{controller:exists}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "teacherArea",
        pattern: "{area=teacherarea}/{controller:exists}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area=teacherarea}/{controller=Question}/{action=Index}/{id?}"
    );
});

app.Run();
