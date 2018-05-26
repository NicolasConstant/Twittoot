using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Twittoot.Twitter.Oauth
{
    /// <summary>
    /// Interaction logic for TwitterOauth.xaml
    /// inspired from https://codereview.stackexchange.com/questions/36623/facebook-oauth-in-wpf
    /// </summary>
    public partial class TwitterOauth : Window
    {
        public TwitterOauth(string destinationUrl)
        {
            InitializeComponent();
            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                //Add the message hook in the code behind since I got a weird bug when trying to do it in the XAML
                webBrowser.MessageHook += webBrowser_MessageHook;

                //Delete the cookies since the last authentication
                //DeleteFacebookCookie();

                ////Create the destination URL
                //var destinationURL = String.Format("https://www.facebook.com/dialog/oauth?client_id={0}&scope={1}&display=popup&redirect_uri=http://www.facebook.com/connect/login_success.html&response_type=token",
                //    AppID, //client_id
                //    "email,user_birthday" //scope
                //);
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
            var codeElement = (wb.Document as dynamic).GetElementsByTagName("code")[0];

            if (codeElement == null) return;

            var code = codeElement.IHTMLElement_innerHTML;

            if (string.IsNullOrWhiteSpace(code)) return;

            Code = code;
            Close();
        }

        public string Code { get; set; }
    }
}
