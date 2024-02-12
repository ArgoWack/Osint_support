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

class MainProgram
{
    static async Task Main(string[] args)
    {
        // installed NuGet packed to be able to load sensitive data from .env file
        DotEnv.Load();

        string hibp_ApiKey = Environment.GetEnvironmentVariable("hibp_ApiKey");

        //Example of password to test:
        //qwerty1
        //asdfzxc

        string password = "qwerty1";
        await HaveIBeenPwned.CheckIfPasswordHasBeenPwned(password);
        Thread.Sleep(6000);

        //Example of emails to test:
        //qwerty1@xyz.com
        //xyz@xyz.xyz

        //email to be searched via API
        string email = "xyz@xyz.xyz";
        await HaveIBeenPwned.CheckIfEmailHasBeenPwned(email, hibp_ApiKey);
    }
}