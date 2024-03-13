from pytube import Playlist, exceptions
from PIL import Image
import os
import lyricsgenius
import requests
import time
import re
import pylast

words_to_replace = [" (Official Audio)", " (Audio)", " [Audio]", " (Official Lyric Video)", " (Unreleased)",
                    " (Official Video)", " (Official Music Video)", " [Official Music Video]",
                    " (Official Net Video)", " (Official HD Video)", " (Audio)", " (Visualizer)", " (Official)",
                    " (Visualizer)", " (Official Visualizer)", " [Official Visualizer]", " (Lyrics)",
                    " (Music Video)", " (Lyric Video)", " [Official Audio]", " [Official Video]",
                    " (Video Official)", " (Original Song)", " (HQ)", " [Official HD Audio]",
                    " (Official visualizer)", " (Video oficial)", "Feat", "Feat.", "feat", "feat.", " ft.", "?",
                    "\'", "\"", "(", ")", ":", "\\", "/", "[Legendado]", "[Directed by Cole Bennett]"]

api_key = "XAhf6ZzYrgiQx4j3DovglzBVz5frEurz6y86RcoHzhvmx5XGJ9JF6O9NpomHmaFg"
last_fm_api_key = "4fdf3c66636a44cea52bada67b7efd00"
last_fm_api_secret = "4192e8996c534794d6c5187c802eef8d"
genius = lyricsgenius.Genius(api_key, timeout=10)

# Initialize Last.fm API
network = pylast.LastFMNetwork(api_key=last_fm_api_key, api_secret=last_fm_api_secret)

# Bad words regex pattern - https://github.com/mogade/badwords/blob/master/en.txt
bad_words_regex = r'''^[a@][s\$][s\$]$
[a@][s\$][s\$]h[o0][l1][e3][s\$]?
b[a@][s\$][t\+][a@]rd 
b[e3][a@][s\$][t\+][i1][a@]?[l1]([i1][t\+]y)?
b[e3][a@][s\$][t\+][i1][l1][i1][t\+]y
b[e3][a@][s\$][t\+][i1][l1][i1][t\+]y
b[e3][a@][s\$][t\+][i1][a@][l1]([i1][t\+]y)?
b[i1][t\+]ch[s\$]?
b[i1][t\+]ch[e3]r[s\$]?
b[i1][t\+]ch[e3][s\$]
b[i1][t\+]ch[i1]ng?
b[l1][o0]wj[o0]b[s\$]?
c[l1][i1][t\+]
^(c|k|ck|q)[o0](c|k|ck|q)[s\$]?$
(c|k|ck|q)[o0](c|k|ck|q)[s\$]u
(c|k|ck|q)[o0](c|k|ck|q)[s\$]u(c|k|ck|q)[e3]d 
(c|k|ck|q)[o0](c|k|ck|q)[s\$]u(c|k|ck|q)[e3]r
(c|k|ck|q)[o0](c|k|ck|q)[s\$]u(c|k|ck|q)[i1]ng
(c|k|ck|q)[o0](c|k|ck|q)[s\$]u(c|k|ck|q)[s\$]
^cum[s\$]?$
cumm??[e3]r
cumm?[i1]ngcock
(c|k|ck|q)um[s\$]h[o0][t\+]
(c|k|ck|q)un[i1][l1][i1]ngu[s\$]
(c|k|ck|q)un[i1][l1][l1][i1]ngu[s\$]
(c|k|ck|q)unn[i1][l1][i1]ngu[s\$]
(c|k|ck|q)un[t\+][s\$]?
(c|k|ck|q)un[t\+][l1][i1](c|k|ck|q)
(c|k|ck|q)un[t\+][l1][i1](c|k|ck|q)[e3]r
(c|k|ck|q)un[t\+][l1][i1](c|k|ck|q)[i1]ng
cyb[e3]r(ph|f)u(c|k|ck|q)
d[a@]mn
d[i1]ck
d[i1][l1]d[o0]
d[i1][l1]d[o0][s\$]
d[i1]n(c|k|ck|q)
d[i1]n(c|k|ck|q)[s\$]
[e3]j[a@]cu[l1]
(ph|f)[a@]g[s\$]?
(ph|f)[a@]gg[i1]ng
(ph|f)[a@]gg?[o0][t\+][s\$]?
(ph|f)[a@]gg[s\$]
(ph|f)[e3][l1][l1]?[a@][t\+][i1][o0]
(ph|f)u(c|k|ck|q)
(ph|f)u(c|k|ck|q)[s\$]?
g[a@]ngb[a@]ng[s\$]?
g[a@]ngb[a@]ng[e3]d
g[a@]y
h[o0]m?m[o0]
h[o0]rny
j[a@](c|k|ck|q)\-?[o0](ph|f)(ph|f)?
j[e3]rk\-?[o0](ph|f)(ph|f)?
j[i1][s\$z][s\$z]?m?
[ck][o0]ndum[s\$]?
mast(e|ur)b(8|ait|ate)
n+[i1]+[gq]+[e3]*r+[s\$]*
[o0]rg[a@][s\$][i1]m[s\$]?
[o0]rg[a@][s\$]m[s\$]?
p[e3]nn?[i1][s\$]
p[i1][s\$][s\$]
p[i1][s\$][s\$][o0](ph|f)(ph|f) 
p[o0]rn
p[o0]rn[o0][s\$]?
p[o0]rn[o0]gr[a@]phy
pr[i1]ck[s\$]?
pu[s\$][s\$][i1][e3][s\$]
pu[s\$][s\$]y[s\$]?
[s\$][e3]x
[s\$]h[i1][t\+][s\$]?
[s\$][l1]u[t\+][s\$]?
[s\$]mu[t\+][s\$]?
[s\$]punk[s\$]?
[t\+]w[a@][t\+][s\$]?'''

# Compile the regex pattern
bad_words_pattern = re.compile(bad_words_regex, re.IGNORECASE)

def censor_bad_words(lyrics):
    # Find all matches of bad words in the lyrics
    matches = bad_words_pattern.findall(lyrics)
    
    # Replace each character of bad words with asterisks
    for word in matches:
        censored_word = '*' * len(word)
        lyrics = lyrics.replace(word, censored_word)
    
    return lyrics

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
            return [first_genre] if first_genre else [], None, None

        # Get album name
        album = track.get_album()
        album_name = album.get_title() if album else None
        
        # Get album cover image URL from Last.fm
        album_cover = album.get_cover_image() if album else None
        
        return [genres[0]], album_cover, album_name
    except Exception as e:
        print(f"Error fetching genres and album cover from Last.fm: {e}")
        return [], None, None

def download_and_process_playlist(playlist_url):
    # Load playlist
    playlist = Playlist(playlist_url)

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
                
                # Censor bad words in lyrics
                censored_lyrics = censor_bad_words(lyrics)
                
                lyrics_file_path = os.path.join(song_directory, f"{song}.txt")
                with open(lyrics_file_path, "w", encoding="utf-8") as lyrics_file:
                    lyrics_file.write(censored_lyrics)
                print(f"Lyrics saved to: {lyrics_file_path}")
            else:
                print(f"No lyrics found for {song} by {artist}")

            # Write song information to a file
            info_file_path = os.path.join(song_directory, f"{song}-information.txt")
            with open(info_file_path, "w", encoding="utf-8") as info_file:
                info_file.write(f"Title: {song}\n")
                info_file.write(f"Artist: {artist}\n")
                info_file.write(f"Release Year: {release_year}\n")
                info_file.write(f"Album Name: {album_name}\n")
                info_file.write("Genres: " + ", ".join(genres) + "\n")
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

        except exceptions.AgeRestrictedError:
            print(f"Age-restricted video: {video.title}. Skipping...")

    print("Finished!")

# Get the playlist URL from the user
playlist_url = r"https://www.youtube.com/watch?v=UJ9QIB3e2ow&list=PLU9oqwAim30S3obi9IVQdeq4nHssuSFkc"
#input("Enter your playlist URL: ")

# Start a timer
start_time = time.time()
print("Starting Timer...")

# Download and process the playlist
download_and_process_playlist(playlist_url)

# Calculate elapsed time
elapsed_time = time.time() - start_time
print(f"Finished!\nElapsed time: {elapsed_time:.2f} seconds")
