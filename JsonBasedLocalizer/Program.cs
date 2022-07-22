using JsonBasedLocalizer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomLocalization();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

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

//app.MapControllers();
app.MapControllerRoute(
    name:"default", 
    pattern:"{culture}/{controller}/{action}/{id?}",
    defaults: new {culture="tr-TR", controller = "Home", action="Index"}
    );

app.Run();
