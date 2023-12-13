using System.Data.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using WebApp.DAL;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<DbConnection>(_ => new NpgsqlConnection(Configuration.GetConnectionString("SomeDatabase")))
                .AddTransient<ISomeRepository, SomeRepository>()

                //Test Life Cycle
                .AddSingleton<ISingleton, TestLifeCycle>()
                .AddTransient<ITransient, TestLifeCycle>()
                .AddScoped<IScoped, TestLifeCycle>()

                //.AddSingleton<ISomeRepository>(p =>
                //{ 
                //    var db = p.GetRequiredService<DbConnection>();
                //    var logger = p.GetRequiredService<ILogger<SomeRepository>>();

                //    return new SomeRepository(db, logger);  

                //})

                .AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName.Replace("+", "_"));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Advertisement Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ""));

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}