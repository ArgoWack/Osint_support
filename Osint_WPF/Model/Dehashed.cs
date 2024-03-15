using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                        entries.AddRange(dehashedResponse.Entries);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                throw;

                // thinks to consider:
                // - logging the exception
                // Log.Error($"Error fetching data: {e.Message}");
                // rethrowing the exception to be handled by the caller
            }
            return entries;
        }
        public class DehashedResponse
        {
            public int Total { get; set; }
            public List<Entry> Entries { get; set; }
        }
        public class Entry
        {
            // Fields: id, email, ip_address, username, password, hashed_password, hash_type, name, vin, address, phone, database_name
            public string Id { get; set; }
            public string Email { get; set; }
            public string Ip_address { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Hashed_password { get; set; }
            public string Hash_type { get; set; }
            public string Name { get; set; }
            public string Vin { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Database_name { get; set; }
        }
    }
}