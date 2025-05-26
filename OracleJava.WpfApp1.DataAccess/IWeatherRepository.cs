using Oraclejava.WpfApp1.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OracleJava.WpfApp1.DataAccess;
public interface IWeatherRepository
{
    /// <summary>
    /// Retrieves the weather data for the specified city.
    /// </summary>
    /// <param name="city">The name of the city for which to retrieve weather data.</param>
    /// <returns>A Task that represents the asynchronous operation, containing a Weather object with the weather data.</returns>
    //Task<Weather> GetWeatherAsync(string city);
    //Task<List<dynamic>> GetFiveDayForecastAsync(string city, string apiKey);

    Task<Forecast> GetFiveDayForecastAsync(string city, string apiKey);

}
