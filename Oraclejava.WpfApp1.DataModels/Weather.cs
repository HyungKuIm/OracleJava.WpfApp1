using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oraclejava.WpfApp1.DataModels;

//날씨 관련 클래스 모음


public class Temperature
{
    public int current { get; }
    public TemperatureUnit temperatureUnit { get; }

    static int celsiusToFahrenheit(int celsius)
    {
        return (int)(celsius * 9.0 / 5.0 + 32);
    }


    public Temperature(int current, TemperatureUnit temperatureUnit = TemperatureUnit.Celsius)
    {
        this.current = current;
        this.temperatureUnit = temperatureUnit;
    }
   
}

public enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    //Kelvin
}

// 날씨상세
public enum WeatherDescription { clear, cloudy, sunny, rain, snow, thunder }

// 날씨 정보
public class Weather
{
    public string City { get; }
    public DateTime DateTime { get; }
    public Temperature Temperature { get; }

    public WeatherDescription WeatherDescription { get; }

    public string WeatherIcon { get; }

    public Weather(string city, DateTime dateTime, Temperature temperature, WeatherDescription weatherDescription,
        string weatherIcon)
    {
        this.City = city;
        this.DateTime = dateTime;
        this.Temperature = temperature;
        this.WeatherDescription = weatherDescription;
        this.WeatherIcon = weatherIcon;
    }

    public static Dictionary<WeatherDescription, string> WeatherIcons = new Dictionary<WeatherDescription, string>
    {
        { WeatherDescription.clear, "선명" },
        { WeatherDescription.cloudy, "흐림" },
        { WeatherDescription.sunny, "맑음" },
        { WeatherDescription.rain, "비" },
        { WeatherDescription.snow, "눈" },
        { WeatherDescription.thunder, "천둥/번개" }
    };

}


// 기상 예보(3시간 단위)
public class ForecastDay
{
    public List<Weather> HourlyWeather { get; }
    public DateTime Date { get; }
    public int Min { get; }
    public int Max { get; }

    public ForecastDay(List<Weather> hourlyWeather, DateTime date, int min, int max)
    {
        HourlyWeather = hourlyWeather;
        Date = date;
        Min = min;
        Max = max;
    }
}

// 기상 예보(5일)
public class Forecast
{
    public List<ForecastDay> Days { get; }
    public string City { get; }
    public Forecast(List<ForecastDay> days, string city)
    {
        Days = days;
        City = city;
    }


}