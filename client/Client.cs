using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

namespace SocketProgram
{
  class Client
  {

    static readonly string ip = "127.0.0.1";
    static readonly int port = 11000;

    static void Main(string[] args)
    {
      // Get the key and IV from the user.
      Console.Write("Enter the key for DES encryption: ");
      string key = Console.ReadLine() ?? string.Empty;
      key = key.PadRight(8)[..8];

      Console.Write("Enter the IV: ");
      string iv = Console.ReadLine() ?? string.Empty;
      iv = iv.PadRight(8)[..8];

      // Set up the DES object.
      DES des = DES.Create();
      des.Key = Encoding.ASCII.GetBytes(key);
      des.IV = Encoding.ASCII.GetBytes(iv);

      // Create the encryptor and decryptor.
      ICryptoTransform encryptor = des.CreateEncryptor();
      ICryptoTransform decryptor = des.CreateDecryptor();

      // Initialize the sender and connect it.
      Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      sender.Connect(ip, port);

      // Read and send the encrypted message.
      Console.WriteLine("Enter a message to send to the server:");
      string message = Console.ReadLine() ?? string.Empty;
      byte[] encryptedMessageBuffer = encryptor.TransformFinalBlock(Encoding.ASCII.GetBytes(message), 0, message.Length);
      sender.Send(encryptedMessageBuffer);

      Console.WriteLine("Message sent to the server.");
      Console.WriteLine("Waiting for response...");

      // Receive the response from the server.
      byte[] buffer = new byte[1024];
      int receivedLength = sender.Receive(buffer);

      // Decrypt and print the message.
      string decryptedResponse = Encoding.ASCII.GetString(decryptor.TransformFinalBlock(buffer, 0, receivedLength));

      // Convert the response to a string and print it.
      Console.WriteLine("Response from server: " + decryptedResponse);

      // Shutdown and close the sender.
      sender.Shutdown(SocketShutdown.Both);
      sender.Close();
    }
  }
}
