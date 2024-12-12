using NetClient_Chat.Views;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace NetClient_Chat.Core
{
    public class Client
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private MainWindow _mainWindow;
        private string _username;

        public Client(MainWindow mainWindow, string username)
        {
            _mainWindow = mainWindow;
            _username = username;
        }

        public async Task Connect(string ipAddress, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ipAddress, port).ConfigureAwait(false);
                _stream = _client.GetStream();

                _ = Task.Run(() => ReceiveMessages());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}");
            }
        }

        private async Task ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    dynamic recievedObject = JsonConvert.DeserializeObject(message);
                    
                    _mainWindow.Dispatcher.Invoke(() =>
                    {
                        _mainWindow.AddMessage((string)recievedObject.Username, (string)recievedObject.Message, isMine: false);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}");
            }
        }

        public async Task SendMessage(string message)
        {
            try
            {
                var myMessage = new { Username = _username, Message = message };
                string jsonMessage = JsonConvert.SerializeObject(myMessage);
                byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
                await _stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                
                _mainWindow.Dispatcher.Invoke(() =>
                {
                    _mainWindow.AddMessage(_username, message, isMine: true);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error message: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            _client.Close();
        }
    }
}