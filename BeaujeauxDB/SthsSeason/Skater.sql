CREATE TABLE [dbo].[Skater]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
)

GO

CREATE INDEX [IX_Skater_Name] ON [dbo].[Skater] ([Name])
