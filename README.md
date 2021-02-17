# Music manager
[![Tam](https://img.shields.io/github/languages/code-size/orestescm76/aplicacion-gestormusica?label=Size)](https://github.com/orestescm76/aplicacion-gestormusica)
[![Ultima version](https://img.shields.io/github/v/release/orestescm76/aplicacion-gestormusica?color=red)](https://github.com/orestescm76/aplicacion-gestormusica)

![Screenshot](https://i.imgur.com/jeawWRv.png)

## What is it for?
It's a desktop application for managing a music collection, such as CDs or vinyl records in a quick and simple way. First you store the metadata however you like.
It has a music player built in, can play in FLAC, OGG and MP3. It can also sync with your Spotify. If you have Premium you can use Spotify from the app.
Doesn't require any installation, can be carried on a USB flash drive.

### Features
* Save your albums for later use.
* Capable of having many databases.
* Linking to your Spotify account.
* Add albums from Spotify.
* Link your local songs to albums in the database.
* Playing your saved albums in Spotify or in the music player.
* Manage CDs, with Goldmine standard for the degradation of the media. Similar to Discogs.

### Launch options
* -reproductor / -player -> Launches the program with only the music player
* -modoStream -> Launches the program in a background mode, only writes the playing song in Spotify in a file called "np.txt". It's great for streamers.
* -consola / -conole -> Launches a console with the program.
* -noSpotify -> Disables everything related to Spotify.

#### This software has many libraries:
* https://github.com/JohnnyCrazy/SpotifyAPI-NET
* https://github.com/filoe/cscore
* https://www.the-roberts-family.net/metadata/index.html
* https://github.com/JamesNK/Newtonsoft.Json
#### Licences:
* https://github.com/JohnnyCrazy/SpotifyAPI-NET/blob/master/LICENSE
* https://github.com/filoe/cscore/blob/master/license.md
* https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md

### Currently working on:
* Playlist system reworked, UI from scratch
* Greek translation
* Completing the config window.

### Stuff to do:
* Improvements in the UI
* Support for vinyl records
* Any good suggestion I may recieve
* Refactoring code
