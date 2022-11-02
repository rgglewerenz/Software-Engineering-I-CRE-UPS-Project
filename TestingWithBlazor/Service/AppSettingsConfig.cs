using DTO.AppSettings;
using TestingWithBlazor.Interface;

namespace TestingWithBlazor.Service
{
    public class AppSettingsConfig : IAppSettingsConfig
    {
        private readonly AppSettings settings;

        public AppSettingsConfig(AppSettings settings)
        {
            this.settings = settings;
        }

        public AppSettings GetAppSettings() { return settings; }
    }
}
