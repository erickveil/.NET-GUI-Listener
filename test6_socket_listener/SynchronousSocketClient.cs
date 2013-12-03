using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class SynchronousSocketClient
{
    public string ip = Dns.GetHostName();
    public int port = 50503;

    public void StartClient()
    {
        byte[] bytes = new byte[1];

        try
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(this.ip);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, this.port);
            Socket sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(remoteEP);
                byte[] msg = Encoding.ASCII.GetBytes("\n");

                int bytesSent = sender.Send(msg);

                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}