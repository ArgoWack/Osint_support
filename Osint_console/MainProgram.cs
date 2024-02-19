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
using System.Collections;
using static Osint_console.Dehashed;
using static Osint_console.HaveIBeenPwned;


class MainProgram
{
    public static void PrintExistingData(List<Dehashed.Entry> leakedEntries, string email, List<string> pwnedHashes, List<Breach> breaches,List<Paste> pastes)
    {
        WriteLine($"Leak information for {email}:");
        foreach (var entry in leakedEntries)
        {
            List<string> nonEmptyFields = new List<string>();

            if (!string.IsNullOrEmpty(entry.Id)) nonEmptyFields.Add($"ID: {entry.Id}");
            if (!string.IsNullOrEmpty(entry.Email)) nonEmptyFields.Add($"Email: {entry.Email}");
            if (!string.IsNullOrEmpty(entry.Username)) nonEmptyFields.Add($"Username: {entry.Username}");
            if (!string.IsNullOrEmpty(entry.Password)) nonEmptyFields.Add($"Password: {entry.Password}");
            if (!string.IsNullOrEmpty(entry.Hashed_password)) nonEmptyFields.Add($"Hashed_password: {entry.Hashed_password}");
            if (!string.IsNullOrEmpty(entry.Hash_type)) nonEmptyFields.Add($"Hash_type: {entry.Hash_type}");
            if (!string.IsNullOrEmpty(entry.Name)) nonEmptyFields.Add($"Name: {entry.Name}");
            if (!string.IsNullOrEmpty(entry.Address)) nonEmptyFields.Add($"Address: {entry.Address}");
            if (!string.IsNullOrEmpty(entry.Phone)) nonEmptyFields.Add($"Phone: {entry.Phone}");
            if (!string.IsNullOrEmpty(entry.Database_name)) nonEmptyFields.Add($"Database_name: {entry.Database_name}");

           // joins the non-empty fields with a separator and print
           WriteLine(string.Join(", ", nonEmptyFields));
        }
        WriteLine();

        if (pwnedHashes.Any())
        {
            WriteLine("Pwned Password Hashes:");
            foreach (var hash in pwnedHashes)
            {
                WriteLine(hash);
            }
        }
        else
        {
            WriteLine("No pwned passwords found.");
        }

        if (breaches.Any())
        {
            WriteLine("\n Breaches:");
            foreach (var breach in breaches)
            {
                WriteLine($"Name: {breach.Name}, Date: {breach.BreachDate}");
            }
        }
        else
        {
            WriteLine("\n No breaches found for this email.");
        }

        if (pastes.Any())
        {
            WriteLine("\n Pastes:");
            foreach (var paste in pastes)
            {
                WriteLine($"Source: {paste.Source}, Date: {paste.Date}, ID: {paste.Id}");
            }
        }
        else
        {
            WriteLine("\n No pastes found for this email.");
        }
    }
    static async Task Main(string[] args)
    {
        //Possible future user input data:
        string dummyEmail = "";
        string dummyId = "";
        string dummyIpAddress = "";
        string dummyUsername = "";
        string dummyPassword = "";
        string dummyHashedPassword = "";
        string dummyHashType = "";
        string dummyNameName = "";
        string dummyVin = "";
        string dummyAddress = "";
        string dummyPhone = "";
        string dummyDatabaseName = "";

        string dummyBreachDate = "";


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

        var aggregatedData = new AggregatedData();

        //Dehashed block:
        string dehashedApiUsername = Environment.GetEnvironmentVariable("dehashedApiUsername");
        string dehashedApiKey = Environment.GetEnvironmentVariable("dehashedApiKey");

        var leakedEntries = await Dehashed.CheckIfEmailHasBeenLeaked(email, dehashedApiUsername, dehashedApiKey, apiName);

        //haveibeenpwned block:
        var pwnedHashes = await HaveIBeenPwned.CheckIfPasswordHasBeenPwned(password);
        aggregatedData.PwnedPasswordHashes.AddRange(pwnedHashes);
        Thread.Sleep(6000);
        string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");
        var (breaches, pastes) = await HaveIBeenPwned.CheckIfEmailHasBeenPwned(email, hibp_ApiKey, apiName);

        //Printing results to console
        PrintExistingData(leakedEntries, email, pwnedHashes, breaches, pastes);
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
        public string BreachDate { get; set; }
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