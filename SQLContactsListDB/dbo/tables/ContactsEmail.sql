CREATE TABLE [dbo].[ContactsEmail]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[PersonId] INT NOT NULL,
	[EmailAddressesId] INT NOT NULL
)
