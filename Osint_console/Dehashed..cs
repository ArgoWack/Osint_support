using static System.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Osint_console
{
    public class Dehashed
    {

        //https://www.dehashed.com/docs
        // up to 5 request a sec, meaning requests not more often than 200ms each

        private const string BaseUrl = "https://api.dehashed.com/search";
        private static readonly HttpClient client = new HttpClient();

        public static async Task CheckIfEmailHasBeenLeaked(string email, string dehashedApiUsername, string dehashedApiKey, string myApiName)
        {
            // constructs the auth header
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{dehashedApiUsername}:{dehashedApiKey}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            client.DefaultRequestHeaders.UserAgent.ParseAdd(myApiName + "/Version");
            client.DefaultRequestHeaders.Accept.ParseAdd("application / json");
            //\" -H \"Accept: application/json\"

            // prepares the request
            string url = $"{BaseUrl}?query={Uri.EscapeDataString(email)}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    //WriteLine(content);

                    var dehashedResponse = JsonSerializer.Deserialize<DehashedResponse>(content);

                    if (dehashedResponse?.Entries != null && dehashedResponse.Entries.Any())
                    {
                        WriteLine($"Leak information for {email}:");
                        foreach (var entry in dehashedResponse.Entries)
                        {
                            WriteLine($"ID: {entry.Id}, Email: {entry.Email}, Leak: {entry.Name}, Date: {entry.BreachDate}");
                        }
                    }
                    else
                    {
                        WriteLine($"{email} has not been found in any leaks.");
                    }
                }
                else
                {
                    WriteLine($"Failed to retrieve data: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                WriteLine($"Error fetching data: {e.Message}");
            }
        }

        public class DehashedResponse
        {
            [JsonPropertyName("total")]
            public int Total { get; set; }

            [JsonPropertyName("entries")]
            public List<Entry> Entries { get; set; }
        }

        public class Entry
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("breach_date")]
            public string BreachDate { get; set; }
        }
    }
}