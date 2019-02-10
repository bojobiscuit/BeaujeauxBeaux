CREATE TABLE [dbo].[Seasons_Teams]
(
	[SeasonId] INT NOT NULL, 
    [TeamId] INT NOT NULL, 
    CONSTRAINT [FK_Seasons_Teams_ToSeason] FOREIGN KEY ([SeasonId]) REFERENCES [Season]([Id]),
    CONSTRAINT [FK_Seasons_Teams_ToTeam] FOREIGN KEY ([TeamId]) REFERENCES [Team]([Id]),
	PRIMARY KEY([SeasonId], [TeamId])
)
