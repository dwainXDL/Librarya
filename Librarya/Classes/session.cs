using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Librarya.Classes
{
    public static class session
    {
        public static string user { get; set; }
        
        public static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=libraryaDB;Integrated Security=True;TrustServerCertificate=True";

        // API Keys
        public static Dictionary<string, string> loadSecrets(string path)
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        // Imgur Client
        public static string clientID;

        // Mail settings
        public static string smtpHost = "smtp.gmail.com";
        public static int smtpPort = 587;
        public static string emailSender;
        public static string apiKey;
        public static string apiSecret;
    }
}
