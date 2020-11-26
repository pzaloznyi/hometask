using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace hometasksolution
{
    class Program
    {
        private static async Task Main()
        {
            var httpClient = new HttpClient();
            var externalIp = await httpClient.GetStringAsync("http://icanhazip.com");
            var ipLocationJson = await httpClient.GetStringAsync($"http://ip-api.com/json/{externalIp}");
            var ipLocation = JsonConvert.DeserializeObject<IpLocation>(ipLocationJson);
            var url = $"http://api.openweathermap.org/data/2.5/weather?lat={ipLocation.lat}&lon={ipLocation.lon}&appid={openWeatherMapAppId}";
            var weatherJson = await httpClient.GetStringAsync(url);
            var weather = JsonConvert.DeserializeObject<WeatherObject>(weatherJson);
            var weat = weather.weather.First();
            Console.WriteLine($"The weather in {weather.name} is {weat.main} and {weat.description}. Current temperature {weather.main.Temp} {(Math.Abs(weather.main.Temp - weather.main.FeelsLike) == 0 ? "and" : "but")} feels like {weather.main.FeelsLike}");
        }
    }

    public class IpLocation
    {
        public string lat;
        public string lon;
    }
    
    public class WeatherObject
    {
        public string name;
        public Main main;
        public Weather[] weather;
        public class Main
        {
            private const double Calvin = -273;
            private double _temp;
            private double _feelsLike;
            public double Temp
            {
                get => _temp;
                set => _temp = Calvin + value;
            }
            public double FeelsLike
            {
                get => _feelsLike;
                set => _feelsLike = Calvin + value;
            }
        }

        public class Weather
        {
            public string main;
            public string description;
        }
    }
}
