
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

using ToDoApi.Database;
using ToDoApi.Services;
using ToDoApi.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.Text;
using System;

namespace ToDoWebApp
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
            services.AddDbContext<ToDoApiContext>(opt => opt.UseInMemoryDatabase("ToDoApiDatabase"));
            services.AddCors();

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore) //ignores self reference object 
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1); //validate api rules

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/clientapp/dist";
            });

            var key = Encoding.ASCII.GetBytes("DeloitteToDoApiWebApp");
            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                        .AddJwtBearer(x =>
                        {
                         /*   x.Events = new JwtBearerEvents
                            {
                                OnTokenValidated = context =>
                                {
                                    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                                    var userId = int.Parse(context.Principal.Identity.Name);
                                    var user = userService.GetById(userId);
                                    if (user == null)
                                    {
                                        // return unauthorized if user no longer exists
                                        context.Fail("Unauthorized");
                                    }
                                    return Task.CompletedTask;
                                }
                            };*/
                            x.RequireHttpsMetadata = false;
                            x.SaveToken = true;
                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                                ValidateIssuer = false,
                                ValidateAudience = false
                            };
                        });

            // configure DI for application services
            services.AddScoped<IToDoApiContext, ToDoApiContext>();
            services.AddScoped<IUserService, UserService>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ToDoApiContext>();
                AddUserData(context);
            }

            // global cors policy

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static void AddUserData(ToDoApiContext context)
        {
            var testUser1 = new ToDoUser
            {
                Id = "test",
                Password = "pwd123",
                FirstName = "Test",
                LastName = "User"
            };

            context.ToDoUsers.Add(testUser1);

            var testPost1 = new ToDoItem
            {
                Id = Guid.NewGuid().ToString(),
                ToDoUser = testUser1,
                ToDoText = "Deloitte ToDo List!",
                Complete = false
            };

            context.ToDoItems.Add(testPost1);

            context.SaveChanges();
        }

    }
}
