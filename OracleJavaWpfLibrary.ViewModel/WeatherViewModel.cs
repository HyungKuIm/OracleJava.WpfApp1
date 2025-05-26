using OracleJava.WpfApp1.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleJavaWpfLibrary.ViewModel;
public class WeatherViewModel : BaseViewModel
{
    private readonly IWeatherRepository weatherRepository;

    public ObservableCollection<ForecastDayDisplay> ForecastDays { get; set; } = new();

    private string city = "Seoul";
    public string City
    {
        get => city;
        set
        {
            if (city != value)
            {
                city = value;
                OnPropertyChanged();
                _ = LoadWeatherDataAsync(); // 데이터 새로 고침
            }
        }
    }

    private NowWeatherDisplay nowWeatherDisplay = new();
    public NowWeatherDisplay NowWeatherDisplay
    {
        get => nowWeatherDisplay;
        set
        {
            nowWeatherDisplay = value;
            OnPropertyChanged();
        }
    }



    public WeatherViewModel()
    {
        weatherRepository = new WeatherRepository(); 
        _ = LoadWeatherDataAsync();
    }

    private async Task LoadWeatherDataAsync()
    {
        //string city = "Seoul";
        string apiKey = "930de60fb21e865b708ed8a777d46835"; // OpenWeatherMap API 키 입력

        var forecast = await weatherRepository.GetFiveDayForecastAsync(City, apiKey);
        ForecastDays.Clear();

        foreach (var day in forecast.Days)
        {
            var desc = day.HourlyWeather.Count > 0 ? day.HourlyWeather[0].WeatherDescription.ToString() : "N/A";
            ForecastDays.Add(new ForecastDayDisplay
            {
                Date = day.Date.ToString("yyyy-MM-dd"),
                Min = day.Min,
                Max = day.Max,
                Weather = desc
            });
        }

        var nowDay = forecast.Days.FirstOrDefault();
        if (nowDay != null && nowDay.HourlyWeather.Count > 0)
        {
            var nowWeather = nowDay.HourlyWeather[0];
            NowWeatherDisplay = new NowWeatherDisplay
            {
                City = nowWeather.City,
                DateTime = nowWeather.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Temperature = nowWeather.Temperature.current,
                WeatherDescription = nowWeather.WeatherDescription.ToString(),
                WeatherIcon = nowWeather.WeatherIcon
            };
        }
        else
        {
            NowWeatherDisplay = new NowWeatherDisplay
            {
                City = City,
                DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Temperature = 0,
                WeatherDescription = "N/A",
                WeatherIcon = string.Empty
            };
        }
    }
}

public class ForecastDayDisplay
{
    public string Date { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public string Weather { get; set; }
}

public class NowWeatherDisplay
{
    public string City { get; set; }
    public string DateTime { get; set; }
    public int Temperature { get; set; }
    public string WeatherDescription { get; set; }
    public string WeatherIcon { get; set; }
}