using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyHotelListing.Configuration;
using MyHotelListing.Data;
using MyHotelListing.IRepository;
using MyHotelListing.Services;

namespace MyHotelListing
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

            services.AddDbContext<DatabaseContext>(option =>
            option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication();

            services.ConfigureIdentity();

            services.ConfigureJWT(Configuration);   

            // اجازه می دهد تمای متد ها هدر ها و ارجین هاا
            services.AddCors(o => o.AddPolicy("AllowAny", builder =>
               builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()));

            services.AddAutoMapper(typeof(Mapperinitilizaer));

            services.AddTransient<IUnitOfWork, UnitOfWork>();// همیشه نیاز دارد هربار یک نمونه ازش را بسازیم
            //services.AddSingleton//فقط یک نمونه برای کل برنامه نیاز است
            //services.AddScoped  //نباز دارد یک نمونه از ان رابسازیم برای هر life time 
            services.AddScoped<IAuthManager, AuthManager>();



            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyHotelListing", Version = "v1" });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyHotelListing v1"));

            app.UseHttpsRedirection();

            app.UseCors("AllowAny");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "Defualt",
                //    pattern: "{controler=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
