using System;
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
        public MastodonOauth(string destinationUrl)
        {
            InitializeComponent();
            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                //Add the message hook in the code behind since I got a weird bug when trying to do it in the XAML
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
            Close();
        }

        public string Code { get; set; }
    }
}
