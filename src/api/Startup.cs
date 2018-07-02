using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Titan.UFC.Users.WebAPI.Mappers;
using Titan.UFC.Users.WebAPI.Repositories;
using Titan.UFC.Users.WebAPI.Resources;
using Titan.UFC.Users.WebAPI.Services;

namespace Titan.UFC.Users.WebAPI
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
            // User Related
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Address Related
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();

            // Contact Related
            services.AddScoped<IContactsService, ContactsService>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();

            //Forgot Password related
            services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();

            // Mappers
            services.AddAutoMapper(x => x.AddProfile(new MappingsProfile()));

            // Connection String
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Localization
            services.AddSingleton<ISharedResource, SharedResource>();
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.AddCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddDataAnnotationsLocalization(options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };                    
                }); 
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseCors(b => b.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseMvc();
        }
    }
}
