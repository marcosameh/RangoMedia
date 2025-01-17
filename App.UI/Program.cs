using App.Domain.Models;
using App.Application;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using App.Infrastructure.Helpers;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RangoMediaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RangoConnection")));
builder.Services.AddHangfire(config =>
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("RangoConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<EmailManager>();
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
app.UseRouting();
app.UseHangfireDashboard();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
