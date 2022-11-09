using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using Location_API_Interface.Interface;
using Location_API_Interface.Service;
using DTO.AppSettings;
using TestingWithBlazor.Interface;
using TestingWithBlazor.Service;
using ElectronNET.API.Entities;
using System.Security.Principal;

namespace TestingWithBlazor
{
    public class Startup
    {


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings = Configuration.GetSection("ApplicationSettings").Get<AppSettings>();
        }

        public IConfiguration Configuration { get; }
        public static BrowserWindow? window_ref { get; set; }
        public AppSettings AppSettings { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<IGPSService, GPSService>();
            services.AddScoped<IMapApi, MapApi>(x => new MapApi(AppSettings.BingSettings.API_KEY));
            services.AddScoped<IPackageHandler, PackageHandler>();
            services.AddScoped<IAppSettingsConfig, AppSettingsConfig>(x => new AppSettingsConfig(AppSettings));
            if (HybridSupport.IsElectronActive)
            {
                var _ref = await CreateWindow();
                window_ref = _ref;
            }
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

        }


        private BrowserWindowOptions get_options()
        {
            return new BrowserWindowOptions()
            {
                DarkTheme = true,
                Frame = false,
                BackgroundColor = "#2d313a"
            };
        }

        private async Task<BrowserWindow> CreateWindow()
        {
            
            Electron.Menu.SetApplicationMenu(new ElectronNET.API.Entities.MenuItem[] { });
            var window = await Electron.WindowManager.CreateWindowAsync(get_options());
            window.OnClosed += () => {
                Electron.App.Quit();
            };
            return window;
        }
    }
}
