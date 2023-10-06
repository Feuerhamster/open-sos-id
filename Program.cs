using open_sos_id.Services;
using open_sos_id.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

#if DEBUG
	builder.Services.AddSassCompiler();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.Run();