using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieTickets.Domain;
using MovieTickets.Domain.Identity;
using MovieTickets.Repository;
using MovieTickets.Repository.Implementation;
using MovieTickets.Repository.Interface;
using MovieTickets.Services;
using MovieTickets.Services.Implementation;
using MovieTickets.Services.Interface;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTickets.Web
{
    public class Startup
    {
        //private EmailSettings mailService;

        public Startup(IConfiguration configuration)
        {
            //mailService = new EmailSettings();
            Configuration = configuration;
            //Configuration.GetSection("EmailSettings").Bind(mailService);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<MoviesUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(ITicketRepository), typeof(TicketRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));

            //services.AddScoped<EmailSettings>(es => mailService);
            //services.AddScoped<IEmailService, EmailService>(email => new EmailService(mailService));
            //services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
            //services.AddHostedService<ConsumeScopedHostedService>();


            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            

            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<ITicketService,TicketService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOrderService, Services.Implementation.OrderService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
