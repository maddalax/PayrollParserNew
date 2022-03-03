using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.UseFileServer();

Task.Run(() =>
{
    try
    {
        var url = "http://localhost:5000";
        Task.Delay(3000);
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", url);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", url);
        }
        else
        {
            // throw 
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Go to http://localhost:5000 in your browser");
    }
});

app.Run("http://localhost:5000");


