using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Twittoot.Mastodon.Oauth
{
    /// <summary>
    /// Interaction logic for TwitterOauth.xaml
    /// inspired from https://codereview.stackexchange.com/questions/36623/facebook-oauth-in-wpf
    /// </summary>
    public partial class MastodonOauth : Window
    {
        //private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        //[DllImport("wininet.dll", SetLastError = true)]
        //private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        public MastodonOauth(string destinationUrl)
        {
            InitializeComponent();

            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                //Add the message hook in the code behind since I got a weird bug when trying to do it in the XAML
                //InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                WinInetHelper.SupressCookiePersist();

                webBrowser.MessageHook += webBrowser_MessageHook;
                webBrowser.Navigate(destinationUrl);
            };
        }

        private IntPtr webBrowser_MessageHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == 130)
            {
                this.Close();
            }
            return IntPtr.Zero;
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            dynamic doc = webBrowser.Document;
            var htmlText = doc.documentElement.InnerHtml; //Strangely needed to get OnLoadCompleted functionnal
        }

        private void WebBrowser_OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            var wb = (WebBrowser)sender;
            var codeElement = (wb.Document as dynamic).GetElementsByTagName("input")[0];

            if (codeElement == null || codeElement.IHTMLElement_className != "oauth-code") return;
            
            var code = codeElement.IHTMLInputElement_defaultValue;

            if (string.IsNullOrWhiteSpace(code)) return;

            Code = code;
            WinInetHelper.EndBrowserSession();
            Close();
        }

        public string Code { get; set; }
    }

    public static class WinInetHelper
    {
        public static bool SupressCookiePersist()
        {
            // 3 = INTERNET_SUPPRESS_COOKIE_PERSIST 
            // 81 = INTERNET_OPTION_SUPPRESS_BEHAVIOR
            return SetOption(81, 3);
        }

        public static bool EndBrowserSession()
        {
            // 42 = INTERNET_OPTION_END_BROWSER_SESSION 
            return SetOption(42, null);
        }

        static bool SetOption(int settingCode, int? option)
        {
            IntPtr optionPtr = IntPtr.Zero;
            int size = 0;
            if (option.HasValue)
            {
                size = sizeof(int);
                optionPtr = Marshal.AllocCoTaskMem(size);
                Marshal.WriteInt32(optionPtr, option.Value);
            }

            bool success = InternetSetOption(0, settingCode, optionPtr, size);

            if (optionPtr != IntPtr.Zero) Marshal.Release(optionPtr);
            return success;
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(
            int hInternet,
            int dwOption,
            IntPtr lpBuffer,
            int dwBufferLength
        );
    }
}
