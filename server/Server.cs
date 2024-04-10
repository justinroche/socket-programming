using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketProgram
{
  class Server
  {

    static readonly string ip = "127.0.0.1";
    static readonly int port = 11000;

    static void Main(string[] args)
    {
      Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Initialize the listener.
      listener.Bind(new IPEndPoint(IPAddress.Parse(ip), port)); // Bind the listener to the IP and port.

      // Listen for incoming connections.
      listener.Listen(1);
      Console.WriteLine("Server listening on " + ip + ":" + port + "...");

      while (true)
      {
        Socket handler = listener.Accept(); // Accept a new connection and get the client socket.

        // Receive the message from the client.
        byte[] buffer = new byte[1024];
        handler.Receive(buffer);

        // TODO: Decrypt the message.

        string request = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
        Console.WriteLine("Received " + request);

        // Convert the message to uppercase and send it back.
        string response = request.ToUpper();
        byte[] responseBuffer = Encoding.UTF8.GetBytes(response);

        // TODO: Encrypt the response.

        handler.Send(responseBuffer);

        // Shutdown and close the handler.
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
      }

    }
  }
}
