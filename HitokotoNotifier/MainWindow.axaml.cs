
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Manganese.Text;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;

namespace HitokotoNotifier
{
    public partial class MainWindow : Window
    {
        private Timer _timer = new Timer();
        public MainWindow()
        {
            InitializeComponent();

            _timer.Interval = 1000 * 3600;
            _timer.Elapsed += async (_, _) =>
            {
                var content = await GetHitokoto();
                ShowToast(content.Item1, content.Item2);
            };
            _timer.Enabled = true;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            ShowToast("Develop by AHpx", "API: v1.hitokoto.cn");
        }

        private void AutoStartCheckBox_OnChecked(object? sender, RoutedEventArgs e)
        {
            SetAutoStart(true);
        }
        
        private void AutoStartCheckBox_OnUnchecked(object? sender, RoutedEventArgs e)
        {
            SetAutoStart(false);
        }
        
        public static void SetAutoStart(bool enable)
        {
            const string appName = "HitokotoNotifier";
            RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (enable)
            {
                registryKey?.SetValue(appName, System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                registryKey?.DeleteValue(appName, false);
            }
        }

        // private void IntervalTextBox_OnTextInput(object? sender, TextInputEventArgs e)
        // {
        //     ShowToast("Changed", IntervalTextBox.Text);
        // }

        public static async Task<(string, string)> GetHitokoto()
        {
            var api = "https://v1.hitokoto.cn/?encode=json&c=k&c=i";
            var client = new HttpClient();
            var content = await client.GetStringAsync(api);

            return (content.Fetch("from_who"), content.Fetch("hitokoto"))!;
        }
        
        public static void ShowToast(string title, string content)
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(title)
                .AddText(content)
                .Show();
        }
    }
}