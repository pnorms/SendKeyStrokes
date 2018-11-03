using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SendKeyStrokes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        // Setup Vars
        private int _iTimeOut;
        private readonly DispatcherTimer _dTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            _dTimer.Interval = new TimeSpan(0, 0, 1);
            _dTimer.Tick += new EventHandler(CountdownTimerStep);
        }

        private void CountdownTimerStep(object sender, EventArgs e)
        {
            if (_iTimeOut > 0)
            {
                _iTimeOut--;
                tStatus.Text = "Sending in: " + _iTimeOut + " seconds...";
                tStatus.Foreground = Brushes.DarkBlue;
            }
            else
            {
                _dTimer.Stop();
                SendTheKeys();
            }
        }

        private void SendKeyString(string sKeys)
        {
            foreach (char cCur in sKeys)
            {
                string cToSend;
                string sSpecChars = "{[+^%~()]}";
                if (sSpecChars.Contains(cCur))
                {
                    cToSend = "{" + cCur + "}";
                }
                else
                {
                    cToSend = cCur.ToString();
                }
                SendKeys.SendWait(cToSend);
                System.Threading.Thread.Sleep(100);
            }
        }

        private void SendTheKeys()
        {
            // Send the keys or error
            try
            {
                // Username
                if (cUsername.IsChecked == true)
                {
                    SendKeyString(tUsername.Text);
                    // Send Tab
                    SendKeys.SendWait("{tab}");
                    System.Threading.Thread.Sleep(500);
                }
                // Password
                SendKeyString(tPassword.Password);
                // Clear the clipboard
                System.Windows.Clipboard.Clear();
                // Finish text
                tStatus.Text = "Keys sent, clipboard cleared!";
                tStatus.Foreground = Brushes.Green;
            }
            catch
            {
                tStatus.Text = "Error sending keys, contact admin.";
                tStatus.Foreground = Brushes.Red;
            }
        }

        private void bSend_Click(object sender, RoutedEventArgs e)
        {
            //Show status
            tStatus.Visibility = Visibility.Visible;
            tStatus.Text = "Please wait sending.";
            tStatus.Foreground = Brushes.DarkBlue;
            //Get the timeout as int
            try
            {
                _iTimeOut = Int32.Parse(tTime.Text);
            }
            catch (FormatException)
            {
                tStatus.Text = "Error parsing timeout, please enter a number.";
                tStatus.Foreground = Brushes.Red;
                return;
            }
            // Start the Timer to send the keys
            _dTimer.Start();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cUsername_Checked(object sender, RoutedEventArgs e)
        {
            tUsername.IsEnabled = true;
            tStatus.Text = "After pasting your text above, click send and place your cursor in the USERNAME field.";
            tStatus.Foreground = Brushes.Blue;
        }
       
        private void cUsername_Unchecked(object sender, RoutedEventArgs e)
        {
            tUsername.IsEnabled = false;
            tStatus.Text = "After pasting your text above, click send and place your cursor in the PASSWORD field.";
            tStatus.Foreground = Brushes.Brown;
        }
    }

}