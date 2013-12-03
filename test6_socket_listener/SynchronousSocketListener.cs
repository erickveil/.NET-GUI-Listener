using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SynchronousSocketListener
{
    public int port = 50503;

    // Incoming data from the client.
    public static string data = null;

    // This delegate has a signature that matches the method I want to call in the form
    public delegate void updateDataBoxCallback(string msg);

    public void StartListening(test6_socket_listener.Form1 form)
    {
        Thread this_thread = Thread.CurrentThread;
        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.
        // Dns.GetHostName returns the name of the 
        // host running the application.
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.port);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and 
        // listen for incoming connections.
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.
            while(true)
            {
                Console.WriteLine("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();

                data = null;

                // An incoming connection needs to be processed.
                
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                

                // Show the data on the console.
                Console.WriteLine("Text received : {0}", data);

                // This is how we use the delagate to pass data back to the form's text box
                form.tb_data.Invoke(new updateDataBoxCallback(form.updateDataBox), data);
                

                // Echo the data back to the client.
                byte[] msg = Encoding.ASCII.GetBytes(data);

                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();
    }

}