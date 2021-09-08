using DotNetX.Azure.Storage.Blobs.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace KeyValueStorage.SampleApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KeyValueStorage.SampleApp", Version = "v1" });
            });

            services.Configure<BlobServiceClientSettings>("Files", options => options.ConnectionString = "UseDevelopmentStorage=true");
            services.AddNamedBlobServiceClient<BlobServiceClientSettings>("Files");

            services.Configure<BlobContainerClientSettings>("Files", options =>
            {
                options.ConnectionString = "UseDevelopmentStorage=true";
                options.Container = "files";
            });
            services.AddNamedBlobContainerClient<BlobContainerClientSettings>("Files");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeyValueStorage.SampleApp v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
