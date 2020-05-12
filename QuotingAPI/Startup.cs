using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using QuotingAPI.BusinessLogic;
using QuotingAPI.Database;
using QuotingAPI.Middlewares;

namespace QuotingAPI
{
    public class Startup
    {
        const string SWAGGER_SECTION_SETTING_KEY = "SwaggerSettings";
        const string SWAGGER_SECTION_SETTING_TITLE_KEY = "Title";
        const string SWAGGER_SECTION_SETTING_VERSION_KEY = "Version";
        
        public Startup(IWebHostEnvironment environment)
        {
            // Dynamic configuration 

            // "appsettings." + env.EnvironmentName + ".json" 
            //""-> String, {}-> Var

            var builder = new ConfigurationBuilder() 

                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build(); //-> Singleton
                
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<IQuotesLogic, QuotesLogic>();
            services.AddSingleton<IQuoteListDB, QuoteListDB>(); //Transient

            var swaggerTitle = Configuration
                .GetSection(SWAGGER_SECTION_SETTING_KEY)
                .GetSection(SWAGGER_SECTION_SETTING_TITLE_KEY);
                
            var swaggerVersion = Configuration
                .GetSection(SWAGGER_SECTION_SETTING_KEY)
                .GetSection(SWAGGER_SECTION_SETTING_VERSION_KEY);

            //  ENABLE SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc
                (
                    swaggerVersion.Value, 
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                         Title = swaggerTitle.Value, //"Quoting API - Group 5", 
                         Version = swaggerVersion.Value
                    }
                );
            });
        }

        // PIPELINE
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExcepHandlerMiddleware();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quoting");
            
            });
        }
    }
}
