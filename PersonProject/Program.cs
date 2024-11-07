using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;
using Services.CountryService;
using Services.Mapper;
using Services.PersonService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService,PersonsServices>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
