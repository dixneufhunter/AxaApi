
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SurveyApi.Models;
using SurveyApi.Data;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

namespace SurveyApi
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

            ////// Konfigurasi JwtSettings
            //services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            ////// Menambahkan JwtTokenService
            //services.AddSingleton<Services.JwtTokenService>();

            //services.AddSingleton<IAuthorizationHandler, BearerAuthorizationHandler>();
            //services.AddSingleton<IAuthorizationHandler, BearerAuthorizationHandler>();

            //services.AddAuthorization(options => options.AddPolicy("Bearer",
            //    policy => policy.AddRequirements(new BearerRequirement())
            //    )
            //);

            string connectionString = Configuration.GetConnectionString("AxaAppCon");

            // Menambahkan Authentication dan JWT Bearer
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            services.AddAuthentication("Bearer")
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["JwtSettings:Issuer"],
                            ValidAudience = Configuration["JwtSettings:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"]))
                        };
                    });

            // Add authorization policies
            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManagerOnly", policy => policy.RequireClaim("Department", "Manager"));
                // Add more policies as needed
            });*/

            // Menambahkan services untuk container.
            services.AddControllers();

            // Menambahkan Serilog
            //DateTime _now = DateTime.Now;
            //string tanggal = Convert.ToString(_now);

            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Debug()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/myLog-.txt", rollingInterval: RollingInterval.Day) //menambahkan interval waktu perhari
                .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //   .ReadFrom.Configuration(builder).CreateLogger();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SurveyApi", Version = "v1" });

                // To Enable authorization using Swagger (JWT)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    //Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    //Scheme = "Bearer",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    //Type = ReferenceType.SecurityScheme,
                                    //Id = "Bearer"
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            //new string[] {}
                            Array.Empty<string>()
                    }
                });



            });

           

            //services.AddDbContext<ApplicationDbContext_Survey>(options => options.UseSqlServer(connectionString));
            //USER
            services.AddDbContext<AppDbContext_User>(options => options.UseSqlServer(connectionString));

            //PRODUCT
            services.AddDbContext<AppDbContext_Product>(options => options.UseSqlServer(connectionString));

            //services.AddControllers().AddNewtonsoftJson(o =>
            //{
            //    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //});

            //services.AddAuthorization(options =>
            //{

            //    options.AddPolicy("Admin",
            //        authBuilder =>
            //        {
            //            authBuilder.RequireRole("Administrators");
            //        });

            //});

            // Enables [Authorize] attribute
            services.AddAuthorization();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SurveyApi v1")); //OFF 20 Sept 2024 (IIS EXPRESS)

                string virDir = Configuration.GetSection("VirtualDirectory").Value;

                app.UseSwaggerUI(c =>
                {
                    //c.SwaggerEndpoint(virDir + "/swagger/v1/swagger.json", "SurveyApi v1"); //WITH VirDir
                    c.SwaggerEndpoint(virDir + "/swagger/v1/swagger.json", "SurveyApi v1");
                });

                //app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                //{
                //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SurveyApi v1");
                //    options.RoutePrefix = string.Empty;
                //});



            }

            // Mengaktifkan Serilog
            //app.UseSerilogRequestLogging();

            //app.UseHttpsRedirection();

            app.UseRouting();

            // Mengaktifkan Authentication (must come before Authorization)
            app.UseAuthentication();

            // Mengaktifkan Authorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
