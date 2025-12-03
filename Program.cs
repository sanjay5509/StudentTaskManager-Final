using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudentTaskManager.Data;
using StudentTaskManager.Models;
using System.Globalization;
using StudentTaskManager.Hubs;
using StudentTaskManager.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
})

    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddSignalR();
builder.Services.AddHostedService<StudentTaskManager.Services.ReminderService>();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") };
    options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-US") };
});


var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    
//    if (!roleManager.RoleExistsAsync("Admin").Result)
//    {
//        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
//    }
//    if (!roleManager.RoleExistsAsync("Student").Result)
//    {
//        roleManager.CreateAsync(new IdentityRole("Student")).Wait();
//    }

   
//    string adminEmail = "newadmin_v2@studybuddy.com";
//    string adminPassword = "AdminPassword123";

//    var existingAdmin = userManager.FindByEmailAsync(adminEmail).Result;

//    if (existingAdmin != null)
//    {
       
//        if (!userManager.IsInRoleAsync(existingAdmin, "Admin").Result)
//        {
//            userManager.AddToRoleAsync(existingAdmin, "Admin").Wait();
//        }

       
//        userManager.RemovePasswordAsync(existingAdmin).Wait();
//        userManager.AddPasswordAsync(existingAdmin, adminPassword).Wait();
//    }
//    else 
//    {
//        ApplicationUser adminUser = new ApplicationUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            EmailConfirmed = true
//        };

//        IdentityResult createResult = userManager.CreateAsync(adminUser, adminPassword).Result;

//        if (createResult.Succeeded)
//        {
//            userManager.AddToRoleAsync(adminUser, "Admin").Wait();
//        }
//    }
//}

if (app.Environment.IsDevelopment())
{

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseRouting();

app.UseRequestLocalization();


app.UseAuthentication();
app.UseAuthorization();


app.MapHub<StudentTaskManager.Hubs.ReminderHub>("/reminderHub");

    
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();


app.Run();