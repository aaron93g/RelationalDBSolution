
using DataAccessLibrary;
using Microsoft.Extensions.Configuration;

// Tests we are able to reach the Data base
//Console.WriteLine(GetConnectionString());


SqlCrud sql = new SqlCrud(GetConnectionString());

//ReadAllContacts(sql);
ReadContact(sql, 1);


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