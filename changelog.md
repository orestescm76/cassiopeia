# Cassiopeia Changelog
#### 2.0.246.0
- Added a button to share lyrics on Twitter

#### 2.0.245.10
- Fixed CD saving [FIX]
- Fixed Vinyl addition [FIX]
- Clicking a song in EditAlbum opened a File Explorer [FIX]
- Tweaked the saving function. [FIX]
- Fixed the Spotify sync in Player [FIX]
- Bugfixing the ViewAlbum with Vinyl [FIX]

#### 2.0.244.10
- Now we're able to calculate the disk ID (CBBD1)
- Some fixes in the Player [FIX]
- Fixed CreateCD form, which wasn't closing [FIX]
- Now the viewer reports the CD duration. [FIX]

#### 2.0.242.20
- Search lyrics now implemented, very basic.
- Added a keyboard shortcut to open LyricsViewer (Ctrl + L)
- Corrected the save mark. [FIX]
- Icons were visible when user exited out of EditForm. That could crash the program. [FIX]

#### 2.0.241.0 - DEBUG RELEASE
- Gweedo Weedo Really Fast Startup [FIX]

#### 2.0.240.50
- Changed the main datastructure to a Dictionary. It should be faster.
- Fixed slow start up [FIX]
- Fix on the Lyrics Viewer when changing the font size. [FIX]
- Made update check async [FIX]
- Fixed saving text not showing in PlaylistUI [FIX]
- Fixed memory leak when selecting many albums [FIX]
- Catched exception when not logging to Spotify correctly [FIX]
- Fix editing album breaks the program [FIX]
- Did i made the Spotify login async? [FIX]

#### 2.0.239.40
- Cassiopeia autodetects the system language in order to set itself.
- Fixed context menu when not selecting any album [FIX]
- Fixed context menu when view is on CD or Vinyl mode [FIX]
- Tweaked the CreateAlbum form [FIX]
- Some fixes in label location in CreateSong form [FIX]
- Removed localization if album doesn't have a location folder [FIX]
- Fixed lyrics icon not removing when dropping the selection [FIX]
- Fixed creating new album from a directory [FIX]
- Fixed the location of the sound folder in EditAlbum [FIX]
- Fixed the linking of sound files and album [FIX]
- Fixed closing Spotify on the Player [FIX]

#### 2.0.238.0
- Added 3 new icons. Add CD, Add vinyl and a search icon to illustrate the textbox.
- Fixed hiding / showing the sidebar when window is maximized. [FIX]

#### 2.0.237.20
- Ported to NET 6 because it is faster and LTS.
- Added a save confirmation when closing and having unmodified changes.
- Fixed crash when using the keyboard in ConfigForm. [FIX]
- Fixed viewing of LongSongs [FIX]
- Fixed opening multiple About forms [FIX]

#### 2.0.236.80 - RELEASE
- Added support for Vinyl albums
- Fixed duplicated results in the real time filters. [FIX]
- Now cover paths are saved with relative path, saves about 14% of space.
- Now program writes to log if languages folder is not found
- Fixed album cover was not saved correctly if downloading from Spotify [FIX]
- Fixed creating albums and viewing albums [FIX]
- Added localization
- Temporarily removed the selected duration because it needs to be reimplemented.
- Fixed adding an album with Spotify if it doesn't have images. [FIX]

#### 2.0.234.0
- Realtime filters
- Ctrl+A and Esc (selecting albums) no longer blocks the program. [FIX]
- However, if you click away, it hangs, need to check the event.
- If you click "Edit album" from the toolbar, when you close, no longer shows the view album. [FIX]
- why am i doing this on December 24th

#### 2.0.233.0
- Textbox in the toolbar, real time filters

#### 2.0.232.0
- Adjusted LyricViewer buttons.

#### 2.0.231.0
- Draft for filters.
- Adding an album from a folder with local files caused to not being able to visualise it [FIX]
- Roman to arabic function changed to allow bigger numbers. [ISSUE #15]

#### 2.0.230.0
- Added a ToolBox with easy access to common functions.
- Added translations to ConfigForm
- Now you know if you made any changes to the database.
#### 2.0.225.30
- Reworked the login to Spotify, using PKCE
- App now starts faster, no more browser open
- Now metadata streams saves the historial, if enabled
- Deleting a lot of albums doesn't take a century [FIX]
- However, seleting all albums and deselecting takes a lot [KNOWN ISSUE]

#### 2.0.224.0
- Added a "low" latency mode and a "high" latency when syncing Spotify if the Player is visible to minimize CPU and network usage. (and API calls.)
- Reworked the metadata stream mode.

#### 2.0.222.20
- Now the program saves the size and sidebar view seting.
- Added the option to customize the string stream for sending the now playing song to a file. It's useful for streamers.
- Now when you resize the config form, the internal controls move according to the resize.

#### 2.0.220.10 - DEBUG RELEASE
- Added a sidebar in the main view.
- Contains the album cover, total albums, duration and some stats for selected album.
- Config form is resizable and smaller.
- Namespace refactor
- Changed data structure to a linked list (tf?)
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


#### 1.6.0.82 
- PlaylistUI - Fix for local songs and time not adding.
- PlaylistUI - Fixed the routine for deleting songs.
- PlaylistUI - Del key now deletes songs.
- PlaylistUI - Fixed a small issue when songs over 60 minutes long weren't showing correctly.
- Player - Same as above.
- Small Fixes
- Added Delunado in the credits.

#### 1.6.0.79
- Main:
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

#### 1.5.2.87
- arreglado el borrado (gracias Jaime!)
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
