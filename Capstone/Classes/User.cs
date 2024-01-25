using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

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
            this.Password = pass;
        }
        public User(string name , string pass)
        {
            this.UName = name;
            this.Password = pass;
        }
    }
}
