using static System.Console;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data.Common;
using System.Text.Json.Serialization;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dotenv.net;

namespace Osint_console
{
    public class HaveIBeenPwned
    {

        public static async Task<(List<Breach>, List<Paste>)> CheckIfEmailHasBeenPwned(string email, string hibp_ApiKey, string myApiName)
        {
            // API documentation: https://haveibeenpwned.com/API/v3

            var breaches = await CheckBreaches(email, myApiName, hibp_ApiKey);
            // Respect API delay requirements
            await Task.Delay(6000);
            var pastes = await CheckPastes(email, myApiName, hibp_ApiKey);

            return (breaches, pastes);
        }

        public static async Task<List<Breach>> CheckBreaches(string email, string myApiName, string apiKey)
        {
            List<Breach> breachDataList = new List<Breach>();

            //specyfying kinds of data for API request
            // 'breachedaccount' checks if an account has been breached
            string service = "breachedaccount";
            // ensures the email is url-safe.
            string parameter = Uri.EscapeDataString(email);
            // "?truncateResponse=false"" is essential for parsing BreachData
            string url = $"https://haveibeenpwned.com/api/v3/{service}/{parameter}?truncateResponse=false";

            using (var client = new HttpClient())
            {
                // adds the api key via header
                client.DefaultRequestHeaders.Add("hibp-api-key", apiKey);
                // set a user agent for identification purposes
                client.DefaultRequestHeaders.UserAgent.ParseAdd(myApiName + "/Version");

                try
                {
                    //sends GET request to api
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var breaches = JsonSerializer.Deserialize<Breach[]>(content);
                        if (breaches != null)
                        {
                            breachDataList.AddRange(breaches);
                            breachDataList = breaches.OrderByDescending(b => b.BreachDate).ToList();
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        //if email not found in any breach
                        WriteLine($"{email} has not been pwned.");
                    }
                    else
                    {
                        //in case of connection or other issues
                        WriteLine($"Failed to retrieve data: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    //handling code errors
                    WriteLine($"Error fetching data: {e.Message}");
                }
            }
            return breachDataList;
        }

        public static async Task<List<Paste>> CheckPastes(string email, string myApiName, string apiKey)
        {
            List<Paste> pasteDataList = new List<Paste>();

            string service = "pasteaccount";
            string parameter = Uri.EscapeDataString(email);
            string url = $"https://haveibeenpwned.com/api/v3/{service}/{parameter}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("hibp-api-key", apiKey);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(myApiName + "/Version");

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var pastes = JsonSerializer.Deserialize<Paste[]>(content);
                        if (pastes != null)
                        {
                            pasteDataList.AddRange(pastes);
                            pasteDataList = pastes.OrderByDescending(p => p.Date).ToList();
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        WriteLine($"{email} has not been pwned in pastes.");
                    }
                    else
                    {
                        WriteLine($"Failed to retrieve paste data: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    WriteLine($"Error fetching paste data: {e.Message}");
                }
            }
            return pasteDataList;
        }
        public static async Task<List<string>> CheckIfPasswordHasBeenPwned(string password)
        {
            List<string> pwnedHashes = new List<string>();

            // converts password to SHA-1 hash
            string sha1Password = GetSha1Hash(password);

            // takes first 5 characters to send to api
            string hashPrefix = sha1Password.Substring(0, 5);

            // api url
            string url = $"https://api.pwnedpasswords.com/range/{hashPrefix}";

            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        // splits each line in response
                        foreach (var line in content.Split('\n'))
                        {
                            var parts = line.Split(':');
                            if (parts.Length == 2)
                            {
                                var hashSuffix = parts[0].Trim();

                                if (hashSuffix.Equals(sha1Password.Substring(5), StringComparison.OrdinalIgnoreCase))
                                {
                                    pwnedHashes.Add(hashPrefix + hashSuffix);
                                }
                            }
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

            return pwnedHashes;
        }

        public static string GetSha1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // "X2" for uppercase letters, "x2" for lowercase
                    sb.Append(b.ToString("X2"));
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

