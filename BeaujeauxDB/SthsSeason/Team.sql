CREATE TABLE [dbo].[Team]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[FranchiseId] INT NULL,
    [Name] NVARCHAR(50) NOT NULL, 
    [Acronym] NVARCHAR(50) NOT NULL, 
    [IconName] NVARCHAR(10) NULL, 
    CONSTRAINT [FK_Team_ToFranchise] FOREIGN KEY ([FranchiseId]) REFERENCES [Franchise]([Id]),
)
