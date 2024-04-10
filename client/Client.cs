using System.Net.Sockets;
using System.Text;

namespace SocketProgram
{
  class Client
  {

    static readonly int port = 11000;
    static readonly string ip = "127.0.0.1";

    static void Main(string[] args)
    {
      Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Initialize the sender.
      sender.Connect(ip, port); // Connect sender to the IP and port.

      // Prepare message to be sent.
      Console.WriteLine("Enter a message to send to the server:");
      string message = Console.ReadLine() ?? string.Empty;
      byte[] messageBytes = Encoding.ASCII.GetBytes(message);

      // TODO: Encrypt the message.

      sender.Send(messageBytes); // Send the message to the server.
      Console.WriteLine("Message sent to the server.");
      Console.WriteLine("Waiting for response...");

      // Receive the response from the server.
      byte[] buffer = new byte[1024];
      int responseBytes = sender.Receive(buffer);

      // TODO: Decrypt the response.

      // Convert the response to a string and print it.
      string response = Encoding.ASCII.GetString(buffer, 0, responseBytes);
      Console.WriteLine("Response from server: " + response);

      // Shutdown and close the sender.
      sender.Shutdown(SocketShutdown.Both);
      sender.Close();
    }
  }
}
