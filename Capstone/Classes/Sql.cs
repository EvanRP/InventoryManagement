using SQLite;
//using System.Data.SQLite;

namespace Capstone.Classes

{
    class Sql
    {
        readonly SQLiteConnection db;
        private static string dbPath = "..\\..\\..\\Data.db";

        //public SQLiteConnection getDB() { return db; }
        public Sql()
        {
            db = new SQLiteConnection(dbPath);
            createTable("Inhouse");
            createTable("Outsourced");
            createTable("Product");
            createTable("User");
        }

        private bool tableExists(string tableName)
        {
            var table = db.GetTableInfo(tableName);
            return table.Count > 0; 
        }

        public void createTable(string tableName)
        {
            if(!tableExists(tableName))
            {
                switch (tableName)
                {
                    case "Inhouse":
                        db.CreateTable<Inhouse>();
                        break;

                    case "Outsources":
                        db.CreateTable<Outsourced>();
                        break;

                    case "Product":
                        db.CreateTable<Product>();
                        break;

                    case "User":
                        db.CreateTable<User>();
                        User usr = new("test","test");
                        db.Insert(usr);
                        break;
                }
            }
        }

        public User getUser(string uName)
        {
            User usr = new();
            usr = db.Table<User>().FirstOrDefault(u => u.UName == uName);
            return usr;
        }
    }
}
