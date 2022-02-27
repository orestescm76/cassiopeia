# Cassiopea - Music manager
[![Tam](https://img.shields.io/github/languages/code-size/orestescm76/aplicacion-gestormusica?label=Size)](https://github.com/orestescm76/aplicacion-gestormusica)
[![Ultima version](https://img.shields.io/github/v/release/orestescm76/aplicacion-gestormusica?color=red)](https://github.com/orestescm76/aplicacion-gestormusica)

![Main Screenshot](https://i.imgur.com/jeawWRv.png)
![Viewing album metadata](https://i.imgur.com/C9Pph8W.png)
![Player and Playlist](https://i.imgur.com/ncb31Bq.png)
![Lyrics viewer](https://i.imgur.com/URNazzL.png)

## What is it for?
It's a desktop application for managing a music collection, such as CDs or vinyl records in a quick and simple way. First you store the metadata however you like.
It has a music player built in, can play in FLAC, OGG and MP3. It can also sync with your Spotify. If you have Premium you can use Spotify from the app.
Doesn't require any installation, can be carried on a USB flash drive.

### Features
* Save your albums for later use.
* Create albums from your music files.
* Play a CD with the Player.
* Capable of having many databases.
* Linking to your Spotify account.
* Add albums from Spotify.
* Link and play your local songs to albums in the database. Create and manage your playlists.
* Playing your saved albums in Spotify or in the music player.
* Manage CDs, with Goldmine standard for the degradation of the media. Similar to Discogs.
* View the lyrics of the albums you've saved in the database.

### Launch options
* -reproductor / -player -> Launches the program with only the music player
* -modoStream -> Launches the program in a background mode, only writes the playing song in Spotify in a file called "np.txt". It's great for streamers.
* -consola / -conole -> Launches a console with the program.
* -noSpotify -> Disables everything related to Spotify.

#### This software has many libraries:
* https://github.com/JohnnyCrazy/SpotifyAPI-NET
* https://github.com/filoe/cscore
* https://github.com/JamesNK/Newtonsoft.Json
* https://github.com/NVorbis/NVorbis
#### Licences:
* https://github.com/JohnnyCrazy/SpotifyAPI-NET/blob/master/LICENSE
* https://github.com/filoe/cscore/blob/master/license.md
* https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md
* https://github.com/NVorbis/NVorbis/blob/master/LICENSE
* Icons made by:
	* From https://www.flaticon.com:
		* Prosymbols. (Settings)
		* Pixel perfect. (Playlist)
		* Tempo doloe. (New)
		* Freepik. (New database, Save)
		* Taufik Ramadhan. (New CD)
		* Those Icons (Vinyl record)
		* Kiranshastry (Search)
	* From https://thenounproject.com
		* Setyo Ari Wibowo. (Lyrics)
	* Microsoft (Player)
	* Spotify (Spotify)
	* @esgmonologos (Main icon)

### Currently working on:
* Greek translation
* Refreshing UI
* Completing the config window.

### Stuff to do:
* Improvements in the UI
* Any good suggestion I may recieve
* Refactoring code
