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


        public ContactModel GetContactById(int id)
        {
            string sql = "Select Id, FirstName, LastName from dbo.Person where Id = @id";

            ContactModel output = new ContactModel();

            output.PersonInfo = db.LoadData<BasicPersonModel, dynamic>(sql, new { Id = id }, connectionString).FirstOrDefault();
           
            if(output.PersonInfo == null)
            {
                return null;
            }
            


            sql = @"select e.*
                    from dbo.EmailAddresses e
                    inner join dbo.ContactsEmail ce on ce.EmailAddressesId = e.Id
                    where ce.PersonId = @Id";

            output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, connectionString);

            sql = @"select p.*
                    from dbo.PhoneNumbers p
                    inner join dbo.ContactsPhoneNumber cp on cp.PhoneNumberId = p.Id
                    where cp.PersonId = @Id";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, connectionString);


            return output;
        }
    }
}
