using DTO;
using DTO.AppSettings;
using Location_API_Interface.Interface;
using Location_API_Interface.Service;
using Microsoft.Extensions.Configuration;

namespace DesktopApp
{
    public partial class Form1 : Form
    {
        AppSettings? appSettings;
        IMapApi? MapApi;
        List<Address>? TestData;
        Address? StartingAddress;
        IGPSService? GPSService;
        int test;

        string TestingString { get
            {
                return label1.Text;
            } set
            {
                label1.Text = value;
            }
        }


        public Form1(IConfigurationRoot config)
        {
            test = 0;
            InitializeComponent();
            AddConfigurations(config);
        }

        void ChangeOutputText(string newText)
        {
            TestingString = newText;
        }

        void AddConfigurations(IConfigurationRoot root)
        {
            appSettings = root.Get<AppSettings>();
            //Initlizes the api with the bing key
            MapApi = new MapApi(appSettings.BingSettings.API_KEY);
            TestData = appSettings.TestData.TestLocations;
            StartingAddress = appSettings.TestData.StartingAddress;
            GPSService = new GPSService();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void testButton_Click(object sender, EventArgs e)
        {
            TestingString = $"Hello {++test}";
        }

        private void testTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}