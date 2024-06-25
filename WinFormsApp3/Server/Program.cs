using System.Net.Sockets;
using System.Net;
using System.Numerics;
using System.Drawing;
using System.IO;
using System.Text;

class Server
{
    public static bool mv = false;
    public static List<TcpClient> Clients = new List<TcpClient>();
    public static async Task ProcessClient(TcpClient client)
    {

        var st = client.GetStream();
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(mv) + '\0';
        byte[] data = Encoding.UTF8.GetBytes(json);
        st.WriteAsync(data);




        bool UserOnServer = true;
        while (UserOnServer)
        {
            try
            {
                var stream = client.GetStream();
                List<byte> bytes = new List<byte>();
                int bytesRead = 0;

                while ((bytesRead = stream.ReadByte()) != '\0')
                {
                    bytes.Add((byte)bytesRead);
                }

                bytes.Add((byte)'\0');


                for (int i = 0; i < Clients.Count; i++)
                {

                    if (client.Connected)
                    {
                        var sendMessageStream = Clients[i].GetStream();
                        _ = sendMessageStream.WriteAsync(bytes.ToArray());
                    }
                }


                bytes.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred.");
                return;
            }
        }
    }


    public static async Task Main(string[] args)
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Parse("192.168.89.189"), 9010);
        tcpListener.Start();

        Console.WriteLine("Server started..");

        Random random = new Random();
        mv = Convert.ToBoolean(random.Next(0,1));

        while (true)
        {
            try
            {

                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                await Console.Out.WriteLineAsync("Client connected..");

                Clients.Add(tcpClient);
                _ = Task.Run(async () => await ProcessClient(Clients[Clients.Count - 1]));

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}