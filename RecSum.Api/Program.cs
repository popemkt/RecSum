namespace RecSum;

public class Program
{  
    public static async Task Main(string[] args) {  
        
        Console.WriteLine($"Starting RecSum api at {DateTime.UtcNow:o}");
        try
        {
            await BuildWebHost(args)
                .Build()
                .RunAsync();
            Console.WriteLine($"Stopping RecSum api at {DateTime.UtcNow:o}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Shutting down because exception occured at {DateTime.UtcNow:o}: \n {e.Message} \n {e.InnerException?.Message} \n {e.StackTrace}");
        }
    }  
    public static IHostBuilder BuildWebHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });  
}