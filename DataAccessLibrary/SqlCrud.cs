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

            if (output.PersonInfo == null)
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


        public void CreateContact(ContactModel contact)
        {
            string sql = "insert into dbo.Person (FirstName, LastName) values (@FirstName, @LastName);";
            db.SaveData(sql, new { contact.PersonInfo.FirstName, contact.PersonInfo.LastName }, connectionString);

            sql = "select Id from dbo.Person where FirstName = @FirstName and LastName = @LastName;";
            int contactId = db.LoadData<IdLookupModel, dynamic>(sql,
                new { contact.PersonInfo.FirstName, contact.PersonInfo.LastName },
                connectionString).First().Id;


            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                // IDENTIFY IF NEW CONTACT PHONE NUMBER PRE-EXISTS
                // IF NUMBER DOES NOT EXIST INSERT IT; GET NEW PHONENUMBER ID
                if (phoneNumber.Id == 0)
                {
                    sql = "insert into dbo.PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
                    db.SaveData(sql, new { phoneNumber.PhoneNumber }, connectionString);

                    sql = "select Id from dbo.PhoneNumbers where PhoneNumber = @PhoneNumber;";
                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                       new { phoneNumber.PhoneNumber }, connectionString).First().Id;
                }

                // INSERT NEW CONTACT AND NUMBER ASSOCIATION INTO LINK TABLE
                sql = "insert into dbo.ContactsPhoneNumber (PersonId, PhoneNumberId) values (@ContactId, @PhoneNumberID);";
                db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, connectionString);
            }

            foreach (var email in contact.EmailAddresses)
            {
                if (email.Id == 0)
                {
                    sql = "insert into dbo.EmailAddresses (Email) values (@Email);";
                    db.SaveData(sql, new { email.Email }, connectionString);

                    sql = "select Id from dbo.EmailAddresses where Email = @Email;";
                    email.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { email.Email },
                        connectionString).First().Id;
                }

                sql = "insert into dbo.ContactsEmail (PersonId, EmailAddressesId) values (@PersonId, @EmailAddressId);";
                db.SaveData(sql, new { PersonId = contactId, EmailAddressId = email.Id }, connectionString);
            }
        }

        public void UpdatePersonName(BasicPersonModel person)
        {
            string sql = "update dbo.Person set FirstName = @FirstName, LastName = @LastName where Id = @Id;";
            db.SaveData(sql, person, connectionString);
        }

        public void RemovePhoneNumberFromContact(int personId, int PhoneNumberId)
        {
            string sql = "select Id, PersonId, PhoneNumberId from dbo.ContactsPhoneNumber where PhoneNumberId = @PhoneNumberId;";
            var links = db.LoadData<ContactsPhoneNumber, dynamic>(sql, new { PhoneNumberId }, connectionString);

            sql = "delete from dbo.ContactsPhoneNumber where PhoneNumberId = @PhoneNumberId and PersonId = @personId;";
            db.SaveData(sql, new { personId, PhoneNumberId }, connectionString);

            if (links.Count == 1)
            {
                sql = "delete from dbo.PhoneNumbers where Id = @PhoneNumberId;";
                db.SaveData(sql, new {PhoneNumberId}, connectionString);
            }
        }
    }
}
