CREATE TABLE [dbo].[ContactsPhoneNumber]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [PersonId] INT NOT NULL, 
    [PhoneNumberId] INT NOT NULL
)
