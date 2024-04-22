print '' print '*** using database musedb ***'
GO
USE [musedb]
GO


/* User Stored Procedures */

/* sp_create_new_user*/

print '' print '*** creating sp_create_new_user ***'
GO
CREATE PROCEDURE [dbo].[sp_create_new_user]
(
	@PasswordHash  [nvarchar](100),
	@Email 		   [nvarchar](100),
	@ProfileName   [nvarchar](200),
	@ImageFilePath [nvarchar](500),
	@Photo 	       [varbinary](MAX),
	@PhotoMimeType [nvarchar](50)
)
AS
	BEGIN
		INSERT INTO [dbo].[User]
				([ProfileName], [Email], [PasswordHash], [ImageFilePath], [Photo], [PhotoMimeType])
			VALUES
				(@ProfileName, @Email, @PasswordHash, @ImageFilePath, @Photo, @PhotoMimeType)
	END
GO

/* sp_authenticate_user */

print '' print '*** creating sp_authenticate_user ***'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
(
	@Email			[nvarchar](100),
	@PasswordHash   [nvarchar](100)
)
AS	
	BEGIN
		SELECT COUNT([UserID]) as 'Authenticated'
		FROM	[User]
		WHERE	@Email = [Email]
			AND	@PasswordHash = [PasswordHash]
			AND	[Active] = 1
	END
GO

/* sp_select_user_by_email */
 
print '' print '*** creating sp_select_user_by_email ***'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_Email]
(
	@Email			[nvarchar](100)
)
AS	
 	BEGIN
		SELECT [UserID], 
			   [ProfileName], 
			   [Email], 
			   [FirstName], 
			   [LastName], 
			   [ImageFilePath], 
			   [Photo], 
			   [PhotoMimeType], 
			   [Active], 
			   [MinutesListened],
			   [isPublic]
		FROM   [User]
		WHERE  @Email = [Email]
	END
GO

/* sp_select_passwordHash_by_Email */

print '' print '*** creating sp_select_passwordHash_by_Email ***'
GO
CREATE PROCEDURE [dbo].[sp_select_passwordHash_by_Email]
(
	@Email			[nvarchar](100)
)
AS	
	BEGIN
		SELECT	[PasswordHash]
		FROM	[User]
		WHERE 	@Email = [Email]
	END
GO

/* sp_select_all_users */
 
print '' print '*** creating sp_select_all_users ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_users]
AS	
 	BEGIN
		SELECT [UserID], 
			   [ProfileName], 
			   [Email], 
			   [FirstName], 
			   [LastName], 
			   [ImageFilePath], 
			   [Photo], 
			   [PhotoMimeType], 
			   [Active], 
			   [MinutesListened],
			   [isPublic]
		FROM   [User]
	END
GO

/* Roles */

/* sp_select_role_by_UserID */

print '' print '*** creating sp_select_role_by_UserID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_role_by_UserID]
(
	@UserID			[int]
)
AS	
	BEGIN
		SELECT	[RoleID]
		FROM	[UserRole]
		WHERE 	@UserID = [UserID]
	END
GO

/* sp_select_all_roles */

print '' print '*** creating sp_select_all_roles ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_roles]
AS	
	BEGIN
		SELECT	[RoleID]
		FROM	[Role]
	END
GO

/* sp_add_role */

print '' print '*** creating sp_add_role'
GO
CREATE PROCEDURE [sp_insert_employee_role]
(
	@UserID	     [int],
	@RoleID		 [nvarchar](50)
)
AS
	BEGIN
		INSERT INTO [dbo].[UserRole]
			([UserID], [RoleID])
		VALUES
			(@UserID, @RoleID)
	END
GO

/* sp_remove_role */

print '' print '*** creating sp_remove_role'
GO
CREATE PROCEDURE [sp_remove_role]
(
	@UserID			[int],
	@RoleID			[nvarchar](50)
)
AS
	BEGIN
		DELETE FROM [dbo].[UserRole]
		WHERE [UserID] =  @UserID
		AND [RoleID] =    @RoleID
	END
GO

/* sp_update_PasswordHash */

print '' print '*** creating sp_update_PasswordHash ***'
GO
CREATE PROCEDURE [dbo].[sp_update_PasswordHash]
(
	@Email				[nvarchar](100),
	@NewPasswordHash	[nvarchar](100),
	@OldPasswordHash	[nvarchar](100)
)
AS	
	BEGIN
		UPDATE	[User]
		SET	[PasswordHash] = @NewPasswordHash
		WHERE 	@Email = [Email]
		  AND   @OldPasswordHash = [PasswordHash]
		  /* System Super Global @@ - Set by the system */
		RETURN  @@ROWCOUNT
	END
GO

/* sp_update_user */

print '' print '*** creating sp_update_user ***'
GO
CREATE PROCEDURE [dbo].[sp_update_user]
(
	@UserID				[int],
	@NewFirstName		[nvarchar](50),
	@NewLastName 		[nvarchar](50),
	@NewImageFilePath	[nvarchar](500),
	@OldFirstName		[nvarchar](50),
	@OldLastName		[nvarchar](50),
	@OldImageFilePath	[nvarchar](500),
	@OldMinutesListened [int],	
	@NewMinutesListened [int],
	@OldActive 			[bit],
	@NewActive 			[bit]	
)
AS	
	BEGIN
		UPDATE [User]
		SET		[FirstName] = @NewFirstName,
				[LastName] = @NewLastName,
				[ImageFilePath] = @NewImageFilePath,
				[MinutesListened] = @NewMinutesListened,
				[Active] = @NewActive
		WHERE 	[UserID] = @UserID 
		  AND   [FirstName] = @OldFirstName
		  AND	[LastName] = @OldLastName 
		  AND	[ImageFilePath] = @OldImageFilePath
		  AND   [MinutesListened] = @OldMinutesListened
		  AND   [Active] = @OldActive
	END
GO

/* sp_update_minutesListened */

print '' print '*** creating sp_update_minutesListened ***'
GO
CREATE PROCEDURE [dbo].[sp_update_minutesListened]
(
	@UserID	[int],
	@NewMinutesListened [int]
)
AS	
	BEGIN
		UPDATE [User]
		SET	[MinutesListened] = @NewMinutesListened
		WHERE 	[UserID] = @UserID 
	END
GO

/* sp_update_active_user */

print '' print '*** creating sp_update_active_user ***'
GO
CREATE PROCEDURE [dbo].[sp_update_active_user]
(
	@UserID	[int],
	@NewActive	[Bit],
	@OldActive	[Bit]
)
AS	
	BEGIN
		UPDATE	[User]
		SET	[Active] = @NewActive
		WHERE 	@UserID = [UserID]
		  AND   @OldActive = [Active]
	END
GO

/* sp_update_online_privacy_status */

print '' print '*** creating sp_update_online_privacy_status ***'
GO
CREATE PROCEDURE [dbo].[sp_update_online_privacy_status]
(
	@UserID		 [int],
	@NewisPublic [bit],
	@OldisPublic [bit]
)
AS	
	BEGIN
		UPDATE	[User]
		SET	[isPublic] = @NewisPublic
		WHERE 	@UserID = [UserID]
		  AND   @OldisPublic = [isPublic]
	END
GO

/* Album Stored Procedures */

/* sp_insert_album */

print '' print '*** creating sp_insert_album ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_album]
(
	@Title			[nvarchar](100),
	@IsExplicit	    [bit],
	@ArtistID		[nvarchar](200),
	@ImageFilePath  [nvarchar](500),
	@Description 	[text],
	@YearReleased   [int]
)
AS
	BEGIN
		DECLARE @DateAdded DATETIME = GETDATE()
		INSERT INTO [dbo].[Album]
			([Title], [ArtistID], [ImageFilePath], [Description], [YearReleased], [DateAdded])
		VALUES
			(@Title, @ArtistID, @ImageFilePath, @Description, @YearReleased, @DateAdded)
	END
GO

/* sp_insert_song_into_album */

PRINT '' PRINT '*** creating sp_insert_song_into_album ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_song_into_album]
(
    @SongID     [int],
    @AlbumID    [int]
)
AS
    BEGIN
        INSERT INTO [dbo].[SongAlbum] 
            ([SongID], [AlbumID])
        VALUES (@SongID, @AlbumID)
    END
GO

/* sp_remove_song_from_album */

print '' print '*** creating sp_remove_song_from_album ***'
GO
CREATE PROCEDURE [dbo].[sp_remove_song_from_album]
(
	@SongID [int]
)
AS
	BEGIN
		DELETE FROM [SongAlbum]
		WHERE [SongID] = @SongID
	END
GO

/* sp_select_songs_by_albumID */

print '' print '*** creating sp_select_songs_by_albumID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_songs_by_albumID]
(
	@AlbumID		[int]
)
AS
	BEGIN
		SELECT	[Song].[SongID], 
				[Song].[Title],
				[Song].[ImageFilePath],
				[Song].[Mp3FilePath],
				[Song].[YearReleased],
				[Song].[Lyrics],
				[Song].[Explicit], 
				[Song].[Genre],
				[Song].[Plays],
				[Song].[UserID],				
				[SongArtist].[ArtistID],
				[Album].[Title],
				[Song].[DateUploaded],
				[Song].[DateAdded],
				[Song].[isLiked],
				[Song].[isPublic]
  
		FROM	[Song] JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
					   LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
					   LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		WHERE [Album].[AlbumID] = @AlbumID
	END
GO

/* sp_select_album_by_AlbumID */

print '' print '*** creating sp_select_album_by_AlbumID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_album_by_AlbumID]
(
	@AlbumID		[int]
)
AS
	BEGIN
		SELECT [AlbumID], 
			   [Title], 
			   [ArtistID], 	
			   [IsExplicit], 
			   [ImageFilePath],
			   [Photo], 
			   [PhotoMimeType], 
			   [Description], 
			   [YearReleased], 
			   [DateAdded]
		FROM [Album]
		WHERE [AlbumID] = @AlbumID
	END
GO

/* sp_update_album */

print '' print '*** creating sp_update_album ***'
GO
CREATE PROCEDURE [dbo].[sp_update_album]
(
	@AlbumID		  [int],
	@NewTitle		  [nvarchar](100),
	@NewImageFilePath [nvarchar](500),
	@NewDescription   [nvarchar](max),
	@NewYearReleased  [int],
	@NewPhoto		  [varbinary](MAX),
	@NewPhotoMimeType [nvarchar](50)
)
AS
	BEGIN
		UPDATE [Album] 
		SET [Title] = @NewTitle, 
			[ImageFilePath] = @NewImageFilePath, 
			[Description] = @NewDescription,
			[YearReleased] = @NewYearReleased
			[Photo] = @NewPhoto
			[PhotoMimeType] = @NewPhotoMimeType
		WHERE [AlbumID] = @AlbumID
	END
GO

/* sp_delete_album */

print '' print '*** creating sp_delete_album ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_album]
(
	@AlbumID [int]
)
AS
	BEGIN
		DELETE FROM [Album]
		WHERE [AlbumID] = @AlbumID
	END
GO

/* sp_select_albumid_from_albumtitle */

print '' print '*** creating sp_select_albumid_from_albumtitle ***'
GO
CREATE PROCEDURE [dbo].[sp_select_albumid_from_albumtitle]
(
	@AlbumTitle [nvarchar](100),
	@ArtistID   [nvarchar](200)
)
AS	
	BEGIN
		SELECT TOP 1 [AlbumID]
		FROM [Album]
		WHERE [Title] = @AlbumTitle
		AND   [ArtistID] = @ArtistID
	END
GO

/* Song Stored Procedures */

/* sp_retrieve_title_from_albumId */

print '' print '*** creating sp_retrieve_title_from_albumId ***'
GO

CREATE PROCEDURE [dbo].[sp_retrieve_title_from_albumId]
(
    @ArtistID       [nvarchar](200),
    @AlbumTitle     [nvarchar](255)
)
AS
	BEGIN
		SELECT AlbumID
		FROM [dbo].[Album]
		WHERE [Title] = @AlbumTitle
		AND [ArtistID] = @ArtistID 
	END
GO

/* sp_insert_into_SongArtist */

print '' print '*** creating sp_insert_into_SongArtist ***'
GO

CREATE PROCEDURE [dbo].[sp_insert_into_SongArtist]
(
    @SongID       [int],
    @ArtistID     [nvarchar](200)
)
AS
	BEGIN
		INSERT INTO [dbo].[SongArtist] 
				([SongID], [ArtistID], [isFeaturing])
		VALUES 
				(@SongID, @ArtistID, 0)
	END
GO

/* sp_insert_into_SongAlbum*/

print '' print '*** creating sp_insert_into_SongAlbum ***'
GO

CREATE PROCEDURE [dbo].[sp_insert_into_SongAlbum]
(
    @SongID       [int],
    @AlbumID      [int]
)
AS
	BEGIN
		INSERT INTO [dbo].[SongAlbum] 
				([SongID], [AlbumID])
		VALUES 
				(@SongID, @AlbumID)
	END
GO

print '' print '*** creating sp_insert_song ***'
GO

CREATE PROCEDURE [dbo].[sp_insert_song]
(
    @Title           [nvarchar](180),
    @ImageFilePath   [nvarchar](500),
    @Mp3FilePath     [nvarchar](500),
    @YearReleased    [int],
    @Lyrics          [text],
    @Explicit        [bit],
    @Genre           [nvarchar](150),
    @Plays           [int],
    @UserID          [int],
    @ArtistID        [nvarchar](200),
    @AlbumTitle      [nvarchar](255)
)
AS
	BEGIN
		DECLARE @SongID [int]
		DECLARE @AlbumID [int]

		-- check if @artistID already exists
		IF NOT EXISTS (SELECT 1 FROM [dbo].[Artist] WHERE [ArtistID] = @ArtistID)
		BEGIN
			INSERT INTO [dbo].[Artist] ([ArtistID])
			VALUES (@ArtistID)
		END

		-- get albumID from the albumtitle and the artist
		SELECT @AlbumID = AlbumID
		FROM [dbo].[Album]
		WHERE [Title] = @AlbumTitle
		AND [ArtistID] = @ArtistID

		-- make new album if it doesnt exist
		IF @AlbumID IS NULL
		BEGIN
			INSERT INTO [dbo].[Album] 
				([Title], [ArtistID], [ImageFilePath])
			VALUES (@AlbumTitle, @ArtistID, @ImageFilePath)

			SELECT @AlbumID = SCOPE_IDENTITY()
		END

		-- insert song
		INSERT INTO [dbo].[Song] 
			([Title], [Mp3FilePath], [ImageFilePath], [YearReleased], [Lyrics], 
			[Explicit], [Genre], [Plays], [UserID], [ArtistID], [AlbumID])
		VALUES 
			(@Title, @Mp3FilePath, @ImageFilePath, @YearReleased, @Lyrics, 
			@Explicit, @Genre, @Plays, @UserID, @ArtistID, @AlbumID)

		-- get song id from the created song
		SELECT @SongID = SCOPE_IDENTITY()

		-- add to SongArtist table
		IF @ArtistID IS NOT NULL AND @ArtistID <> ''
		BEGIN
			INSERT INTO [dbo].[SongArtist] 
				([SongID], [ArtistID])
			VALUES 
				(@SongID, @ArtistID)
		END

		-- add to SongAlbum table
		IF @AlbumID IS NOT NULL AND @AlbumID <> ''
		BEGIN
			INSERT INTO [dbo].[SongAlbum] 
				([SongID], [AlbumID])
			VALUES 
				(@SongID, @AlbumID)
		END
	END
GO

/* sp_select_songs_by_UserID */

print '' print '*** creating sp_select_songs_by_UserID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_songs_by_UserID]
(
	@UserID			[int]
)
AS	
	BEGIN
		SELECT	[Song].[SongID], 
				[Song].[Title],
				[Song].[ImageFilePath],
				[Song].[Mp3FilePath],
				[Song].[YearReleased],
				[Song].[Lyrics],
				[Song].[Explicit], 
				[Song].[Genre],
				[Song].[Plays],
				[Song].[UserID],				
				[SongArtist].[ArtistID],
				[Album].[Title],
				[Song].[DateUploaded],
				[Song].[DateAdded],
				[Song].[isLiked],
				[Song].[isPublic]
  
		FROM	[Song] JOIN [User] ON [Song].[UserID] = [User].[UserID]
					   JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
					   LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
					   LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		WHERE 	@UserID = [User].[UserID]
	END
GO

print '' print '*** creating sp_update_song ***'
GO
CREATE PROCEDURE [dbo].[sp_update_song]
(
    @SongID int,
    @NewTitle nvarchar(180),
    @NewImageFilePath nvarchar(500),
    @NewYearReleased int,
    @NewLyrics nvarchar(max),
    @NewExplicit bit,
    @NewGenre nvarchar(200),
    @NewPlays int,
    @NewArtistID nvarchar(200),
    @NewAlbumTitle nvarchar(255),
    @NewIsLiked bit,
	@NewPhoto		  [varbinary](MAX),
	@NewPhotoMimeType [nvarchar](50)
)
AS
	BEGIN
		DECLARE @AlbumID int

		-- check if @artistID already exists
		IF NOT EXISTS (SELECT 1 FROM [dbo].[Artist] WHERE [ArtistID] = @NewArtistID)
		BEGIN
			INSERT INTO [dbo].[Artist] ([ArtistID])
			VALUES (@NewArtistID)
		END

		-- get albumID from the albumtitle and the artist
		SELECT @AlbumID = AlbumID
		FROM [dbo].[Album]
		WHERE [Title] = @NewAlbumTitle
		AND [ArtistID] = @NewArtistID

		-- make new album if it doesn't exist
		IF @AlbumID IS NULL
		BEGIN
			INSERT INTO [dbo].[Album]
				([Title], [ArtistID])
			VALUES (@NewAlbumTitle, @NewArtistID)

			SELECT @AlbumID = SCOPE_IDENTITY()
		END

		-- Update the song
		UPDATE [Song]
		SET [Title] = @NewTitle,
			[ImageFilePath] = @NewImageFilePath,
			[Photo] = @NewPhoto,	
			[PhotoMimeType] = @NewPhotoMimeType,
			[YearReleased] = @NewYearReleased,
			[Lyrics] = @NewLyrics,
			[Explicit] = @NewExplicit,
			[Genre] = @NewGenre,
			[Plays] = @NewPlays,
			[ArtistID] = @NewArtistID,
			[AlbumID] = @AlbumID,
			[IsLiked] = @NewIsLiked
		WHERE [SongID] = @SongID

		-- add to SongArtist table
		IF @NewArtistID IS NOT NULL AND @NewArtistID <> ''
		BEGIN
			INSERT INTO [dbo].[SongArtist]
				([SongID], [ArtistID])
			VALUES
				(@SongID, @NewArtistID)
		END

		-- add to SongAlbum table
		IF @AlbumID IS NOT NULL
		BEGIN
			INSERT INTO [dbo].[SongAlbum]
				([SongID], [AlbumID])
			VALUES
				(@SongID, @AlbumID)
		END
    END
GO

/* sp_update_favorite_song */

print '' print '*** creating sp_update_favorite_song ***'
GO
CREATE PROCEDURE [dbo].[sp_update_favorite_song]
(
	@SongID	[int],
	@NewisLiked [bit]
)
AS	
	BEGIN
		UPDATE [Song]
		SET	[isLiked] = @NewisLiked
		WHERE 	[SongID] = @SongID 
	END
GO

/* sp_update_song_plays */

print '' print '*** creating sp_update_song_plays ***'
GO
CREATE PROCEDURE [dbo].[sp_update_song_plays]
(
	@SongID	[int],
	@NewPlays [int]
)
AS	
	BEGIN
		UPDATE [Song]
		SET	[Plays] = @NewPlays
		WHERE 	[SongID] = @SongID 
	END
GO

/* sp_delete_song */

print '' print '*** creating sp_delete_song ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_song]
(
	@SongID [int]
)
AS
	BEGIN
		DELETE FROM [Song]
		WHERE [SongID] = @SongID
	END
GO

/* Review Stored Procedures */

/* sp_create_review */

print '' print '*** creating sp_create_review ***'
GO
CREATE PROCEDURE [dbo].[sp_create_review]
(
	@Rating   [int],
	@Message  [nvarchar](max),
	@UserID   [int],					
	@SongID   [int]
)	
AS
	BEGIN
		INSERT INTO [dbo].[Review] 
			([Rating], [Message], [UserID], [SongID])
		VALUES (@Rating, @Message, @UserID, @SongID)
	END
GO

/* sp_select_reviews_by_UserID */

print '' print '*** creating sp_select_reviews_by_UserID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_reviews_by_UserID]
(
	@UserID [int]
)
AS
	BEGIN
		SELECT [Song].[SongID], 
			   [Song].[Title],
			   [Song].[ImageFilePath],
			   [Song].[Mp3FilePath],
			   [Song].[YearReleased],
			   [Song].[Lyrics],
			   [Song].[Explicit], 
			   [Song].[Genre],
			   [Song].[Plays],
			   [Song].[UserID],				
			   [SongArtist].[ArtistID],
			   [Album].[Title],
			   [Song].[DateUploaded],
			   [Song].[DateAdded],
			   [Song].[isLiked],
			   [Review].[ReviewID], 
			   [Review].[Rating], 
			   [Review].[Message], 
			   [Review].[UserID]
		FROM [Review]
		JOIN [Song] ON [Review].[SongID] = [Song].[SongID]
		JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
		LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
		LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		JOIN [User] ON [Review].[UserID] = [User].[UserID]
		WHERE [Review].[UserID] = @UserID
	END
GO

/* sp_select_review_by_ReviewID */

print '' print '*** creating sp_select_review_by_ReviewID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_review_by_ReviewID]
(
	@UserID [int],
	@ReviewID [int]
)
AS
	BEGIN
		SELECT [Song].[SongID], 
			   [Song].[Title],
			   [Song].[ImageFilePath],
			   [Song].[Mp3FilePath],
			   [Song].[YearReleased],
			   [Song].[Lyrics],
			   [Song].[Explicit], 
			   [Song].[Genre],
			   [Song].[Plays],
			   [Song].[UserID],				
			   [SongArtist].[ArtistID],
			   [Album].[Title],
			   [Song].[DateUploaded],
			   [Song].[DateAdded],
			   [Song].[isLiked],
			   [Review].[ReviewID], 
			   [Review].[Rating], 
			   [Review].[Message], 
			   [Review].[UserID]
		FROM [Review]
		JOIN [Song] ON [Review].[SongID] = [Song].[SongID]
		JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
		LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
		LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		JOIN [User] ON [Review].[UserID] = [User].[UserID]
		WHERE [Review].[UserID] = @UserID
	END
GO

/* sp_update_review */

print '' print '*** creating sp_update_review ***'
GO
CREATE PROCEDURE [dbo].[sp_update_review]
(
	@ReviewID   [int],
	@NewRating  [int],
	@NewMessage [nvarchar](max),	
	@OldRating  [int],
	@OldMessage [nvarchar](max),		
	@UserID     [int],							
	@SongID     [int]
)
AS
	BEGIN
		UPDATE [Review]
		SET [Rating] = @NewRating,
			[Message] = @NewMessage	
		WHERE [ReviewID] = @ReviewID
		AND [Rating] = @OldRating
		AND [Message] = @OldMessage
		AND [UserID] = @UserID
		AND [SongID] = @SongID
	END
GO

/* sp_delete_review */

print '' print '*** creating sp_delete_review ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_review]
(
	@ReviewID [int]
)
AS
	BEGIN
		DELETE FROM [Review]
		WHERE [ReviewID] = @ReviewID
	END
GO

/* Playlist Stored Procedures */

/* sp_create_playlist */

print '' print '*** creating sp_create_playlist ***'
GO
CREATE PROCEDURE [dbo].[sp_create_playlist]
(
	@Title			[nvarchar](50),
	@ImageFilePath  [nvarchar](500),
	@Photo			[varbinary](max),
	@Description	[Text],
	@UserID			[int]
)
AS	
	BEGIN
		INSERT INTO [dbo].[Playlist] 
			([Title], [ImageFilePath], [Photo], [Description], [UserID])
		VALUES (@Title, @ImageFilePath, @Photo, @Description, @UserID)
	END
GO

/* sp_select_playlists_by_UserID */

print '' print '*** creating sp_select_playlists_by_UserID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_playlists_by_UserID]
(
	@UserID			[int]
)
AS	
	BEGIN
		SELECT	[PlaylistID], 
				[Title], 
				[Playlist].[ImageFilePath], 
				[Playlist].[Photo],
				[Playlist].[PhotoMimeType],
				[Description], 
				[User].[UserID]
		FROM	[Playlist] JOIN [User] ON [Playlist].[UserID] = [User].[UserID]
		WHERE 	@UserID = [User].[UserID]
	END
GO

/* sp_select_playlist_by_UserID */

print '' print '*** creating sp_select_playlist_by_UserID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_playlist_by_UserID]
(
	@UserID			[int],
    @PlaylistID     [int]
)
AS	
	BEGIN
		SELECT	[PlaylistID], 
				[Title], 
				[Playlist].[ImageFilePath], 
				[Playlist].[Photo], 
				[Playlist].[PhotoMimeType], 
				[Description], 
				[User].[UserID]
		FROM	[Playlist] JOIN [User] ON [Playlist].[UserID] = [User].[UserID]
		WHERE 	@UserID = [User].[UserID] AND @PlaylistID = [Playlist].[PlaylistID]
	END
GO

/* sp_update_playlist */

print '' print '*** creating sp_update_playlist ***'
GO
CREATE PROCEDURE [dbo].[sp_update_playlist]
(
	@PlaylistID		  [int],
	@NewTitle 		  [nvarchar](180), 
	@NewImageFilePath [nvarchar](500), 
	@NewDescription   [nvarchar](max), 
	@OldTitle         [nvarchar](180), 
	@OldImageFilePath [nvarchar](500), 
	@OldDescription   [nvarchar](max),
	@NewPhoto		  [varbinary](MAX),
	@NewPhotoMimeType [nvarchar](50)
)
AS
	BEGIN
		UPDATE [Playlist]
		SET [Title] = @NewTitle,
			[ImageFilePath] = @NewImageFilePath,
			[Description] = @NewDescription,
			[Photo] = @NewPhoto,
			[PhotoMimeType] = @NewPhotoMimeType
		WHERE [PlaylistID] = @PlaylistID
	END
GO

/* sp_delete_playlist */

print '' print '*** creating sp_delete_playlist ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_playlist]
(
	@PlaylistID [int]
)
AS
	BEGIN
		DELETE FROM [Playlist]
		WHERE [PlaylistID] = @PlaylistID
	END
GO

/* PlaylistSong Stored Procedures */

/* sp_select_songs_by_PlaylistID */

print '' print '*** creating sp_select_songs_by_PlaylistID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_songs_by_PlaylistID]
(
    @UserID     [int],
    @PlaylistID [int]
)
AS
	BEGIN
		SELECT
			[Song].[SongID], 
				[Song].[Title],
				[Song].[ImageFilePath],
				[Song].[Mp3FilePath],
				[Song].[YearReleased],
				[Song].[Lyrics],
				[Song].[Explicit], 
				[Song].[Genre],
				[Song].[Plays],
				[Song].[UserID],				
				[SongArtist].[ArtistID],
				[Album].[Title],
				[Song].[DateUploaded],
				[Song].[DateAdded],
				[Song].[isLiked]
		FROM
			[Song]
		JOIN [User] ON [Song].[UserID] = [User].[UserID]
		JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
		LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
		LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		LEFT JOIN [PlaylistSong] ON [Song].[SongID] = [PlaylistSong].[SongID]
		LEFT JOIN [Playlist] ON [PlaylistSong].[PlaylistID] = [Playlist].[PlaylistID]
		WHERE
			[Song].[UserID] = @UserID
			AND [Playlist].[PlaylistID] = @PlaylistID
	END
GO

/* sp_insert_song_into_playlist */

PRINT '' PRINT '*** creating sp_insert_song_into_playlist ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_song_into_playlist]
(
    @SongID     [int],
    @PlaylistID [int]
)
AS
    BEGIN
        DECLARE @TimeAdded DATETIME = GETDATE()

        INSERT INTO [dbo].[PlaylistSong] 
            ([SongID], [PlaylistID], [TimeAdded])
        VALUES (@SongID, @PlaylistID, @TimeAdded)
    END
GO

/* sp_remove_song_from_playlist */

print '' print '*** creating sp_remove_song_from_playlist ***'
GO
CREATE PROCEDURE [dbo].[sp_remove_song_from_playlist]
(
	@SongID [int]
)
AS
	BEGIN
		DELETE FROM [PlaylistSong]
		WHERE [SongID] = @SongID
	END
GO

/* sp_select_all_albums */

print '' print '*** creating sp_select_all_albums ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_albums]
AS
	BEGIN
		SELECT  [AlbumID], 
				[Title], 
				[ArtistID],
				[isExplicit],
				[ImageFilePath],
				[Photo],
				[PhotoMimeType],
				[Description],
				[YearReleased],
				[DateAdded]
		FROM	[Album]
	END
GO

/* sp_select_friends_from_UserID */

print '' print '*** creating sp_select_friends_from_UserID ***'
GO

CREATE PROCEDURE [dbo].[sp_select_friends_from_UserID]
(
    @UserID [int]
)
AS
	BEGIN
		SELECT  [User].[UserID],
				[User].[ProfileName], 
				[User].[FirstName], 
				[User].[LastName], 
				[User].[Email], 
				[User].[ImageFilePath], 
				[User].[Active], 
				[User].[MinutesListened], 
				[User].[isPublic], 
				[UserFriend].[DayAddedAsFriend]
		FROM    [UserFriend]
		INNER JOIN [User] ON [UserFriend].[FriendID] = [User].[UserID]
		WHERE   [UserFriend].[UserID] = @UserID
	END
GO

/* sp_select_all_genres_from_songs */

CREATE PROCEDURE [dbo].[sp_select_all_genres_from_songs]
AS
	BEGIN
		SELECT DISTINCT [Genre]
		FROM [Song]
	END
GO



/* sp_select_song_by_SongID */

print '' print '*** creating sp_select_song_by_SongID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_song_by_SongID]
(
	@UserID			[int],
	@SongID			[int]
)
AS	
	BEGIN
		SELECT	[Song].[SongID], 
				[Song].[Title],
				[Song].[ImageFilePath],
				[Song].[Photo],
				[Song].[PhotoMimeType],
				[Song].[Mp3FilePath],
				[Song].[YearReleased],
				[Song].[Lyrics],
				[Song].[Explicit], 
				[Song].[Genre],
				[Song].[Plays],
				[Song].[UserID],				
				[SongArtist].[ArtistID],
				[Album].[Title],
				[Song].[DateUploaded],
				[Song].[DateAdded],
				[Song].[isLiked],
				[Song].[isPublic]

		FROM	[Song] JOIN [User] ON [Song].[UserID] = [User].[UserID]
					   JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
					   LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
					   LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		WHERE 	@UserID = [User].[UserID] AND @SongID = [Song].[SongID]
	END
GO

/* Artist Stored Procedures */

/* sp_select_artist_by_ArtistID */

print '' print '*** creating sp_select_artist_by_ArtistID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_artist_by_ArtistID]
(
	@ArtistID	    [nvarchar](200)
)
AS	
	BEGIN
		SELECT  [ArtistID], 		
				[ImageFilePath],
				[Photo],
				[PhotoMimeType],
				[FirstName],
				[LastName],		
				[Description],    
				[isLiked],       
				[DateOfBirth]
		FROM	[Artist] 
		WHERE 	[ArtistID] = @ArtistID
	END
GO

/* sp_select_all_artists */

print '' print '*** creating sp_select_all_artists ***'
GO
CREATE PROCEDURE [dbo].[sp_select_all_artists]
AS	
	BEGIN
		SELECT  [ArtistID], 		
				[ImageFilePath],
				[Photo],
				[PhotoMimeType],
				[FirstName],
				[LastName],		
				[Description],    
				[isLiked],       
				[DateOfBirth]
		FROM	[Artist]
	END
GO

/* sp_select_songs_by_ArtistID */

print '' print '*** creating sp_select_songs_by_ArtistID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_songs_by_ArtistID]
(
	@ArtistID	    [int]
)
AS	
	BEGIN
		SELECT	[Song].[SongID], 
				[Song].[Title],
				[Song].[ImageFilePath],
				[Song].[Photo],
				[Song].[PhotoMimeType],
				[Song].[Mp3FilePath],
				[Song].[YearReleased],
				[Song].[Lyrics],
				[Song].[Explicit], 
				[Song].[Genre],
				[Song].[Plays],
				[Song].[UserID],				
				[SongArtist].[ArtistID],
				[Album].[Title],
				[Song].[isLiked]
  
		FROM	[Song] JOIN [User] ON [Song].[UserID] = [User].[UserID]
					   JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
					   LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
					   LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		WHERE 	@ArtistID = [SongArtist].[ArtistID]
	END
GO

/* sp_update_artist */

/* sp_delete_artist */

print '' print '*** creating sp_delete_artist ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_artist]
(
	@ArtistID [nvarchar]
)
AS	
	BEGIN
		Delete	[Artist]
		WHERE 	[ArtistID] = @ArtistID
	END
GO