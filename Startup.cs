using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AutoMapper;
using OnlineStoreProject_Intf;
using OnlineStoreProject.Services.CustomerService;
using OnlineStoreProject.Services.AuthenticationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using OnlineStoreProject_Intf.IAuthenticationService;
using OnlineStoreProject.Data.DataContext;

namespace OnlineStoreProject
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
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddDbContext<DataContext>(x=> x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineStoreProject", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                    Description = "Standart Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey});
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineStoreProject v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
