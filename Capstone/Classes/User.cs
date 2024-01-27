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
        public int Uid { get; set; }
        public string UName {  get; set; }
        public string Password { get; set; }
        
        public User()
        {
            this.Uid = 0;
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
