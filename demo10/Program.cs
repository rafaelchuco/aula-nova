using AulaNova.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<AlumnoRepository>();
var app = builder.Build();
app.Services.GetRequiredService<AlumnoRepository>().Initialize();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Alumno/Error"); app.UseHsts(); }
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Alumno}/{action=Index}/{id?}");
app.Run();
