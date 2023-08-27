using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqlCrud
    {
        private readonly string connectionString;
        private SqlDataAccess db = new SqlDataAccess();


        public SqlCrud(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<BasicPersonModel> GetAllContacts()
        {
            string sql = "Select Id, FirstName, LastName from dbo.Person";
            return db.LoadData<BasicPersonModel, dynamic>(sql, new { }, connectionString);

            
            
        }
    }
}
