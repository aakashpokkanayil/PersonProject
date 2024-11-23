using Serilog;
using PersonProject.StartupExtensions;
using PersonProject.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices(builder.Configuration);
builder.Host.UseSerilog((HostBuilderContext hostBuilderContext,IServiceProvider service,LoggerConfiguration loggerConfiguration ) 
    => { 
        loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration)// read serilog config from built in
                                                                                    // IConfiguration means read from appsettings.
        .ReadFrom.Services(service) // This line allows Serilog to use objects (like settings or tools)
                                    // that are already registered in your app's Dependency Injection (DI) system
                                    // Hey Serilog, you can use the services that my app has already created or registered.
        ;
    }); 

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}
app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
