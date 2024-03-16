using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
// string entryDetails = $"First Entry Details:\nEmail: {firstEntry.Email}, Username: {firstEntry.Username}, IP: {firstEntry.Ip_address}, Password: {firstEntry.Password}, Hashed Password: {firstEntry.Hashed_password}, Hash Type: {firstEntry.Hash_type}, Name: {firstEntry.Name}, Address: {firstEntry.Address}, Phone: {firstEntry.Phone}, Database Name: {firstEntry.Database_name}";
namespace Osint_WPF.Model
{
    public class Dehashed
    {
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
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var dehashedResponse = JsonSerializer.Deserialize<DehashedResponse>(content);
                    if (dehashedResponse?.Entries != null && dehashedResponse.Entries.Any())
                    {
                        return dehashedResponse.Entries;
                    }
                }
            }
            catch (HttpRequestException e)
            {
                throw;
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

            public override string ToString()
            {
                return $"Id: {Id}, Email: {Email}, Ip_address: {Ip_address}, Username: {Username},Password: {Password}, Hashed_password: {Hashed_password}, Hash_type: {Hash_type}, Name: {Name}, Vin: {Vin}, Address: {Address},Phone: {Phone}, Database_name: {Database_name}";
            }
        }
    }
}