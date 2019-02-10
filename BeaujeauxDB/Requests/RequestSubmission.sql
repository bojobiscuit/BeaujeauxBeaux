CREATE TABLE [dbo].[RequestSubmission]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RequestTypeId] INT NOT NULL,
    [DateSubmitted] DATETIME NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [DateComplete] DATETIME NULL, 
    [CompleteComment] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_RequestSubmission_ToRequestType] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestType]([Id]), 
)
