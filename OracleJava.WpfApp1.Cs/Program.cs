using Oraclejava.WpfApp1.DataModels;
using OracleJava.WpfApp1.DataAccess;

namespace OracleJava.WpfApp1.Cs;

internal class Program
{
    static async Task Main(string[] args)
    {
        // This is just a placeholder for the main method.
        // You can implement your logic here or remove this method if not needed.
        IWeatherRepository weatherRepository = new WeatherRepository();
        string city = "Seoul"; // Example city
        string apiKey = "930de60fb21e865b708ed8a777d46835";

        List<Forecast> forecasts = await weatherRepository.GetFiveDayForecastAsync(city, apiKey);

        foreach (var forecast in forecasts)
        {
            Console.WriteLine($"city: {forecast.City}");
            foreach (var day in forecast.Days)
            {
                Console.WriteLine($"{day.Date:yyyy-MM-dd}| {day.HourlyWeather[0].WeatherDescription} | min: {day.Min}, max: {day.Max}");
            }
        }
    }
}
