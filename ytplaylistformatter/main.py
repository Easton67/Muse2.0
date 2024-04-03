from pytube import Playlist, exceptions
from PIL import Image
import os
import lyricsgenius
import requests
import time
import re
import pylast
from profanity import profanity
from mutagen.easyid3 import EasyID3
from mutagen.mp3 import MP3
from bs4 import BeautifulSoup

words_to_replace = [" (Official Audio)", " (Audio)", " [Audio]", " (Official Lyric Video)", " (Unreleased)",
                    " (Official Video)", " (Official Music Video)", " [Official Music Video]",
                    " (Official Net Video)", " (Official HD Video)", " (Audio)", " (Visualizer)", " (Official)",
                    " (Visualizer)", " (Official Visualizer)", " [Official Visualizer]", " (Lyrics)",
                    " (Music Video)", " (Lyric Video)", " [Official Audio]", " [Official Video]",
                    " (Video Official)", " (Original Song)", " (HQ)", " [Official HD Audio]",
                    " (Official visualizer)", " (Video oficial)", "Feat", "Feat.", "feat.", "feat", " ft.", "ft", "?",
                    "\'", "\"", "(", ")", ":", ",", "\\", "/", "[Legendado]", "[Directed by Cole Bennett]", " - Video Edit", 
                    "*", '[Clean]', '[clean]', '-', '&', '|', '...']

api_key = "XAhf6ZzYrgiQx4j3DovglzBVz5frEurz6y86RcoHzhvmx5XGJ9JF6O9NpomHmaFg"
last_fm_api_key = "4fdf3c66636a44cea52bada67b7efd00"
last_fm_api_secret = "4192e8996c534794d6c5187c802eef8d"
genius = lyricsgenius.Genius(api_key, timeout=50)

# Initialize Last.fm API
network = pylast.LastFMNetwork(api_key=last_fm_api_key, api_secret=last_fm_api_secret)

def get_song_release_year(artist_name, song_title):
        try:
            # Get track from Last.fm
            track = network.get_track(artist_name, song_title)
            
            # Get release year
            release_date = track.get_wiki_published_date()
            release_year = release_date.split('-')[0] if release_date else "Release year not found"

            return release_year
        except Exception as e:
            print(f"Error fetching release year from Last.fm: {e}")
            return "Release year not found"

def get_genre_and_cover_and_album_name(artist_name, track_name):
    try:
        # Get track from Last.fm
        track = network.get_track(artist_name, track_name)
        
        # Get top tags (genres) from Last.fm
        tags = track.get_top_tags()
        genres = [tag.item.get_name() for tag in tags]

        # If genres list is empty or the first genre is "MySpotigramBot", get the next genre
        if not genres or genres[0] == "MySpotigramBot" or genres[0] == artist_name:
            first_genre = genres[1] if len(genres) > 1 else None
            return first_genre if first_genre else None, None, None

        # Get album name
        album = track.get_album()
        album_name = album.get_title() if album else "Unknown"
        
        # Get album cover image URL from Last.fm
        album_cover = album.get_cover_image() if album else None
        
        return genres[0], album_cover, album_name
    except Exception as e:
        print(f"Error fetching genres and album cover from Last.fm: {e}")
        return None, None, None

def download_and_process_playlist(playlist_url, playlist_title):
    # Load playlist
    playlist = Playlist(playlist_url)

    # List to store folder names
    folder_names = []

    # Loop over each video in the playlist
    for video in playlist.videos:
        try:
            # Get artist and song name
            artist = video.author
            song = video.title

            # Replace unwanted words in song title
            for word in words_to_replace:
                song = song.replace(word, "")
            song = song.strip()

            # Check if the song already exists in the current directory
            if any(os.path.exists(os.path.join(root, song)) for root, dirs, files in os.walk('.')):
                print(f"Song '{song}' already exists in the current directory. Skipping...")
                continue

            # Get genres, album name, and album cover from Last.fm
            genres, album_cover, album_name = get_genre_and_cover_and_album_name(artist, song)

            # Create directory for the song if it doesn't exist
            song_directory = os.path.join(os.getcwd(), song)
            if not os.path.exists(song_directory):
                os.makedirs(song_directory)

            # Download the audio stream
            audio_stream = video.streams.filter(only_audio=True).first()
            file_path = audio_stream.download(output_path='./', filename_prefix=song.replace(" ", "_"))

            # Move downloaded audio to the song directory and rename to mp3
            new_file_path = os.path.join(song_directory, f"{artist} - {song}.mp3")
            os.rename(file_path, new_file_path)

            # Get release year from Genius
            release_year = get_song_release_year(artist, song)

            # Search for lyrics using Genius API
            song_info = genius.search_song(song, artist)
            if song_info:
                lyrics = song_info.lyrics.split("\n", 1)[-1]
                
                lyrics_file_path = os.path.join(song_directory, f"{song}.txt")
                censored_lyrics = profanity.censor(lyrics)
                with open(lyrics_file_path, "w", encoding="utf-8") as lyrics_file:
                    lyrics_file.write(censored_lyrics)
                print(f"Lyrics saved to: {lyrics_file_path}")
            else:
                print(f"No lyrics found for {song} by {artist}")

            isExplicit = profanity.contains_profanity(lyrics)

            # Attempt to load MP3 file and add metadata
            try:
                audio = MP3(new_file_path, ID3=EasyID3)
                audio["title"] = song
                audio["artist"] = artist
                audio["album"] = album_name
                audio["genre"] = genres
                # Add more metadata as needed
                audio.save()
                print(f"Metadata added to '{new_file_path}'")
            except Exception as e:
                print(f"Error processing MP3 file '{new_file_path}': {e}")

            # Write song information to a file
            info_file_path = os.path.join(song_directory, f"{song}-information.txt")
            with open(info_file_path, "w", encoding="utf-8") as info_file:
                info_file.write(f"Title: {song}\n")
                info_file.write(f"Artist: {artist}\n")
                info_file.write(f"Release Year: {release_year}\n")
                info_file.write(f"Album Name: {album_name}\n")
                info_file.write(f"Genre: {genres}\n")
                info_file.write(f"Explicit: {isExplicit}\n")
            print(f"Song information saved to: {info_file_path}")

            # Download album cover or video thumbnail
            if album_cover:
                response = requests.get(album_cover)
                if response.status_code == 200:
                    # Save album cover
                    album_cover_filename = f"{song}.png"
                    album_cover_path = os.path.join(song_directory, album_cover_filename)
                    with open(album_cover_path, 'wb') as f:
                        f.write(response.content)
                    print(f"Album cover saved to: {album_cover_path}")
                    
                    # Resize album cover to 500x500 pixels
                    img = Image.open(album_cover_path)
                    img = img.resize((500, 500))
                    img.save(album_cover_path)
                    print(f"Album cover resized to 500x500 pixels")
                else:
                    print("Failed to download album cover from Last.fm. Downloading video thumbnail instead.")
                    thumbnail_url = video.thumbnail_url
                    thumbnail_filename = f"{song}.png"
                    response = requests.get(thumbnail_url)
                    if response.status_code == 200:
                        # Save thumbnail
                        thumbnail_path = os.path.join(song_directory, thumbnail_filename)
                        with open(thumbnail_path, 'wb') as f:
                            f.write(response.content)
                        print(f"Thumbnail saved to: {thumbnail_path}")
                        
                        # Resize thumbnail to 500x500 pixels
                        img = Image.open(thumbnail_path)
                        img = img.resize((500, 500))
                        img.save(thumbnail_path)
                        print(f"Thumbnail resized to 500x500 pixels")
            else:
                print("No album cover available. Downloading video thumbnail.")
                thumbnail_url = video.thumbnail_url
                thumbnail_filename = f"{song}.png"
                response = requests.get(thumbnail_url)
                if response.status_code == 200:
                    # Save thumbnail
                    thumbnail_path = os.path.join(song_directory, thumbnail_filename)
                    with open(thumbnail_path, 'wb') as f:
                        f.write(response.content)
                    print(f"Thumbnail saved to: {thumbnail_path}")
                    
                    # Resize thumbnail to 500x500 pixels
                    img = Image.open(thumbnail_path)
                    img = img.resize((500, 500))
                    img.save(thumbnail_path)
                    print(f"Thumbnail resized to 500x500 pixels")

            print(f"Song '{song}' processed and saved to '{song_directory}'")

            # Append folder name to the list
            folder_names.append(song_directory)

        except exceptions.AgeRestrictedError:
            print(f"Age-restricted video: {video.title}. Skipping...")

    print("Finished!")

    # Create mega folder with playlist title
    mega_folder_path = os.path.join(os.getcwd(), playlist_title)
    os.makedirs(mega_folder_path, exist_ok=True)

    # Move song folders into mega folder
    for folder_name in folder_names:
        folder_name = os.path.basename(folder_name)
        os.rename(os.path.join(os.getcwd(), folder_name), os.path.join(mega_folder_path, folder_name))

    print(f"All songs moved to mega folder '{playlist_title}'")

# Get the playlist URL and title from the user
playlist_url = input("\nEnter your playlist URL: ")
playlist_title = input("\nEnter your playlist Title: ")

# Start a timer
start_time = time.time()
print("\nStarting Timer...")

# Download and process the playlist
download_and_process_playlist(playlist_url, playlist_title)

# Calculate elapsed time
elapsed_time = time.time() - start_time
print(f"\nFinished!\nElapsed time: {elapsed_time:.2f} seconds")

#        https://www.youtube.com/watch?v=kmnm-uGcxzs&list=PLU9oqwAim30SX-GdwhcgIp2sddqkWMj4X
