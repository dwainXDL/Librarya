using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Librarya.Classes
{
    public static class session
    {
        public static string user { get; set; }
        public static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=libraryaDB;Integrated Security=True;TrustServerCertificate=True";
        public static string loadSecrets()
        {
            return File.ReadAllText(@"D:\Work\Uni\NSBM\C# Assignment\Librarya\Librarya\Librarya\Secrets\imgur.secret".Trim());
        }
    }
}
