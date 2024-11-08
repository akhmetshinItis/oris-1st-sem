using System.Net;
using System.Text;

namespace my_http;

class Program
{
    static void Main(string[] args)
    {
        HttpListener server = new HttpListener();
        // установка адресов прослушки
        server.Prefixes.Add("http://localhost:8888/");
        server.Start(); // начинаем прослушивать входящие подключения
        
        Console.WriteLine("Server start");
        
        // получаем контекст
        var context = server.GetContext();
 
        var response = context.Response;
        // отправляемый в ответ код htmlвозвращает
        string responseText = File.ReadAllText("/Users/tagirahmetsin/<!DOCTYPE html>.html");
        byte[] buffer = Encoding.UTF8.GetBytes(responseText);
        // получаем поток ответа и пишем в него ответ
        response.ContentLength64 = buffer.Length;
        using Stream output = response.OutputStream;
        // отправляем данные
        output.WriteAsync(buffer);
        output.FlushAsync();
 
        Console.WriteLine("Запрос обработан");
 
        server.Stop();
        Console.WriteLine("Server stop");
        Console.ReadKey();
    }
}