using OracleJava.WpfApp1.DataAccess;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OracleJavaWpfLibrary.ViewModel;
public class WeatherViewModel : BaseViewModel
{
    private readonly IWeatherRepository weatherRepository;

    public ObservableCollection<ForecastDayDisplay> ForecastDays { get; set; } = new();


    public ReactivePropertySlim<string> City { get; }


    public ReactivePropertySlim<NowWeatherDisplay> NowWeatherDisplay { get; }


    public ReactivePropertySlim<string> TemperatureLabel { get; }

    public ICommand TemperatureCommand
    {
        get {  return new ActionCommand(action =>
            {
                if (TemperatureLabel.Value == "°C")
                {
                    TemperatureLabel.Value = "°F";
                    NowWeatherDisplay.Value.Temperature.Value = (int)(NowWeatherDisplay.Value.Temperature.Value * 9.0 / 5.0 + 32);
                    foreach (var day in ForecastDays)
                    {
                        day.Min.Value = (int)(day.Min.Value * 9.0 / 5.0 + 32);
                        day.Max.Value = (int)(day.Max.Value * 9.0 / 5.0 + 32);
                    }
                }
                else
                {
                    TemperatureLabel.Value = "°C";
                    NowWeatherDisplay.Value.Temperature.Value = (int)((NowWeatherDisplay.Value.Temperature.Value - 32) * 5.0 / 9.0);
                    foreach (var day in ForecastDays)
                    {
                        day.Min.Value = (int)((day.Min.Value - 32) * 5.0 / 9.0);
                        day.Max.Value  = (int)((day.Max.Value - 32) * 5.0 / 9.0);
                    }
                }
            });
        }
    }


    public WeatherViewModel()
    {
        this.City = new ReactivePropertySlim<string>("Seoul");
        this.NowWeatherDisplay = new ReactivePropertySlim<NowWeatherDisplay>();
        this.TemperatureLabel = new ReactivePropertySlim<string>("°C");

        weatherRepository = new WeatherRepository(); 
        _ = LoadWeatherDataAsync();

        

        //TemperatureLabel.Value = "°C";
    }

    // Fix for CS0747 and CS0103 errors in the LoadWeatherDataAsync method
    private async Task LoadWeatherDataAsync()
    {
        string apiKey = "930de60fb21e865b708ed8a777d46835"; // OpenWeatherMap API 키 입력

        var forecast = await weatherRepository.GetFiveDayForecastAsync(City.Value, apiKey);
        ForecastDays.Clear();

        foreach (var day in forecast.Days)
        {
            var desc = day.HourlyWeather.Count > 0 ? day.HourlyWeather[0].WeatherDescription.ToString() : "N/A";
            var forecastDayDisplay = new ForecastDayDisplay
            {
                Date = { Value = day.Date.ToString("yyyy-MM-dd") },
                Min = { Value = day.Min },
                Max = { Value = day.Max },
                Weather = { Value = desc }
            };
            ForecastDays.Add(forecastDayDisplay);
        }

        var nowDay = forecast.Days.FirstOrDefault();
        if (nowDay != null && nowDay.HourlyWeather.Count > 0)
        {
            var nowWeather = nowDay.HourlyWeather[0];
            NowWeatherDisplay.Value = new NowWeatherDisplay
            {
                City = { Value = nowWeather.City },
                DateTime = { Value = nowWeather.DateTime.ToString("yyyy-MM-dd HH:mm:ss") },
                Temperature = { Value = nowWeather.Temperature.current },
                WeatherDescription = { Value = nowWeather.WeatherDescription.ToString() },
                WeatherIcon = { Value = nowWeather.WeatherIcon }
            };
        }
        else
        {
            NowWeatherDisplay.Value = new NowWeatherDisplay
            {
                DateTime = { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                Temperature = { Value = 0 },
                WeatherDescription = { Value = "N/A" },
                WeatherIcon = { Value = string.Empty }
            };
        }
    }
}

public class ForecastDayDisplay
{

    public ReactivePropertySlim<string> Date { get; }
    public ReactivePropertySlim<int> Min { get; }
    public ReactivePropertySlim<int> Max { get; }
    public ReactivePropertySlim<string> Weather { get; }

    public ForecastDayDisplay()
    {
        Date = new ReactivePropertySlim<string>();
        Min = new ReactivePropertySlim<int>();
        Max = new ReactivePropertySlim<int>();
        Weather = new ReactivePropertySlim<string>();
    }
}

public class NowWeatherDisplay 
{

    public ReactivePropertySlim<string> City { get; }
    public ReactivePropertySlim<string> DateTime { get; }
    public ReactivePropertySlim<int> Temperature { get; }
    public ReactivePropertySlim<string> WeatherDescription { get; }
    public ReactivePropertySlim<string> WeatherIcon { get; }
    public NowWeatherDisplay()
    {
        this.City = new ReactivePropertySlim<string>();
        this.DateTime = new ReactivePropertySlim<string>();
        this.Temperature = new ReactivePropertySlim<int>();
        this.WeatherDescription = new ReactivePropertySlim<string>();
        this.WeatherIcon = new ReactivePropertySlim<string>();
    }
}