
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tutorial6.Services;
using Tutorial6.Middleware;

namespace Tutorial6
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbService, SqlServerDbService>();
            services.AddControllers();
        }

         
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbService dbService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseMiddleware<CustomLoggingMiddleware>();

            app.Use(async (context, next) =>
            {

                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("No IndexHeader ERROR 97936178649283");
                    return;


                }
                else
                {
                    var index = context.Request.Headers["Index"].ToString();
                    var existIndex = dbService.ExistIndexNumber(index);
                    if (!existIndex)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("No IndexHeader ERROR 97936178649283");
                        return;


                    }

                }

                await next();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
