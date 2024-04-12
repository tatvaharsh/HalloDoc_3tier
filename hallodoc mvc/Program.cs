
//using hallocdoc_mvc_Service.Implementation;
//using hallocdoc_mvc_Service.Interface;
//using hallodoc_mvc_Repository.DataContext;
//using hallodoc_mvc_Repository.Implementation;
//using hallodoc_mvc_Repository.Interface;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext <ApplicationDbContext>();
//builder.Services.AddSingleton<IPatient_Service,Patient_Service>();
//builder.Services.AddSingleton<IPatient_Repository, Patient_Repository>();
//builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();


//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(60);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
//app.UseSession();

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=patient_screen}/{id?}");

//app.Run();

using hallocdoc_mvc_Service.Implementation;
using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataContext;
using hallodoc_mvc_Repository.Implementation;
using hallodoc_mvc_Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPatient_Service, Patient_Service>();
builder.Services.AddScoped<IPatient_Repository, Patient_Repository>();
builder.Services.AddScoped<IAdmin_Service, Admin_Service>();
builder.Services.AddScoped<IPhysician_Service, Physician_Service>();
builder.Services.AddScoped<IAdmin_Repository, Admin_Repository>();
builder.Services.AddScoped<IPhysician_Repository, Physician_Repository>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.IsEssential = true;
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
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
//pattern: "{controller=Home}/{action=patient_screen}/{id?}");
pattern: "{controller=Login}/{action=Admin_Login}/{id?}");

app.Run();
