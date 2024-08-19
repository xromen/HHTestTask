using NReco.Logging.File;


namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add log file
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            string logFileName = Path.Combine(logFolder, DateTime.UtcNow.ToString("yyyy-MM-dd") + ".log");

            builder.Services.AddLogging(loggingBuilder => {
                loggingBuilder.AddFile(logFileName, append: true);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseWebSockets();

            app.MapControllers();

            app.Run();

            Console.WriteLine("Запуск");
        }
    }
}