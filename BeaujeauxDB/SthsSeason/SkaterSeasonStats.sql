CREATE TABLE [dbo].[SkaterSeasonStats]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SkaterId] INT NOT NULL, 
    [TeamId] INT NULL,
    [SeasonId] INT NOT NULL,
    [IsSubtotal] BIT NOT NULL,
    [GP] INT NOT NULL, 
	[G] INT NOT NULL,
	[A] INT NOT NULL,
	[P] INT NOT NULL,
	[PLMI] INT NOT NULL,
	[PIM] INT NOT NULL,
	[PM5] INT NOT NULL,
	[HIT] INT NOT NULL,
	[HTT] INT NOT NULL,
	[SHT] INT NOT NULL,
	[OSB] INT NOT NULL,
	[OSM] INT NOT NULL,
	[SB] INT NOT NULL,
	[MP] INT NOT NULL,
	[PPG] INT NOT NULL,
	[PPA] INT NOT NULL,
	[PPP] INT NOT NULL,
	[PPS] INT NOT NULL,
	[PPM] INT NOT NULL,
	[PKG] INT NOT NULL,
	[PKA] INT NOT NULL,
	[PKP] INT NOT NULL,
	[PKS] INT NOT NULL,
	[PKM] INT NOT NULL,
	[GW] INT NOT NULL,
	[GT] INT NOT NULL,
	[FOW] INT NOT NULL,
	[FOT] INT NOT NULL,
	[GA] INT NOT NULL,
	[TA] INT NOT NULL,
	[EG] INT NOT NULL,
	[HT] INT NOT NULL,
	[PSG] INT NOT NULL,
	[PSS] INT NOT NULL,
	[FW] INT NOT NULL,
	[FL] INT NOT NULL,
	[FT] INT NOT NULL,
	[GS] INT NOT NULL,
	[PS] INT NOT NULL,
	[WG] INT NOT NULL,
	[WP] INT NOT NULL,
	[S1] INT NOT NULL,
	[S2] INT NOT NULL,
	[S3] INT NOT NULL,
    CONSTRAINT [FK_SkaterSeasonStats_ToSkater] FOREIGN KEY ([SkaterId]) REFERENCES [Skater]([Id]), 
    CONSTRAINT [FK_SkaterSeasonStats_ToSeason] FOREIGN KEY ([SeasonId]) REFERENCES [Season]([Id]), 
    CONSTRAINT [FK_SkaterSeasonStats_ToTeam] FOREIGN KEY ([TeamId]) REFERENCES [Team]([Id]), 
)
