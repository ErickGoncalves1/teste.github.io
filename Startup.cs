using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using Microsoft.EntityFrameworkCore;
using PortalAdvanced.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using PortalAdvanced.DAO;

namespace PortalAdvanced
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object SchemesNamesConst { get; private set; }
        public static readonly LoggerFactory MyConsoleLoggerFactory
            = new LoggerFactory(new[] {
              new ConsoleLoggerProvider((category, level)
                => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Trace, true) });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseOracle(Configuration.GetConnectionString("DefaultConnection"), b => b.UseOracleSQLCompatibility("11")));

            //services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<MensagemDAO>();
            services.AddTransient<HomeDAO>();
            services.AddTransient<ExtratoDAO>();
            services.AddTransient<RelatorioDAO>();
            services.AddTransient<UsuarioDAO>();
            services.AddTransient<Helper>();
            

            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<AppDbContext>()
            //   .AddDefaultTokenProviders();

            //services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddAuthentication(o => {
            //    o.DefaultScheme = SchemesNamesConst.TokenAuthenticationDefaultScheme;
            //})
            //.AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(SchemesNamesConst.TokenAuthenticationDefaultScheme, o => { });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //configura o uso da Sessão
            services.AddMemoryCache();
            services.AddSession();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.Run(async (context) =>
            //{
            //    //Demo: Basic ODP.NET Core application for ASP.NET Core
            //    // to connect, query, and return results to a web page

            //    string conOracle = Configuration.GetConnectionString("DefaultConnection");
            //    //Configuration.GetConnectionString("DefaultConnection");

            ////Create a connection to Oracle			
            //string conString = "User Id=PCIPORTAL;Password=pciportal;" +

            //    //How to connect to an Oracle DB without SQL*Net configuration file
            //    //  also known as tnsnames.ora.
            //    "Data Source=192.168.42.230/ORCL";

            //    //How to connect to an Oracle DB with a DB alias.
            //    //Uncomment below and comment above.
            //    //"Data Source=<service name alias>;";

            //    using (OracleConnection con = new OracleConnection(conOracle))
            //    {
            //        using (OracleCommand cmd = con.CreateCommand())
            //        {
            //            try
            //            {
            //                con.Open();
            //                cmd.BindByName = true;

            //                //Use the command to display employee names from 
            //                // the EMPLOYEES table
            //                //cmd.CommandText = "select NOME from USUARIOS where department_id = :id";
            //                cmd.CommandText = "select texto from mensagens ";
            //                // Assign id to the department number 50 
            //                //OracleParameter id = new OracleParameter("id", 50);
            //                //cmd.Parameters.Add(id);

            //                //Execute the command and use DataReader to display the data
            //                OracleDataReader reader = cmd.ExecuteReader();
            //                while (reader.Read())
            //                {
            //                    await context.Response.WriteAsync("Mensagens: " + reader.GetString(0) + "\n");
            //                }

            //                reader.Dispose();
            //            }
            //            catch (Exception ex)
            //            {
            //                await context.Response.WriteAsync(ex.Message);
            //            }
            //        }
            //    }

            //});

            app.UseHttpsRedirection();
            //app.UseDefaultFiles();
            app.UseStaticFiles();            
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Home",
                    template: "{controller=Login}/{action=Login}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
