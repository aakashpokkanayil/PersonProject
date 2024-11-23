using Microsoft.EntityFrameworkCore;
using PersonData.PersonsContext;
using PersonProject.Filters.ActionFilters;
using RepositoryContracts.Interfaces;
using RepositoryProject.CountryRepository;
using RepositoryProject.PersonRepository;
using ServiceContracts.Interfaces.Country;
using ServiceContracts.Interfaces.Person;
using Services.CountryService;
using Services.Mapper;
using Services.PersonService;

namespace PersonProject.StartupExtensions
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services ,IConfiguration configuration)
        {
            services.AddControllersWithViews(options => {
                // options.Filters.Add<ResponseHeaderActionFilter>();// this is for filters without parameter 
                //ILogger<ResponseHeaderActionFilter> logger = services.BuildServiceProvider()
                //.GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                options.Filters.Add(new ResponseHeaderActionFilter("X-CustomGlobal-Key", "Custom-Value", 1));
            });
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsService, PersonsServices>();
            services.AddScoped<ICountryRepository, CountriesRepository>();
            services.AddScoped<IPersonRepository, PersonsRepository>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("PersonsConnectionString"));
            });

            //Services.AddHttpLogging(options =>
            //options.LoggingFields =HttpLoggingFields.RequestProperties|HttpLoggingFields.ResponsePropertiesAndHeaders);
        }
    }
}
 