CREATE TABLE [dbo].[Franchise]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LatestTeamId] INT NOT NULL, 
    [LeagueId] INT NOT NULL, 
    CONSTRAINT [FK_Franchise_ToTeam] FOREIGN KEY ([LatestTeamId]) REFERENCES [Team]([Id]),
    CONSTRAINT [FK_Franchise_ToLeague] FOREIGN KEY ([LeagueId]) REFERENCES [League]([Id])
)
