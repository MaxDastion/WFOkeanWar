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
    private static Pair<bool, bool> playersReady = new Pair<bool, bool>(false, false);
    public static async Task ProcessClient(TcpClient client, int playerCount)
    {
       
        if (playerCount == 1)
        {
            playersReady.First = true;
            mv = true;
        }
        else
        {
            playersReady.Second = true;
            mv = false;
        }
        var st1 = client.GetStream();
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(mv) + '\0';
        byte[] data = Encoding.UTF8.GetBytes(json);
        st1.WriteAsync(data);

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

                    if (client.Connected && (client != Clients[i]))
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
        int i = 0;
        while (true)
        {
            try
            {

                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                await Console.Out.WriteLineAsync("Client connected..");

                Clients.Add(tcpClient);
                i += 1;
                _ = Task.Run(async () => await ProcessClient(Clients[Clients.Count - 1], i));

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}


public class Pair<T, U>
{
    public Pair()
    {
    }
    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }
    public T First { get; set; }
    public U Second { get; set; }
};