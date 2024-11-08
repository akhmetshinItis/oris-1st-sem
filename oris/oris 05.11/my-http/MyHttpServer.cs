using System.Net;
using System.Text;
using System.Threading.Channels;

namespace my_http;

public class MyHttpServer
{
    public int Port = 8080;

    private HttpListener _listener;

    public string? Path;

    private string PathToNotFoundFile = "private/not_found.html";

    public MyHttpServer(string path)
    {
        Path = path;
    }
    
    public void Start()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:{Port}/");
        _listener.Start();
        Console.WriteLine("Server start");
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Server stop");
    }

    private void Receive()
    {
       var context = _listener.GetContext();
       ProcessContext(context);
        Console.WriteLine("Receiving context");
    }
    
    private void ProcessContext(HttpListenerContext context)
    {
        if (File.Exists(Path))
        {
            Console.WriteLine("File index.html exists");
            RenderResponse(Path, context, 200);
            Console.WriteLine("Response sent");
            Receive(); 
        }
        else
        {
            Console.WriteLine("File index.html doesn't exist");
            RenderResponse(PathToNotFoundFile,context, 404);
            Stop();
        }
    }
    
    private HttpListenerResponse RenderResponse(string? pathToContent, HttpListenerContext context, int statusCode)
    {
        var response = context.Response;
        response.StatusCode = statusCode;
        var content = File.ReadAllText(pathToContent);
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.ContentLength64 = buffer.Length;
        var stream = response.OutputStream;
        stream.Write(buffer);
        stream.FlushAsync();
        stream.Close();
        Console.WriteLine("Response rendered");
        return response;
    }
}