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
        
        public static string connectionString = @"Server=tcp:libraryadb-server.database.windows.net,1433;Initial Catalog=libraryaDB;Persist Security Info=False;User ID=admin-librarya;Password=hbuvoc%qAqB@32;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;".Trim();

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
