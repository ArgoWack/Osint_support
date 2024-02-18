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
using Osint_console;

using System.Diagnostics;

class MainProgram
{
    public static void ExecuteCurlCommand(string username, string apikey, string userAgent)
    { 
        //method used only for tests when new API calls doesn't work
        var escapedArgs = $"-u \"{username}:{apikey}\" \"https://api.dehashed.com/search?query=example%40example.com\" -H \"User-Agent: {userAgent}/Version\" -H \"Accept: application/json\"";

        var startInfo = new ProcessStartInfo
        {
            FileName = "curl",
            Arguments = escapedArgs,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
            if (process != null)
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    WriteLine(result);
                }
            }
            else
            {
                WriteLine("Failed to start curl process.");
            }

        }
        //ExecuteCurlCommand(dehashedApiUsername, dehashedApiKey, apiName); (for execution in main)
    }

    static async Task Main(string[] args)
    {

        var dehashed = new Dehashed();
        var haveIBeenPwned = new HaveIBeenPwned();

        var aggregatedData = new AggregatedData();

        /*
        //Extracts data from methods below into main program

        aggregatedData.LeakDetails.AddRange(dehashed.CheckIfEmailHasBeenLeaked());

        aggregatedData.PwnedPasswordHashes.AddRange(haveIBeenPwned.CheckIfPasswordHasBeenPwned());

        aggregatedData.EmailBreaches.AddRange(haveIBeenPwned.CheckIfEmailHasBeenPwnedBreaches());
        aggregatedData.EmailPastes.AddRange(haveIBeenPwned.CheckIfEmailHasBeenPwnedPastes());

        */

        //Example of password to test:
        //qwerty1
        //asdfzxc
        string password = "qwerty1";

        //Example of emails to test:
        //qwerty1@xyz.com
        //xyz@xyz.xyz

        //email to be searched via API
        string email = "xyz@xyz.xyz";

        // installed NuGet packed to be able to load sensitive data from .env file
        DotEnv.Load();

        //name of my api for identification purposes
        string apiName = Environment.GetEnvironmentVariable("myApiName");

        //Dehashed block:

        string dehashedApiUsername = Environment.GetEnvironmentVariable("dehashedApiUsername");
        string dehashedApiKey = Environment.GetEnvironmentVariable("dehashedApiKey");

        //await Dehashed.CheckIfEmailHasBeenLeaked(email, dehashedApiUsername, dehashedApiKey, apiName);


        //haveibeenpwned block:

        
        string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");

        await HaveIBeenPwned.CheckIfPasswordHasBeenPwned(password);
        Thread.Sleep(6000);

        await HaveIBeenPwned.CheckIfEmailHasBeenPwned(email, hibp_ApiKey, apiName);
        
    }

    public class LeakDataDehashed
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HashedPassword { get; set; }
        public string HashType { get; set; }
        public string Name { get; set; }
        public string Vin { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string DatabaseName { get; set; }
    }

    public class BreachDataHaveibeenpwned
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }

    public class PasteDataHaveibeenpwned
    {
        public string Source { get; set; }
        public string Date { get; set; }
        public string Id { get; set; }
    }

    public class AggregatedData
    {
        public List<LeakDataDehashed> LeakDetails { get; set; } = new List<LeakDataDehashed>();
        public List<string> PwnedPasswordHashes { get; set; } = new List<string>();
        public List<BreachDataHaveibeenpwned> EmailBreaches { get; set; } = new List<BreachDataHaveibeenpwned>();
        public List<PasteDataHaveibeenpwned> EmailPastes { get; set; } = new List<PasteDataHaveibeenpwned>();
    }
}