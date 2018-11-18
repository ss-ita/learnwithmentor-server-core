using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using AspNetCoreCurrentRequestContext;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using LearnWithMentor.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnWithMentor
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserIdentityService, UserIdentityService>();

            services.AddDbContext<LearnWithMentorContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // AddFluentValidation() adds FluentValidation services to the default container
            // Lambda-argument automatically registers each validator in this assembly 
            services.AddMvc()
                .AddWebApiConventions()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
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
                app.UseHsts();
            }

            app.UseMiddleware<CurrentRequestContextMiddleware>();
            app.UseCurrentRequestContext();

            app.UseAuthentication();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
