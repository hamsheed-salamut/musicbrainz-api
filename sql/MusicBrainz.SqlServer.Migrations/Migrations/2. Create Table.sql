USE [MusicBrainz];

CREATE TABLE Artist 
(
    ArtistName varchar(300),
    ArtistId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Country	varchar(300),
    Aliases	nvarchar(300)COLLATE Japanese_Unicode_CI_AS
);