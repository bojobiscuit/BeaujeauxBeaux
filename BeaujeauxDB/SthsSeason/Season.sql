CREATE TABLE [dbo].[Season]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Number] INT NOT NULL, 
    [SeasonTypeId] INT NOT NULL, 
    [LeagueId] INT NOT NULL, 
    CONSTRAINT [FK_Season_ToSeasonType] FOREIGN KEY ([SeasonTypeId]) REFERENCES [SeasonType]([Id]),
    CONSTRAINT [FK_Season_ToLeague] FOREIGN KEY ([LeagueId]) REFERENCES [League]([Id])
)
