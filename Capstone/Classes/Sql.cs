using SQLite;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Classes

{
    class Sql
    {
        private readonly SQLiteConnection db;
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
        public SQLiteConnection GetConn()
        {
            return this.db;
        }
        private bool tableExists(string tableName)
        {   // returns true if table exists
            var table = db.GetTableInfo(tableName);
            return table.Count > 0;
        }

        public void createTable(string tableName)
        {
            // if table does not exist create table
            if (!tableExists(tableName))
            {
                switch (tableName)
                {
                    case "Inhouse":
                        db.CreateTable<Inhouse>();
                        break;

                    case "Outsourced":
                        db.CreateTable<Outsourced>();
                        break;

                    case "Product":
                        db.CreateTable<Product>();
                        break;

                    case "User":
                        db.CreateTable<User>();
                        User usr = new("test", "test");
                        db.Insert(usr);
                        break;
                }
            }
        }

        public User getUser(string uName)
        {
            User usr = new();
            // return user where users username == provided username
            usr = db.Table<User>().FirstOrDefault(u => u.UName == uName);
            return usr;
        }
        public BindingList<Part> GetParts()
        {
            // get list of Inhouse parts and Outsourced parts
            List<Inhouse> inList = db.Table<Inhouse>().ToList();
            List<Outsourced> outList = db.Table<Outsourced>().ToList();

            // upcast list to type Part
            List<Part> p1 = inList.Cast<Part>().ToList();
            List<Part> p2 = outList.Cast<Part>().ToList();

            // concatenate lists and create BindingList<Part>
            BindingList<Part> partsList = new(p1.Concat(p2).ToList());
            return partsList;
        }
        public Part GetPart(int pId)
        {
            BindingList<Part> parts = GetParts();
            return parts.FirstOrDefault(p => p.partID == pId);
        }
        public bool IsInhouse(int pId)
        {
            Inhouse P = db.Table<Inhouse>().FirstOrDefault(p => p.partID == pId);
            if (P == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public BindingList<Product> GetProducts()
        {

            BindingList<Product> blPro = new(db.Table<Product>().ToList());
            return blPro;
        }
        public void MovePart(int pId, Inhouse i)
        {
            db.Delete(GetPart(pId));
            db.Insert(i);
        }
        public void MovePart(int pId, Outsourced o)
        {
            db.Delete(GetPart(pId));
            db.Insert(o);
        }
        public void UpdatePart(Inhouse i)
        {
            db.Update(i);
        }
        public void UpdatePart(Outsourced o)
        {
            db.Update(o);

        }
        public void UpdatePro(Product pro)
        {
            db.Update(pro);
        }
        public int GetNextId(string table)
        {
            if(table == "product")
            {
                Product lastPro = db.Table<Product>().OrderByDescending(p => p.productID).FirstOrDefault();
                if (lastPro != null)
                {
                    return lastPro.productID + 1;
                }
                return 1;
            }
            else
            {
                Inhouse n = db.Table<Inhouse>().OrderByDescending(n => n.partID).FirstOrDefault();
                Outsourced o = db.Table<Outsourced>().OrderByDescending(n => n.partID).FirstOrDefault();
                if (n == null &&  o == null)
                {
                    return 1;
                }
                else if (n == null && o != null)
                {
                    return o.partID + 1;
                }
                else if (o == null && n != null)
                {
                    return n.partID + 1;
                }
                else if(n.partID > o.partID)
                {
                    return n.partID + 1;
                }
                else
                {
                    return o.partID + 1;
                }
            }
            
        }
        public int DeleteObject(Inhouse n)
        {
            return db.Delete(n);
        }
        public int DeleteObject(Outsourced o)
        {
            return db.Delete(o);
        }
        public int DeleteObject(Product pro)
        {
            return db.Delete(pro);
        }
        public void AddToDB(Inhouse n)
        {
            db.Insert(n);
        }
        public void AddToDB(Outsourced o)
        {
            db.Insert(o);
        }
        public void AddToDB(Product pro)
        {
            db.Insert(pro);
        }
    }
}
