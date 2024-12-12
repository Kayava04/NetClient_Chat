using NetClient_Chat.Core;
using NetClient_Chat.Controls;
using System.Windows;
using System.Windows.Input;

namespace NetClient_Chat.Views
{
    public partial class MainWindow : Window
    {
        private string _ipAddress;
        private string _port;
        private Client _client;
        public string Username { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        #region BASE METHODS

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region LOAD PROGRAM

        private async void InitializeData()
        {
            if (_client != null)
            {
                _client.Disconnect();
                MessageBox.Show("Reconnect successfully!");
            }

            if (string.IsNullOrEmpty(_ipAddress) && string.IsNullOrEmpty(_port))
            {
                _ipAddress = "127.0.0.1";
                _port = "8080";

                MessageBox.Show("Data are set successfully!");
            }

            Username = UsernameTextBox.Text;

            try
            {
                if (Username == string.Empty)
                    Username = "User";
                _client = new Client(this, Username);
                await _client.Connect(_ipAddress, int.Parse(_port)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the server: {ex.Message}");
            }
        }

        #endregion

        #region CONTROL OPERATIONS

        private void StartChatingButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_ipAddress) && string.IsNullOrEmpty(_port) && string.IsNullOrEmpty(Username))
                MessageBox.Show("Please, connect to the server!");
            else ChatSection.Visibility = Visibility.Visible;
        }

        private void ShowSettingsSection_Click(object sender, RoutedEventArgs e)
        {
            TurnBackButton.Margin = new Thickness(0,0,10,0);
            TurnBackButton.Visibility = SettingsSection.Visibility = Visibility.Visible;
            ChatButton.Visibility = ChatSection.Visibility = SettingsButton.Visibility = Visibility.Collapsed;

            iPAddressTextBox.Text = _ipAddress;
            PortTextBox.Text = _port;
        }

        private void TurnBackButton_Click(object sender, RoutedEventArgs e)
        {
            TurnBackButton.Visibility = SettingsSection.Visibility = Visibility.Collapsed;
            ChatSection.Visibility = ChatButton.Visibility = SettingsButton.Visibility = Visibility.Visible;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            _ipAddress = iPAddressTextBox.Text;
            _port = PortTextBox.Text;

            try
            {
                InitializeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}");
            }
        }

        private void CancelChanges_Click(object sender, RoutedEventArgs e)
        {
            iPAddressTextBox.Text = PortTextBox.Text = string.Empty;
            _ipAddress = _port = string.Empty;

            MessageBox.Show("Please, set new IP and PORT!");
        }

        private async void InputText_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && inputText.Text != string.Empty)
                {
                    await _client.SendMessage(inputText.Text).ConfigureAwait(false);
                    inputText.Text = string.Empty;
                }
            }
            catch
            {
                inputText.Text = string.Empty;
                MessageBox.Show("You're not connected to the server!");
            }
        }

        private async void SentMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inputText.Text != string.Empty)
                {
                    await _client.SendMessage(inputText.Text).ConfigureAwait(false);
                    inputText.Text = string.Empty;
                }
            }
            catch
            {
                inputText.Text = string.Empty;
                MessageBox.Show("You're not connected to the server!");
            }
        }

        public void AddMessage(string username, string text, bool isMine = true)
        {
            var message = new Message(username, text, isMine);
            messages.Children.Add(message);
        }

        #endregion
    }
}