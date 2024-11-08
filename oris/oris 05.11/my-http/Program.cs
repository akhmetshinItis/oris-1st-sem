using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace my_http;

class Program
{
    static void Main(string[] args)
    {
        var server = new MyHttpServer("public/index.html");
        server.Start();
        Console.ReadKey();
        server.Stop();
    }

}

