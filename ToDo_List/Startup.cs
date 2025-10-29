using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDo_List.BLL.Interfaces;
using ToDo_List.BLL.Repositories;
using ToDo_List.PL.MappingProfiles;
using ToDO_List.DAL.Data;



namespace ToDo_List
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
            services.AddControllersWithViews();

            services.AddDbContext<ToDoDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<TasksRepository>();
            services.AddScoped<ProjectRepository>();

            services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            services.AddIdentity<IdentityUser , IdentityRole>(
                config =>{
                    config.Password.RequiredLength = 8;
                    config.Password.RequireDigit = true;
                    config.Password.RequireNonAlphanumeric = true;
                    config.Password.RequireUppercase = true;
                    config.Password.RequireLowercase = true;
                    config.User.RequireUniqueEmail = true;
                    config.SignIn.RequireConfirmedEmail = true;
                }
                ).AddEntityFrameworkStores<ToDoDbContext>().AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.ConfigureApplicationCookie(options =>
             {
                 options.LoginPath = "/Auth/SignIn";
             });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tasks}/{action=Index}/{id?}");
            });
        }
    }
}
