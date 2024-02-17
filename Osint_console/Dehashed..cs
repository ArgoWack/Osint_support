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
                          WriteLine($"ID: {entry.Id}, Email: {entry.Email}, Ip_address: {entry.Ip_address}, Username: {entry.Username},Password: {entry.Password}, Hashed_password: {entry.Hashed_password}, Hash_type: {entry.Hash_type}, Name: {entry.Name},Vin: {entry.Vin}, Address: {entry.Address}, Phone: {entry.Phone}, Database_name: {entry.Database_name}");
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