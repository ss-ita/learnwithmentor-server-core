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
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddTransient<IPlanService, PlanService>();
            services.AddDbContext<LearnWithMentorContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // AddFluentValidation() adds FluentValidation services to the default container
            // Lambda-argument automatically registers each validator in this assembly 
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
