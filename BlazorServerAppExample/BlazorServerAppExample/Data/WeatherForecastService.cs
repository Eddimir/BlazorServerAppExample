using System.Text.Json;
using System.Text.Json.Serialization;
namespace BlazorServerAppExample.Data
{
    public class WeatherForecastService
    {
        //private static readonly string[] Summaries = new[]
        //{        
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"   
        //};

        //public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        //{
        //    return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = startDate.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    }).ToArray());
        //}
        
     

        public async Task<WeatherData> GetWeatherData(string cityOrCountryName)
        {
            if (string.IsNullOrEmpty(cityOrCountryName)) throw new Exception("No valores en blanco");

            WeatherData? weather = null;
            const string apiKey = "4d8fb5b93d4af21d66a2948710284366";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityOrCountryName}&appid={apiKey}&units=metric";
            using (var httpclient = new HttpClient())
            {
                try
                {
                    using var httpResponse = await httpclient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        using var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                        try
                        {
                            weather = await JsonSerializer.DeserializeAsync<WeatherData?>(contentStream,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                        catch (Exception ex) 
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return weather;
        }
    }
}