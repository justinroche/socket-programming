using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

namespace SocketProgram
{
  class Server
  {
    static readonly string ip = "127.0.0.1";
    static readonly int port = 11000;

    static void Main(string[] args)
    {

      // Get the key and IV from the user.
      Console.Write("Enter the key for DES encryption (8 bytes): ");
      string key = Console.ReadLine() ?? string.Empty;
      key = key.PadRight(8)[..8];

      Console.Write("Enter the IV (8 bytes): ");
      string iv = Console.ReadLine() ?? string.Empty;
      iv = iv.PadRight(8)[..8];

      // Set up the DES object.
      DES des = DES.Create();
      des.Key = Encoding.ASCII.GetBytes(key);
      des.IV = Encoding.ASCII.GetBytes(iv);

      // Create the encryptor and decryptor.
      ICryptoTransform encryptor = des.CreateEncryptor();
      ICryptoTransform decryptor = des.CreateDecryptor();

      // Initialize the listener and bind it.
      Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      listener.Bind(new IPEndPoint(IPAddress.Parse(ip), port));

      // Listen for incoming connections.
      listener.Listen(1);
      Console.WriteLine("Server listening on " + ip + ":" + port + "...");

      while (true)
      {
        // Accept the incoming connection.
        Socket handler = listener.Accept();

        // Receive the message from the client.
        byte[] buffer = new byte[1024];
        int receivedLength = handler.Receive(buffer);

        // Decrypt the message.
        string decryptedMessage = Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, receivedLength));
        Console.WriteLine("Received: " + decryptedMessage);

        // Convert the message to uppercase and send it back.
        string response = decryptedMessage.ToUpper();
        byte[] responseBuffer = encryptor.TransformFinalBlock(Encoding.ASCII.GetBytes(response), 0, response.Length);
        handler.Send(responseBuffer);
        Console.WriteLine("Sent: " + response);

        // Shutdown and close the handler.
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
      }
    }
  }
}
