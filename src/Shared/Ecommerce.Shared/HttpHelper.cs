using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Ecommerce.Shared
{
    public class HttpHelper
    {
        public async Task<T> performHttpPost<T>(string url, string body, Dictionary<string,string> headers,ILogger _logger)
        {
            try
            {

                HttpClient client = new HttpClient();
                foreach(var item in headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                string requestBody = JsonSerializer.Serialize(body);
                var response = await client.PostAsync(url, new StringContent(requestBody, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var content = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
                _logger.LogInformation("Successfully make post call");
                return content;
            }
            catch(Exception ex)
            {
                _logger.LogError("An error occurred while making Post call: " + ex.Message);
                return default(T);
            }

        }
    }
}
