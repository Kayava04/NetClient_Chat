using System.Text;

namespace NetServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("//-- SERVER --\\\\" + "\n");

            Console.Write("Server IP: ");
            string ipAddress = Console.ReadLine();
            Console.Write("Server Port: ");
            string port = Console.ReadLine();

            if (ipAddress == string.Empty && port == string.Empty)
                ipAddress = "127.0.0.1"; port = "8080";

            var server = new Server(ipAddress, int.Parse(port));
            await server.Start().ConfigureAwait(false);

            Console.WriteLine("Press Enter to stop the server");
            Console.ReadLine();
        }
    }
}