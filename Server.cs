using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.WebSockets;

public static class Server
{
    public static async Task<IPAddress> LoadIP()
    {
        using var client = new HttpClient();
        
        var response = await client.GetAsync("http://ipinfo.io/ip");
        response.EnsureSuccessStatusCode();
        string publicIpAddress = await response.Content.ReadAsStringAsync();
        return IPAddress.Parse(publicIpAddress.Trim());
    }

    public static async Task<string> Recive()
    {
        var ip = IPAddress.Parse("0.0.0.0");
        IPEndPoint endPoint = new(ip, 11_000);
        using var listener = new TcpListener(endPoint);
        listener.Start();

        using var client = await listener.AcceptTcpClientAsync();
        using var stream = client.GetStream();

        var buffer = new byte[1024];
        var bytesRead = await stream.ReadAsync(buffer);

        if (bytesRead != 0)
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        client.Close();

        return "no resposne";
    }

    public static async Task Send(string message, string ipAddress)
    {
        var ip = IPAddress.Parse(ipAddress.Trim());

        using var client = new TcpClient();
        await client.ConnectAsync(ip, 11_000);

        using var stream = client.GetStream();
        var buffer = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(buffer);

        client.Close();
    }
}