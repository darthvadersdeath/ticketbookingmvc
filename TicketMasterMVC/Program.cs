using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketMasterMVC.Data;
using TicketMasterMVC.Models;
using TicketMasterMVC.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DatabaseCon");

builder.Services.AddDbContext<TicketContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TicketContext>()
    .AddDefaultTokenProviders();

//EmailSender
builder.Services.AddTransient<IEmailSender, EmailSender>();


// Register HttpClient for ApiService
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7267/");
});


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<TicketContext>()
    .AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
