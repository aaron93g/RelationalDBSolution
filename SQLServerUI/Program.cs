
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

// Tests we are able to reach the Data base
//Console.WriteLine(GetConnectionString());


SqlCrud sql = new SqlCrud(GetConnectionString());

//ReadAllContacts(sql);
//ReadContact(sql, 1);
CreateNewContact(sql);





static void CreateNewContact(SqlCrud sql)
{
    ContactModel model = new ContactModel
    {
        PersonInfo = new BasicPersonModel { FirstName = "Alexander", LastName = "Garcia" }
        //EmailAddresses = new List<EmailAddressModel> { new EmailAddressModel { Email = "Andrew@outlook.com"},
        //                                               new EmailAddressModel{Email = "AndrewIsAwesome@Gmail.com"}},
        //PhoneNumbers = new List<PhoneNumberModel> { new PhoneNumberModel { PhoneNumber = "559-5559"},
        //                                            new PhoneNumberModel {PhoneNumber = "915-5559"}}
    };

    model.EmailAddresses.Add(new EmailAddressModel {Id=1, Email = "aaron@yahoo.com" });
    model.EmailAddresses.Add(new EmailAddressModel { Email = "AdrianIsAwesome@outlook.com" });
    model.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "559-1111" });
    model.PhoneNumbers.Add(new PhoneNumberModel {Id=1, PhoneNumber = "555-1235" });

    sql.CreateContact(model);
}

static void ReadAllContacts(SqlCrud sql)
{
    var rows = sql.GetAllContacts();

    foreach (var row in rows)
    {
        Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
    }
}

static void ReadContact(SqlCrud sql, int contactId)
{
    var contact = sql.GetContactById(contactId);

    
        Console.WriteLine($"{contact.PersonInfo.Id}: {contact.PersonInfo.FirstName} {contact.PersonInfo.LastName}");
    
}

static string GetConnectionString(string connectionStringName = "Defualt")
{
    string output = "";

    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("AppSettings.json");

    var config = builder.Build();

    output = config.GetConnectionString(connectionStringName);

    return output;
}