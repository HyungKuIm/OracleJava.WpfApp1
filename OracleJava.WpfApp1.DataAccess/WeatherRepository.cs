using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oraclejava.WpfApp1.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleJava.WpfApp1.DataAccess;
public class WeatherRepository : IWeatherRepository
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<Forecast> GetFiveDayForecastAsync(string city, string apiKey)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric";

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JObject.Parse(json);

        var list = data["list"];
        var cityName = data["city"]?["name"]?.ToString() ?? city;

        // 날짜별 그룹핑
        var groupedByDate = list
            .GroupBy(item => DateTime.Parse(item["dt_txt"]!.ToString()).Date);

        var forecastDays = new List<ForecastDay>();

        foreach (var group in groupedByDate)
        {
            var hourlyWeatherList = new List<Weather>();
            var temperatures = new List<int>();

            foreach (var item in group)
            {
                var dateTime = DateTime.Parse(item["dt_txt"]!.ToString());
                int tempCelsius = (int)Math.Round(item["main"]!["temp"]!.ToObject<double>());
                temperatures.Add(tempCelsius);

                var weatherMain = item["weather"]![0]!["main"]!.ToString().ToLower();
                var weatherIcon = item["weather"]![0]!["icon"]!.ToString();

                WeatherDescription description = MapWeatherDescription(weatherMain);

                var weather = new Weather(
                    city: cityName,
                    dateTime: dateTime,
                    temperature: new Temperature(tempCelsius),
                    weatherDescription: description,
                    weatherIcon: weatherIcon
                );

                hourlyWeatherList.Add(weather);
            }

            int minTemp = temperatures.Min();
            int maxTemp = temperatures.Max();

            forecastDays.Add(new ForecastDay(hourlyWeatherList, group.Key, minTemp, maxTemp));
        }

        return new Forecast(forecastDays, cityName);
    }

    private WeatherDescription MapWeatherDescription(string main)
    {
        return main switch
        {
            "clear" => WeatherDescription.clear,
            "clouds" => WeatherDescription.cloudy,
            "sunny" => WeatherDescription.sunny,
            "rain" => WeatherDescription.rain,
            "snow" => WeatherDescription.snow,
            "thunderstorm" => WeatherDescription.thunder,
            _ => WeatherDescription.clear
        };
    }

    //public async Task<List<dynamic>> GetFiveDayForecastAsync(string city, string apiKey)
    //{
    //    string url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric";

    //    try
    //    {
    //        HttpResponseMessage response = await client.GetAsync(url);
    //        response.EnsureSuccessStatusCode();

    //        string json = await response.Content.ReadAsStringAsync();
    //        dynamic forecastData = JsonConvert.DeserializeObject(json);

    //        List<dynamic> forecastList = new List<dynamic>();

    //        foreach (var item in forecastData.list)
    //        {
    //            forecastList.Add(new
    //            {
    //                DateTime = item.dt_txt,
    //                Temperature = item.main.temp,
    //                Weather = item.weather[0].description,
    //                Icon = item.weather[0].icon
    //            });
    //        }

    //        return forecastList;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error fetching forecast: {ex.Message}");
    //        return new List<dynamic>();
    //    }
    //}


}
