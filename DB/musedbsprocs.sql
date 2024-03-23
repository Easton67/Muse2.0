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
	@PasswordHash [nvarchar](100),
	@Email [nvarchar](100),
	@ProfileName [nvarchar](200),
	@ImageFilePath [nvarchar](500)
)
AS
	BEGIN
		INSERT INTO [dbo].[User]
				([ProfileName], [Email], [PasswordHash], [ImageFilePath])
			VALUES
				(@ProfileName, @Email, @PasswordHash, @ImageFilePath)
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
		SELECT [UserID], [ProfileName], [Email], [FirstName], [LastName], [ImageFilePath], [Active], [MinutesListened]
		FROM	[User]
		WHERE	@Email = [Email]
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
			   [Active], 
			   [MinutesListened]
		FROM   [User]
	END
GO

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
	@OldTitle 		  [nvarchar](100),
	@OldImageFilePath [nvarchar](500),
	@OldDescription   [nvarchar](max)
)
AS
	BEGIN
		UPDATE [Album] 
		SET [Title] = @NewTitle, 
			[ImageFilePath] = @NewImageFilePath, 
			[Description] = @NewDescription
		WHERE [AlbumID] = @AlbumID
		AND	  [Title] = @OldTitle
		AND   [ImageFilePath] = @OldImageFilePath 
		AND   [Description] = @OldDescription
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

/* Song Stored Procedures */

/* sp_insert_song */

print '' print '*** creating sp_insert_song ***'
GO

CREATE PROCEDURE [dbo].[sp_insert_song]
(
    @Title          [nvarchar](180),
    @ImageFilePath  [nvarchar](500),
    @Mp3FilePath    [nvarchar](500),
    @YearReleased   [int],
    @Lyrics         [text],
    @Explicit       [bit],
    @Genre          [nvarchar](150),
    @Plays          [int],
    @UserID         [int],
    @ArtistID       [nvarchar](200),
    @AlbumTitle     [nvarchar](255),
    @DateAdded      [datetime]
)
AS
	BEGIN
		DECLARE @SongID INT
		DECLARE @AlbumID INT

		-- Check if the album exists based on title
		SELECT @AlbumID = AlbumID
		FROM [dbo].[Album]
		WHERE [Title] = @AlbumTitle
		AND [ArtistID] = @ArtistID ;

		-- If the album doesn't exist, insert a new one
		IF @AlbumID IS NULL
		BEGIN
			INSERT INTO [dbo].[Album] ([Title])
			VALUES (@AlbumTitle);

			SET @AlbumID = SCOPE_IDENTITY();
		END

		INSERT INTO [dbo].[Song] 
			([Title], [Mp3FilePath], [ImageFilePath], [YearReleased], [Lyrics], 
			[Explicit], [Genre], [Plays], [UserID], [AlbumID], [DateAdded])
		VALUES 
			(@Title, @Mp3FilePath, @ImageFilePath, @YearReleased, @Lyrics, 
			@Explicit, @Genre, @Plays, @UserID, @AlbumID, @DateAdded);

		SET @SongID = SCOPE_IDENTITY()

		INSERT INTO [dbo].[SongArtist] 
			([SongID], [ArtistID])
		VALUES 
			(@SongID, @ArtistID)

		-- <> meaning not equal to 
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
				[Song].[DateAdded]
  
		FROM	[Song] JOIN [User] ON [Song].[UserID] = [User].[UserID]
					   JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
					   LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
					   LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		WHERE 	@UserID = [User].[UserID]
	END
GO

/* sp_update_song */

print '' print '*** creating sp_update_song ***'
GO
CREATE PROCEDURE [dbo].[sp_update_song]
(
	@SongID [int],
	@NewTitle [nvarchar](180),
	@NewImageFilePath [nvarchar](500),
	@NewYearReleased [int],
	@NewLyrics [nvarchar](max),
	@NewExplicit [bit],
	@NewPlays [int],
	@OldTitle [nvarchar](180),
	@OldImageFilePath [nvarchar](500),
	@OldYearReleased [int],
	@OldLyrics [nvarchar](max),
	@OldExplicit [bit],
	@OldPlays [int]
)
AS
	BEGIN
		UPDATE [Song]
		SET [Title] = @NewTitle,
			[ImageFilePath] = @NewImageFilePath,
			[YearReleased] = @NewYearReleased,
			[Lyrics] = @NewLyrics,
			[Explicit] = @NewExplicit,
			[Plays] = @NewPlays
		WHERE [SongID] = @SongID
		AND [Title] = @OldTitle
		AND [ImageFilePath] = @OldImageFilePath
		AND [YearReleased] = @OldYearReleased
		AND [Lyrics] = @OldLyrics
		AND [Explicit] = @OldExplicit
		AND [Plays] = @OldPlays
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

/* sp_select_reviews_by_ReviewID */

print '' print '*** creating sp_select_reviews_by_UserID ***'
GO
CREATE PROCEDURE [dbo].[sp_select_reviews_by_UserID]
(
	@UserID [int]
)
AS
	BEGIN
		SELECT [Review].[ReviewID], [Review].[Rating], 
			   [Review].[Message], [Review].[UserID], 
			   [Song].[SongID], [Song].Title, 
			   [Song].YearReleased, [SongArtist].ArtistID, 
			   [Song].ImageFilePath, [Song].Mp3FilePath, 
			   [Song].Explicit
		FROM [dbo].[Review]
		JOIN [dbo].[Song] ON [Review].[SongID] = [Review].[SongID]
		JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
		WHERE [Review].[UserID] = @UserID
		AND [Song].SongID = [Review].[SongID]
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
		SELECT [Review].[ReviewID], [Review].[Rating], 
			[Review].[Message], [Review].[UserID], 
			[Song].[SongID], [Song].[Title], 
			[Song].[YearReleased], [SongArtist].[ArtistID], 
			[Song].[ImageFilePath], [Song].[Mp3FilePath], 
			[Song].[Explicit]
		FROM [dbo].[Review]
		JOIN [dbo].[Song] ON [Review].[SongID] = [Song].[SongID]
		JOIN [dbo].[SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
		WHERE [Review].[UserID] = @UserID
		AND [Review].[ReviewID] = @ReviewID
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
		DELETE FROM [Reviews]
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
	@Description	[Text],
	@UserID			[int]
)
AS	
	BEGIN
		INSERT INTO [dbo].[Playlist] 
			([Title], [ImageFilePath], [Description], [UserID])
		VALUES (@Title, @ImageFilePath, @Description, @UserID)
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
		SELECT	[PlaylistID], [Title], [Playlist].[ImageFilePath], [Description], [User].[UserID]
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
		SELECT	[PlaylistID], [Title], [Playlist].[ImageFilePath], [Description], [User].[UserID]
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
	@OldDescription   [nvarchar](max)
)
AS
	BEGIN
		UPDATE [Playlist]
		SET [Title] = @NewTitle,
			[ImageFilePath] = @NewImageFilePath,
			[Description] = @NewDescription
		WHERE [PlaylistID] = @PlaylistID
		AND [Title] = @OldTitle
		AND [ImageFilePath] = @OldImageFilePath
		AND [Description] = @OldDescription
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
			[Song].[Plays],
			[Song].[UserID],
			[SongArtist].[ArtistID],
			[Album].[Title]
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
				[UserFriend].[DayAddedAsFriend]
		FROM    [UserFriend]
		INNER JOIN [User] ON [UserFriend].[FriendID] = [User].[UserID]
		WHERE   [UserFriend].[UserID] = @UserID
	END
GO

/* sp_select_friends_from_UserID */

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
				[Song].[DateAdded]
  
		FROM	[Song] JOIN [User] ON [Song].[UserID] = [User].[UserID]
					   JOIN [SongArtist] ON [Song].[SongID] = [SongArtist].[SongID]
					   LEFT JOIN [SongAlbum] ON [Song].[SongID] = [SongAlbum].[SongID]
					   LEFT JOIN [Album] ON [SongAlbum].[AlbumID] = [Album].[AlbumID]
		WHERE 	@UserID = [User].[UserID] AND @SongID = [Song].[SongID]
	END
GO

/* Artist Stored Procedures */

/* sp_select_artist_by_artist_id */

print '' print '*** creating sp_select_artist_by_artist_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_artist_by_artist_id]
(
	@ArtistID	    [int]
)
AS	
	BEGIN
		SELECT  [ArtistID], 		
				[ImageFilePath],
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
				[FirstName],
				[LastName],		
				[Description],    
				[isLiked],       
				[DateOfBirth]
		FROM	[Artist]
	END
GO

/* sp_select_songs_by_artist_id */

print '' print '*** creating sp_select_songs_by_artist_id ***'
GO
CREATE PROCEDURE [dbo].[sp_select_songs_by_artist_id]
(
	@ArtistID	    [int]
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
				[Song].[DateAdded]
  
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



