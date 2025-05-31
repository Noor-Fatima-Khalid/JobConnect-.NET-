// for signalr hub
using JobPortal.Hubs;
using System.Security.Claims;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Models.Interfaces;
using JobPortal.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// registering DI here
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
builder.Services.AddScoped<IJobApplicationsRepository, JobApplicationsRepository>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// configure db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Adding policy here
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployerOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Employer"));
    options.AddPolicy("JobSeekerOnly", policy => policy.RequireClaim(ClaimTypes.Role, "JobSeeker"));
});
// email confirmation not needed
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
});

builder.Services.AddControllersWithViews();
// for signal r  -  notifications 
builder.Services.AddSignalR();

// for custom routing
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Pages/Account/Login";
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// for identity
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//for notifications
app.MapHub<NotificationHub>("/notificationHub");

app.Run();