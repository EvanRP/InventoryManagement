using SQLite;

namespace Capstone.Classes

{
    class Sql
    {
        readonly SQLiteConnection db;
        private static string dbPath = "..\\..\\..\\Data.db";

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
            if(tableExists(tableName))
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
                        User usr = new User("test","test");
                        db.Insert(usr);
                        break;
                }
            }
        }
    }
}
