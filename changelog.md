# Cassiopeia Changelog
#### 2.0.222.0
- Now the program saves the size and sidebar view seting.
- Added the option to customize the string stream for sending the now playing song to a file. It's useful for streamers.
#### 2.0.220.0 
- Added a sidebar in the main view.
- Contains the album cover, total albums, duration and some stats for selected album.
- Config form is resizable and smaller.
- Namespace refactor
- Changed data structure to a linked list
- Fixed the manual add album
- Proof of concept for a future search function.

#### 2.0.216.10
- Added the ability to import a Spotify album collection. It's quite slow.
- If an album has a semicolon in the title, it's removed
- Moved the Spotify options to another drop down list
- Fixed deletion for good!

#### 2.0.215.0 
- History. Have a text file where you can store your listening history.
- Fix for playing mark in playlist ui. (still something is not fixed)
- Added Ctrl C to console. Now console doesn't ask you to press enter to exit.

#### 2.0.214.60 
- Finally fixed the Spotify token refresh!
- Some refactoring on Spotify API code...

#### 2.0.212.20 
- Clipboard config now has a little help so you know what to type.
- Now an album can have many types, EP, Live, so you can have a detailed collection. (filters!)
- Old databases migrate without problems.
- Reduced waiting time for linking Spotify from 30 seconds to 20.
- Added Bitcoin address in about page.

#### 2.0.210.70 
- Create CD reimplemented. New logic and support for any number of CDs.
- Now refreshing the token works.
- Main form is fully resizable and the status bar does not hide the list.

#### 2.0.203.20 
- Main form is fully resizable. (WIP)

#### 2.0.202.0  
- Tweaked the method to download covers. Now it's asynchrnonus

#### 2.0.199.0  
- More work to port the API, this time on the Player. Readonly but sync works.
- Fixed bug where album cover keeps downloading 100% of the time, this had an effect on CPU and network usage.

#### 2.0.196.0 
- Small port to 6.0 Spotify API. Search and playing albums work.

#### 2.0.190.0  
- Console returns.
- [FIX] When linked, after starting the app was out of focus.
- Button should look correctly on Spotfy sync mode.
- Twit button on the player is fixed. (Although there is a known issue with songs containing ')
- Console makes a return. (Windows only)
- Tweaked the startup, now I can release working single .exe files.
- [FIX] Linking with Spotify option was not showing.
- Tweaked the startup, now I can release working single .exe files.
- Fix in Player, where no longer crashes on drag & dropping wrong file extensions.
- Additions to log, translating to English. (Forza Italia!)
- Reworked the refesh token task.
- Tweaked the application exit.
- Reworked the application launch. New Kernel class to manage the core of the application.
- Ported to NET 5.


1.6.0.82 - PlaylistUI - Fix for local songs and time not adding.
		 - PlaylistUI - Fixed the routine for deleting songs.
		 - PlaylistUI - Del key now deletes songs.
		 - PlaylistUI - Fixed a small issue when songs over 60 minutes long weren't showing correctly.
         - Player - Same as above.
		 - Small Fixes
		 - Added Delunado in the credits.

1.6.0.79 - Main:
    -Greek and Italian translations (Thanks Lorenzo!)
    -Main window can be resized.
    -New window, Lyrics Viewer. You can manage lyrics from the songs you have stored.
    -You can view the log in the program.
    -A new window for configuring the program.
    -Adding a new album from a folder.
    -Reworked the Spotify search.
    -Now you can see whether you're viewing the Digital or CD collection

		- Player:
    -Support for CD Audio
    -New playlist system from scratch
    -A lot of bugfixes and improvements.

		- Misc:
    -English parameters.

		- Known issues.
    -The Player can't play certain OGG files. This is from a bug in the library.
    -Twit button on the Player is broken.
    -CD creation can be a little janky.
    -Missing information in the clipboard config

1.5.2.87 - arreglado el borrado (gracias Jaime!)
         - perfilado el formulario de editar álbum.
		 - arreglado el bug del reproductor y el slider de la posicion
		 - arreglado el bug de la salida doble
		 - arreglado el indexado de las canciones
		 - shortkeys para el volumen. ctrl + arriba o + para subir. ctrl + abajo o - para bajar
         - arreglados los atajos de teclado para saltar cancion
         - quitado el foobar, era un experimento
         - retoques en el reproductor, debería ser mucho más estable.
         - arreglado el error de que el reproductor se salía y guardaba 2 veces
		 - arreglado status bar spotify fix
         - si apagas spotify se pausa. fix
         - titulo cuando vuelves a local y duraciones reiniciadas fix
         - mejoras en el log (duh)
         - refresco cuando editas un álbum fix
         - cuando reproduces una cancion larga en Spotify se te agregan todas las partes a la cola fix
         - arreglado el "falso" bloqueo de los discos al intentar borrarlos fix
         - arreglado el crash al twittear una canción local sin registrar en la base de datos fix
         - cuando apagas el modo spotify, se deshabilita el botón de "twit" fix
         - cuando iniciabas en modo reproductor no se cerraba fix
		 - drag & drop de ficheros al reproductor
         - Ctrl + O para abrir ficheros (disabled)
