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
    private static bool IsValidField(string field) =>!string.IsNullOrEmpty(field) && !string.Equals(field, "NULL", StringComparison.OrdinalIgnoreCase);
    public static void PrintExistingDehashedData(List<Dehashed.Entry> leakedEntries, string dataKind)
    {
        WriteLine($"Leak information for: {dataKind}");
        foreach (var entry in leakedEntries)
        {
            List<string> nonEmptyFields = new List<string>();

            //
            if (IsValidField(entry.Id)) nonEmptyFields.Add($"ID: {entry.Id}");
            if (IsValidField(entry.Email)) nonEmptyFields.Add($"Email: {entry.Email}");
            if (IsValidField(entry.Ip_address)) nonEmptyFields.Add($"Ip_address: {entry.Ip_address}");
            if (IsValidField(entry.Username)) nonEmptyFields.Add($"Username: {entry.Username}");
            if (IsValidField(entry.Password)) nonEmptyFields.Add($"Password: {entry.Password}");
            if (IsValidField(entry.Hashed_password)) nonEmptyFields.Add($"Hashed_password: {entry.Hashed_password}");
            if (IsValidField(entry.Hash_type)) nonEmptyFields.Add($"Hash_type: {entry.Hash_type}");
            if (IsValidField(entry.Name)) nonEmptyFields.Add($"Name: {entry.Name}");
            if (IsValidField(entry.Vin)) nonEmptyFields.Add($"Vin: {entry.Vin}");
            if (IsValidField(entry.Address)) nonEmptyFields.Add($"Address: {entry.Address}");
            if (IsValidField(entry.Phone)) nonEmptyFields.Add($"Phone: {entry.Phone}");
            if (IsValidField(entry.Database_name)) nonEmptyFields.Add($"Database_name: {entry.Database_name}");

            // joins the non-empty fields with a separator and print
            WriteLine(string.Join(", ", nonEmptyFields));
        }
    }
    public static void PrintExistingPwnedHashes(List<string> pwnedHashes)
    {
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
    }
    public static void PrintExistingBreachesPastes(string email, List<Breach> breaches, List<Paste> pastes)
    {
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
        string dummyEmail = "xyz@xyz.xyz"; // Example of emails to test: qwerty1@xyz.com, xyz@xyz.xyz
        string dummyId = "";
        string dummyIpAddress = "";
        string dummyUsername = "";
        string dummyPassword = "qwerty1";  //Example of password to test: qwerty1, asdfzxc
        string dummyHashedPassword = "";
        string dummyHashType = "";
        string dummyName = "";
        string dummyVin = "";
        string dummyAddress = "";
        string dummyPhone = "";
        string dummyDatabaseName = "";

        string dummyBreachDate = "";

        // installed NuGet packed to be able to load sensitive data from .env file
        DotEnv.Load();

        //name of my api for identification purposes
        string apiName = Environment.GetEnvironmentVariable("myApiName");

        string dehashedApiUsername = Environment.GetEnvironmentVariable("dehashedApiUsername");
        string dehashedApiKey = Environment.GetEnvironmentVariable("dehashedApiKey");

        // Possible searches according to data types:

        //dummyEmail
        //string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");
        //string leak_search = "email:\"" + dummyEmail + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //var (breaches, pastes) = await HaveIBeenPwned.CheckIfEmailHasBeenPwned(dummyEmail, hibp_ApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyEmail);
        //PrintExistingBreachesPastes(dummyEmail, breaches, pastes);

        //dummyId
        //string leak_search = "id:\"" + dummyId + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyId);

        //dummyIpAddress
        //string leak_search = "ip_address:\"" + dummyIpAddress + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyIpAddress);

        //dummyUsername
        //string leak_search = "username:\"" + dummyUsername + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyUsername);

        //dummyPassword
        //string leak_search = "password:\"" + dummyPassword + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        var pwnedHashes = await HaveIBeenPwned.CheckIfPasswordHasBeenPwned(dummyPassword);
        //Thread.Sleep(6000);
        //PrintExistingDehashedData(leakedEntries, dummyPassword);
        PrintExistingPwnedHashes(pwnedHashes);

        //dummyHashedPassword
        //string leak_search = "hashed_password:\"" + dummyHashedPassword + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyHashedPassword);

        //dummyHashType
        //string leak_search = "hash_type:\"" + dummyHashType + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyHashType);

        //dummyName
        //string leak_search = "name:\"" + dummyName + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyName);

        //dummyVin
        //string leak_search = "vin:\"" + dummyVin + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyVin);

        //dummyAddress
        //string leak_search = "address:\"" + dummyAddress + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyAddress);

        //dummyPhone
        //string leak_search = "phone:\"" + dummyPhone + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyPhone);

        //dummyDatabaseName
        //string leak_search = "database_name:\"" + dummyDatabaseName + "\"";
        //var leakedEntries = await Dehashed.CheckIfDataHasBeenLeaked(leak_search, dehashedApiUsername, dehashedApiKey, apiName);
        //PrintExistingDehashedData(leakedEntries, dummyDatabaseName);

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
}