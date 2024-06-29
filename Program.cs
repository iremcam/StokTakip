using StokTakip.Data;
using StokTakip.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DataConnection>();
builder.Services.AddScoped<DataConnection>();
builder.Services.AddScoped<UrunServisi>();

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
    pattern: "{controller=Kullanici}/{action=Index}/{id?}");

    // veya services.AddScoped<DataConnection>();
    // Diðer servis kayýtlarý


app.Run();
