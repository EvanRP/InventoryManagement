using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Security.Cryptography;

namespace Capstone.Classes

{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int Uid { get; private set; }
        private string UName;
        private string Password;
       
        public string getUName()
        {
            return this.UName;
        }
        public string getPassword()
        {
            return this.Password;
        }
        
        public User()
        {
            this.UName = "";
            this.Password = "";
        }
        public User(int id , string name , string pass)
        {
            this.Uid = id;
            this.UName = name;
            this.Password = HashPass(pass);
        }
        public User(string name , string pass)
        {
            this.UName = name;
            this.Password = HashPass(pass);
        }

        public static string HashPass(string pass)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));

                StringBuilder sb = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
