namespace ToDoAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) 
            => services.AddCors(options 
                => { 
                    options.AddPolicy("CorsPolicy", 
                        builder => builder.AllowAnyOrigin().
                                            AllowAnyMethod().
                                            AllowAnyHeader()); 
                });
        // WithOrigins("https://example.com")
        // WithMethods("POST", "GET")
        // WithHeaders("accept", "content-type")

        public static void ConfigureIISIntegration(this IServiceCollection services) 
            => services.Configure<IISOptions>(options => 
            { 
            });
    }
}
