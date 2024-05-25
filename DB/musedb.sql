/* check to see if the database exists, if so, drop it*/
IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases
			WHERE name = 'musedb')
BEGIN
	DROP DATABASE musedb
	print '' print '*** dropping database musedb ***'
END
GO

print '' print '*** creating database musedb ***'
GO
CREATE DATABASE [musedb]
GO

print '' print '*** using database musedb ***'
GO
USE [musedb]
GO

/* User Table */
print '' print '*** creating User Table ***'
GO
CREATE TABLE [dbo].[User] (
    [UserID]            [int] IDENTITY(100000,1)   NOT NULL,
    [ProfileName]       [nvarchar](100)            NOT NULL,
    [Email]             [nvarchar](100)            NOT NULL,
    [FirstName]         [nvarchar](50)             DEFAULT 'Unknown',
    [LastName]          [nvarchar](50)             DEFAULT 'Unknown',
    [PasswordHash]      [nvarchar](100)            NOT NULL DEFAULT '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8',
    [ImageFilePath]     [nvarchar](500)            NULL DEFAULT 'defaultAccount.png',
    [Photo]             [varbinary](MAX)           NULL 
	DEFAULT (CONVERT(varbinary(MAX), '0x89504E470D0A1A0A0000000D49484452000008C0000008C00803000000EC2E33170000000467414D410000B18F0BFC6105000000017352474200AECE1CE900000237504C544547704C0000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFF00000000000000000000000000000')),
    [PhotoMimeType]     [varchar](50)              NULL DEFAULT 'image/png',
    [Active]            [bit]                      NOT NULL DEFAULT 1,
    [MinutesListened]   [int]                      NULL DEFAULT 0,
    [isPublic]          [bit]                      DEFAULT 0,
    CONSTRAINT [pk_UserID] PRIMARY KEY ([UserID]),
    CONSTRAINT [ak_ProfileName] UNIQUE([ProfileName]),
    CONSTRAINT [ak_Email] UNIQUE([Email])
)
GO

/* UserFriend Table */
print '' print '*** creating UserFriend table ***'
GO
CREATE TABLE [dbo].[UserFriend] (
	[UserID]			[int]				       NOT NULL,
	[FriendID]			[int]                      NOT NULL,
	[DayAddedAsFriend]	[date]              	   NULL	
	
    CONSTRAINT [fk_UserFriend_UserID] FOREIGN KEY([UserID])
        REFERENCES [dbo].[User]([UserID]),
    
    CONSTRAINT [fk_UserFriend_FriendID] FOREIGN KEY([FriendID])
        REFERENCES [dbo].[User]([UserID]),
    
    CONSTRAINT [pk_UserFriend] PRIMARY KEY([UserID], [FriendID]),
    
    CONSTRAINT [u_UserFriend_Unique] UNIQUE([UserID], [FriendID])
)

/* Role Table */
print '' print '*** creating Role table ***'
GO
CREATE TABLE [dbo].[Role] (
	[RoleID]	[nvarchar](50)
	CONSTRAINT[pk_RoleID] PRIMARY KEY([RoleID])
)

/* UserRole Table */
print '' print '*** creating UserRole table ***'
GO
CREATE TABLE [dbo].[UserRole] (
	[UserID]	[int]						NOT NULL,
	[RoleID]	[nvarchar](50)				NOT NULL,
	CONSTRAINT [fk_UserRole_UserID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[User]([UserID]),
		
	CONSTRAINT [fk_UserRole_RoleID] FOREIGN KEY([RoleID])
		REFERENCES [dbo].[Role]([RoleID]),
		
	CONSTRAINT [pk_UserRole] PRIMARY KEY([UserID], [RoleID])
)

/* Artist Table */

print '' print '*** creating Artist table ***'
GO
CREATE TABLE [dbo].[Artist] (
	[ArtistID] 		  [nvarchar](200) 			 NOT NULL,
	[ImageFilePath]   [nvarchar](500)            NULL DEFAULT 
	'defaultAccount.png',
	[Photo]			  [varbinary](MAX)	         NULL,
	[PhotoMimeType]   [varchar](50)		         NULL,
	[FirstName] 	  [nvarchar](50)             NULL DEFAULT 'Unknown',
	[LastName] 		  [nvarchar](50)             NULL DEFAULT 'Unknown',
	[Description]     [nvarchar](max)			 NULL DEFAULT "",
	[isLiked]         [bit]			             NULL DEFAULT 0,
	[DateOfBirth] 	  [DateTime]				 NULL Default '2000-01-01',				 
	[UserID]		  [int],

	CONSTRAINT [fk_Artist_UserID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[User]([UserID]),
		
	CONSTRAINT[pk_ArtistID] PRIMARY KEY([ArtistID]))

/* Album Table */

print '' print '*** creating Album Table ***'
GO
CREATE TABLE [dbo].[Album] (
	[AlbumID] 		[int] IDENTITY(100000,1)     NOT NULL,
	[Title] 		[nvarchar](100)          	 NOT NULL DEFAULT 'Unknown',
	[ArtistID] 		[nvarchar](200)          	 NOT NULL DEFAULT 'Unknown',
	[IsExplicit]    [bit]                        DEFAULT 0, 
	[ImageFilePath] [nvarchar](500)              DEFAULT 
	'defaultAlbumImage.png',
	[Photo]			  [varbinary](MAX)	         NULL,
	[PhotoMimeType]   [varchar](50)		         NULL,
	[Description] 	  [nvarchar](max)			 NULL, 
	[YearReleased] 	  [int]				 		 DEFAULT 2002, 
	[DateAdded] 	  [DateTime],
	[UserID] 		  [int]						 NULL
	
	CONSTRAINT [fk_Album_ArtistID] FOREIGN KEY([ArtistID])
		REFERENCES [dbo].[Artist]([ArtistID]),

	CONSTRAINT [fk_Album_UserID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[User]([UserID]),

	CONSTRAINT[pk_AlbumID] PRIMARY KEY([AlbumID])
)
GO

/* Song Table */

print '' print '*** creating Song Table ***'
GO
CREATE TABLE [dbo].[Song] (
    [SongID]         [int] IDENTITY(100000,1)    NOT NULL,
    [Title]          [nvarchar](180)          	 NOT NULL DEFAULT 'Unknown',
    [ImageFilePath]  [nvarchar](500)             DEFAULT '\bin\Debug\MuseConfig\art\SongDefault.jpg',
	[Photo]			 [varbinary](MAX)	         NULL,
	[PhotoMimeType]  [varchar](50)		         NULL,
	[Mp3FilePath]    [nvarchar](500)			 NOT NULL,
    [YearReleased]   [int]						 DEFAULT 2024,
    [Lyrics]         [nvarchar](max)		     DEFAULT 'No Lyrics Provided',
    [Explicit]       [bit]						 NOT NULL DEFAULT 0,
    [Genre]          [nvarchar](200)			 NULL DEFAULT "None",
    [Plays]          [int]						 NOT NULL DEFAULT 0,
	[UserID]		 [int],
	[ArtistID]		 [nvarchar](200),
	[AlbumID]		 [int]						 NULL,
    [DateUploaded]   [DATETIME]                  NULL,
	[DateAdded]      [DATETIME]                  NOT NULL DEFAULT GETDATE(),
	[isLiked]        [bit]			             DEFAULT 0,
	[isPublic]       [bit]			             DEFAULT 0,
	CONSTRAINT [fk_Song_UserID] FOREIGN KEY([UserID])
        REFERENCES [dbo].[User]([UserID]), 
	CONSTRAINT [fk_Song_AlbumID] FOREIGN KEY([AlbumID])
        REFERENCES [dbo].[Album]([AlbumID]),
	CONSTRAINT [fk_Song_ArtistID] FOREIGN KEY([ArtistID])
        REFERENCES [dbo].[Artist]([ArtistID]), 	
    CONSTRAINT [pk_SongID] PRIMARY KEY([SongID])		
)
GO

/* SongAlbum Table */

print '' print '*** creating SongAlbum table ***'
GO
CREATE TABLE [dbo].[SongAlbum] (
	[SongID]      	[int]						 NOT NULL,
	[AlbumID]		[int]						 NOT NULL,
	[TrackNumber]   [int]						 NULL,
	CONSTRAINT [fk_SongAlbum_SongID] FOREIGN KEY([SongID])
		REFERENCES [dbo].[Song]([SongID]) ON DELETE CASCADE,
		
	CONSTRAINT [pk_SongAlbum] PRIMARY KEY([SongID], [AlbumID])
)
GO

/* SongArtist Table */

print '' print '*** creating SongArtist table ***'
GO
CREATE TABLE [dbo].[SongArtist] 
(
	[SongID]        [int]					     NOT NULL,
	[ArtistID]      [nvarchar](200) 			 NOT NULL,
	[isFeaturing]	[bit]  					     DEFAULT 0,
	CONSTRAINT [fk_SongArtist_SongID] FOREIGN KEY([SongID])
		REFERENCES [dbo].[Song]([SongID]) ON DELETE CASCADE,
		
	CONSTRAINT [pk_SongArtist] PRIMARY KEY([SongID], [ArtistID])
)
GO

/* Playlist Table */

print '' print '*** creating Playlist table ***'
GO
CREATE TABLE [dbo].[Playlist] (
	[PlaylistID]  [int] IDENTITY(100000,1)     NOT NULL,
	[Title]	[nvarchar](50)					   NOT NULL DEFAULT 'Playlist',
	[ImageFilePath] [nvarchar](500)            DEFAULT 'defaultAlbumImage.png',
    [Photo]             [varbinary](MAX)       NULL,
	[PhotoMimeType]     [varchar](50)          NULL,
	[Description] [nvarchar](max)			   NULL DEFAULT "",
	[UserID] [int]
	
	CONSTRAINT [fk_Playlist_UserID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[User]([UserID]),
	
	CONSTRAINT [pk_Playlist] PRIMARY KEY([PlaylistID])
)
GO

/* PlaylistSong Table */

print '' print '*** creating PlaylistSong table ***'
GO
CREATE TABLE [dbo].[PlaylistSong] 
(
	[PlaylistID]      [int]						 NOT NULL,
	[SongID]  		  [int]			 		     NOT NULL,
	[TimeAdded]  	  [DateTime]			     NOT NULL
	
	CONSTRAINT [fk_PlaylistSong_SongID] FOREIGN KEY([SongID])
		REFERENCES [dbo].[Song]([SongID]) ON DELETE CASCADE,
		
	CONSTRAINT [fk_PlaylistSong_PlaylistID] FOREIGN KEY([PlaylistID])
		REFERENCES [dbo].[Playlist]([PlaylistID]) ON DELETE CASCADE,
		
	CONSTRAINT [pk_PlaylistID] PRIMARY KEY([PlaylistID], [SongID])
)
GO

/* Review Table */

print '' print '*** creating Review table ***'
GO
CREATE TABLE [dbo].[Review] (
	[ReviewID][int] IDENTITY(100000,1)     NOT NULL,
	[Rating]  [int]					       NULL,	
	[Message] [nvarchar](max)              NULL DEFAULT "",
	[UserID]  [int]						   NOT NULL,
	[SongID]  [int]						   NOT NULL
	CONSTRAINT [fk_Review_UserID] FOREIGN KEY([UserID])
		REFERENCES [dbo].[User]([UserID]),
	
	CONSTRAINT [fk_Review_SongID] FOREIGN KEY([SongID])
		REFERENCES [dbo].[Song]([SongID]) ON DELETE CASCADE,
		
	CONSTRAINT[pk_Review] PRIMARY KEY([ReviewID])
)
GO