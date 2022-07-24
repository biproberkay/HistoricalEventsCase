using HistoricalEventsCase.Data;
using HistoricalEventsCase.Extensions;
using HistoricalEventsCase.Infrastructure.Jwt;
using HistoricalEventsCase.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SqliteConnection") ?? throw new InvalidOperationException("Connection string 'SqliteConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
});
builder.Services.AddCustomLocalization();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
var token = builder.Configuration.GetSection("tokenManagement").Get<TokenManagement>();
builder.Services.AddSingleton(token);
builder.Services.AddCustomAuthentication(token);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseRequestLocalizationByCulture();
app.UseRouting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.ConfigureSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
    RequestPath = new PathString("/Files")
});

//app.MapControllers();
app.MapControllerRoute(
    name:"default", 
    pattern:"{culture}/{controller}/{action}/{id?}",
    defaults: new {culture="tr-TR", controller = "Home", action="Index"}
    );

app.Run();
