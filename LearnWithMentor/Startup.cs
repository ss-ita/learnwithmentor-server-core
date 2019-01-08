using FluentValidation.AspNetCore;
using LearnWithMentor.BLL.Interfaces;
using LearnWithMentor.BLL.Services;
using LearnWithMentor.Controllers;
using LearnWithMentor.DAL.Entities;
using LearnWithMentor.DAL.UnitOfWork;
using LearnWithMentorBLL.Interfaces;
using LearnWithMentorBLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading.Tasks;
using LearnWithMentor.DAL.Repositories.Interfaces;

namespace LearnWithMentor
{
    public class Startup
    {
        private readonly string ConnectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Environment.GetEnvironmentVariable("AzureConnection") ?? Configuration.GetConnectionString("DefaultConnection");
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Constants.Token.SecretString)),
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateActor = false
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && 
                                path.StartsWithSegments("/api/notifications"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserIdentityService, UserIdentityService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IGroupChatService, GroupChatService>();
            services.AddScoped<ITaskDiscussionService, TaskDiscussionService>();

            services.AddDbContext<LearnWithMentorContext>(options => 
                options.UseSqlServer(ConnectionString));

            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
                .AddEntityFrameworkStores<LearnWithMentorContext>()
                .AddDefaultTokenProviders();

            services.AddCors(o => o.AddPolicy(Constants.Cors.policyName, builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()));
            services.AddSignalR();

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fvConfig => fvConfig.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new Info { Title = "LearnWithMentor API" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(setup => setup.SwaggerEndpoint("/swagger/v1/swagger.json", "LearnWithMentor API"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            IdentityDataInitializer.SeedData(userManager, roleManager).Wait();
            app.UseCors(Constants.Cors.policyName);
            app.UseSignalR(routes => routes.MapHub<NotificationController>("/api/notifications"));
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
