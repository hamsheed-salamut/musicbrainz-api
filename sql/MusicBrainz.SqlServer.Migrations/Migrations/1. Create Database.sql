IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MusicBrainz')
  BEGIN
    CREATE DATABASE [MusicBrainz]

END