using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetServer
{
    public class Server
    {
        private TcpListener _listener;
        private List<TcpClient> _clients;

        public Server(string ipAddress, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _clients = new List<TcpClient>();
        }

        public async Task Start()
        {
            try
            {
                _listener.Start();
                Console.WriteLine("Server is running...\n");

                while (true)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    Console.WriteLine("New client connected...\n");

                    _clients.Add(client);
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Client: {message}");

                    foreach (var otherClient in _clients)
                    {
                        if (otherClient != client)
                        {
                            NetworkStream otherStream = otherClient.GetStream();
                            await otherStream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                _clients.Remove(client);
                client.Close();
            }
        }
    }
}