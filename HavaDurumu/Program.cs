using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Weather
{
    public string Temperature { get; set; }
    public string Wind { get; set; }
    public string Description { get; set; }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }
    public Weather Weather { get; set; }
    public Weather[] Forecast { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        await GetWeatherData("https://goweather.herokuapp.com/weather/istanbul");
        await GetWeatherData("https://goweather.herokuapp.com/weather/İzmir");
        await GetWeatherData("https://goweather.herokuapp.com/weather/ankara");
    }

    static async Task GetWeatherData(string apiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string response = await client.GetStringAsync(apiUrl);

                // İstanbul API'si NOT_FOUND hatası döndüğünde işlemi sonlandır
                if (response.Contains("NOT_FOUND"))
                {
                    Console.WriteLine($"City: {apiUrl.Split('/').Last()}");
                    Console.WriteLine("Weather data is not available.");
                    Console.WriteLine("--------------");
                    return;
                }

                WeatherForecast weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(response);

                Console.WriteLine($"City: {apiUrl.Split('/').Last()}");
                Console.WriteLine($"Date: {weatherForecast.Date.ToShortDateString()}");

                // Genel hava durumu bilgileri
                Console.WriteLine($"Temperature: {weatherForecast.Weather?.Temperature}");
                Console.WriteLine($"Description: {weatherForecast.Weather?.Description}");
                Console.WriteLine($"Wind: {weatherForecast.Weather?.Wind}");

                // 3 günlük hava durumu tahminleri
                Console.WriteLine("Forecast:");
                for (int i = 0; i < weatherForecast.Forecast.Length; i++)
                {
                    Console.WriteLine($"Day {i + 1}:");
                    Console.WriteLine($"  Temperature: {weatherForecast.Forecast[i]?.Temperature}");
                    Console.WriteLine($"  Wind: {weatherForecast.Forecast[i]?.Wind}");
                }

                Console.WriteLine("--------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from {apiUrl}: {ex.Message}");
            }
        }
    }
}