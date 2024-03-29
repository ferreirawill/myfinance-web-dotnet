using System.Text.Json;
using myfinance_web_dotnet;
using myfinance_web_dotnet.Domain.Services;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Utils.Logger;




var builder = WebApplication.CreateBuilder(args);


builder.Logging.AddCustomLoger();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyFinanceDbContext>();
builder.Services.AddScoped<IPlanoContaService,PlanoContaService>();
builder.Services.AddScoped<ITransacaoService,TransacaoService>();


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
