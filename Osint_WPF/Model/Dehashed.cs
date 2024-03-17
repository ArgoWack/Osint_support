using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
namespace Osint_WPF.Model
{
    public class Dehashed
    {
        //https://www.dehashed.com/docs
        // up to 5 request a sec, meaning requests not more often than 200ms each

        private const string BaseUrl = "https://api.dehashed.com/search";
        private readonly HttpClient client = new HttpClient();

        public Dehashed(string dehashedApiUsername, string dehashedApiKey, string myApiName)
        {
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{dehashedApiUsername}:{dehashedApiKey}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(myApiName);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }
        public async Task<List<Entry>> CheckIfDataHasBeenLeaked(string data)
        {
            List<Entry> entries = new List<Entry>();
            string url = $"{BaseUrl}?query={Uri.EscapeDataString(data)}";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        string content = await response.Content.ReadAsStringAsync();
                        var dehashedResponse = JsonSerializer.Deserialize<DehashedResponse>(content);
                        if (dehashedResponse?.Entries != null && dehashedResponse.Entries.Any())
                        {
                            return dehashedResponse.Entries;
                        }
                        break;

                    case System.Net.HttpStatusCode.Unauthorized:
                        MessageBox.Show("Unauthorized: Check your API username and API key.");
                        break;

                    case System.Net.HttpStatusCode.Forbidden:
                        MessageBox.Show("Forbidden: You do not have access to the requested resource.");
                        break;

                    case System.Net.HttpStatusCode.TooManyRequests:
                        MessageBox.Show("Rate limit exceeded: You have made too many requests in a short period.");
                        break;

                    case System.Net.HttpStatusCode.InternalServerError:
                        MessageBox.Show("Internal Server Error: The server encountered an unexpected condition.");
                        break;

                    case System.Net.HttpStatusCode.ServiceUnavailable:
                        MessageBox.Show("Service Unavailable: The server is currently unable to handle the request due to temporary overloading or maintenance.");
                        break;

                    default:
                        MessageBox.Show($"An unexpected error occurred: {response.StatusCode}");
                        break;
                }
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"HttpRequestException: {e.Message}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred: {e.Message}");
            }
            return entries;
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
            // Fields: id, email, ip_address, username, password, hashed_password, hash_type, name, vin, address, phone, database_name

            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("ip_address")]
            public string Ip_address { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonPropertyName("password")]
            public string Password { get; set; }

            [JsonPropertyName("hashed_password")]
            public string Hashed_password { get; set; }

            [JsonPropertyName("hash_type")]
            public string Hash_type { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("vin")]
            public string Vin { get; set; }

            [JsonPropertyName("address")]
            public string Address { get; set; }

            [JsonPropertyName("phone")]
            public string Phone { get; set; }

            [JsonPropertyName("database_name")]
            public string Database_name { get; set; }
        }
    }
}