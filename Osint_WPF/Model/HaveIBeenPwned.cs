using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Osint_WPF.Model
{
    public class HaveIBeenPwned
    {
        // API documentation: https://haveibeenpwned.com/API/v3

        private readonly HttpClient client = new HttpClient();
        private string apiKey;

        public HaveIBeenPwned(string myApiName, string apiKey)
        {
            // adds the api key via header
            client.DefaultRequestHeaders.Add("hibp-api-key", apiKey);
            // set a user agent for identification purposes
            client.DefaultRequestHeaders.UserAgent.ParseAdd(myApiName);
        }

        public async Task<(List<Breach>, List<Paste>)> CheckIfEmailHasBeenPwned(string email)
        {
            var breaches = await CheckBreaches(email);
            // Respect API delay requirements
            await Task.Delay(6000);
            var pastes = await CheckPastes(email);

            return (breaches, pastes);
        }

        public async Task<List<Breach>> CheckBreaches(string email)
        {

            // 'breachedaccount' checks if an account has been breached
            // "?truncateResponse=false"" is essential for parsing BreachData

            var url = $"https://haveibeenpwned.com/api/v3/breachedaccount/{Uri.EscapeDataString(email)}?truncateResponse=false";

            // sends GET request to API
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                List<Breach> breachDataList = new List<Breach>();

                var content = await response.Content.ReadAsStringAsync();
                var breaches = JsonSerializer.Deserialize<List<Breach>>(content);

                if (breaches != null)
                {
                    breachDataList.AddRange(breaches);
                    breachDataList = breaches.OrderByDescending(b => b.BreachDate).ToList();
                }
                return breachDataList ?? new List<Breach>();
            }           
            return new List<Breach>();
        }

        public async Task<List<Paste>> CheckPastes(string email)
        {
            var url = $"https://haveibeenpwned.com/api/v3/pasteaccount/{Uri.EscapeDataString(email)}";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                List<Paste> pasteDataList = new List<Paste>();

                var content = await response.Content.ReadAsStringAsync();
                var pastes = JsonSerializer.Deserialize<List<Paste>>(content);

                if (pastes != null)
                {
                    pasteDataList.AddRange(pastes);
                    pasteDataList = pastes.OrderByDescending(p => p.Date).ToList();
                }
                return pasteDataList ?? new List<Paste>();
            }
            return new List<Paste>();
        }

        public async Task<List<string>> CheckIfPasswordHasBeenPwned(string password)
        {
            // converts password to SHA-1 hash
            string sha1Password = GetSha1Hash(password);
            // takes first 5 characters to send to api
            string hashPrefix = sha1Password.Substring(0, 5);
            // api url
            string url = $"https://api.pwnedpasswords.com/range/{hashPrefix}";

            var response = await client.GetAsync(url);
            var pwnedHashes = new List<string>();

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                foreach (var line in content.Split('\n'))
                {
                    var parts = line.Split(':');
                    if (parts[0].Equals(sha1Password.Substring(5), StringComparison.OrdinalIgnoreCase))
                    {
                        pwnedHashes.Add(parts[0]);
                    }
                }
            }
            return pwnedHashes;
        }

        private static string GetSha1Hash(string input)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1Managed())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                var sb = new System.Text.StringBuilder();
                foreach (var obj in hashBytes)
                {
                    sb.Append(obj.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public class Breach
        {
            // properties which are going to be retrieved from breachdata in form of json with the same names
            public string Name { get; set; }
            public string BreachDate { get; set; }
            // helper property to convert BreachDate string to a DateTime object for easier sorting
            public DateTime BreachDateTime => DateTime.Parse(BreachDate);
        }
        public class Paste
        {
            public string Source { get; set; }
            public string Id { get; set; }
            public string Date { get; set; }
        }
    }
}
