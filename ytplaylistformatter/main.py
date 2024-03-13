from pytube import Playlist
from PIL import Image
import os
import lyricsgenius
import shutil
import requests  

words_to_replace = [" (Official Audio)", " (Audio)", " [Audio]", " (Official Lyric Video)", " (Unreleased)", " (Official Video)", " (Official Music Video)", " [Official Music Video]", " (Official Net Video)", " (Official HD Video)", " (Audio)", " (Visualizer)", " (Official)", " (Visualizer)", " (Official Visualizer)", " [Official Visualizer]", " (Lyrics)", " (Music Video)", " (Lyric Video)", " [Official Audio]", " [Official Video]", " (Video Official)", " (Original Song)", " (HQ)", " [Official HD Audio]", " (Official visualizer)", " (Video oficial)", " feat ", " feat. ", " ft. ", "?", "\'"]

api_key = "XAhf6ZzYrgiQx4j3DovglzBVz5frEurz6y86RcoHzhvmx5XGJ9JF6O9NpomHmaFg"
genius = lyricsgenius.Genius(api_key, timeout=10)

def download_and_process_playlist(playlist_url):
    # Create a Playlist object
    playlist = Playlist(playlist_url)

    # Iterate through the videos in the playlist
    for video in playlist.videos:
        # Get the audio stream
        audio_stream = video.streams.filter(only_audio=True).first()

        # Rename video title 
        song = video.title
        for word in words_to_replace:
            song = song.replace(word, "")
        song = song.strip()

        # Download the audio and get the uploader name
        file_path = audio_stream.download(output_path='./', filename_prefix=song.replace(" ", "_"))
        artist = video.author

        # Create directory for the song if it doesn't exist
        song_directory = os.path.join(os.getcwd(), song)
        if not os.path.exists(song_directory):
            os.makedirs(song_directory)

        # Move downloaded audio to the song directory and rename to mp3
        new_file_path = os.path.join(song_directory, f"{song}.mp3")
        os.rename(file_path, new_file_path)

        # Search for lyrics
        try:
            song_info = genius.search_song(song, artist)
            if song_info:
                lyrics = song_info.lyrics.split("\n", 1)[-1]  
                lyrics_file_path = os.path.join(song_directory, f"{song}.txt")
                with open(lyrics_file_path, "w", encoding="utf-8") as lyrics_file:
                    lyrics_file.write(lyrics)
                print(f"Lyrics saved to: {lyrics_file_path}")
            else:
                print(f"No lyrics found for {song} by {artist}")
        except Exception as e:
            print(f"Error searching for lyrics: {e}")

        # Download the thumbnail image
        thumbnail_url = video.thumbnail_url
        thumbnail_filename = f"{song}.png"

        # Fetch the thumbnail image using requests
        response = requests.get(thumbnail_url)
        if response.status_code == 200:
            # Save the thumbnail image in the song directory
            thumbnail_path = os.path.join(song_directory, thumbnail_filename)
            with open(thumbnail_path, 'wb') as f:
                f.write(response.content)

            # Open the image using PIL
            image = Image.open(thumbnail_path)

            # Crop the image to be 500x500
            width, height = image.size
            left = (width - 500) / 2
            top = (height - 500) / 2
            right = (width + 500) / 2
            bottom = (height + 500) / 2
            cropped_image = image.crop((left, top, right, bottom))

            # Save the cropped image as PNG
            cropped_image.save(thumbnail_path)

        print(f"Song '{song}' processed and saved to '{song_directory}'")

# Define the playlist URL
playlist_url = input("Enter your playlist URL: ")

# Call the method to download and process the playlist
download_and_process_playlist(playlist_url)
print("Finished!")