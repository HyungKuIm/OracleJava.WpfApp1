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

    public WeatherViewModel()
    {
        weatherRepository = new WeatherRepository(); 
        _ = LoadWeatherDataAsync();
    }

    private async Task LoadWeatherDataAsync()
    {
        //string city = "Seoul";
        string apiKey = "930de60fb21e865b708ed8a777d46835"; // OpenWeatherMap API 키 입력

        var forecasts = await weatherRepository.GetFiveDayForecastAsync(City, apiKey);
        ForecastDays.Clear();

        foreach (var day in forecasts[0].Days)
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
    }
}

public class ForecastDayDisplay
{
    public string Date { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public string Weather { get; set; }
}