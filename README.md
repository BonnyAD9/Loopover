# Loopover
Remake of Carkh's loopover for console in C# .NET Core

Application saves statistics inside a json file<br>
On widows: `%appdata%\Loopover\save.json`<br>
On linux should be: `home/.config/Loopover/save.json`

Times made while trying scramble aren't saved

## Sreenshots
Game page:
![image](https://user-images.githubusercontent.com/46282097/121363644-6d5b0d00-c937-11eb-820d-694f1c1341a3.png)

Stats page:
![image](https://user-images.githubusercontent.com/46282097/121363812-8ebbf900-c937-11eb-9cba-6a5d1e19a147.png)

## Controls
### In game
- [Enter] Scramble
- [Arrows] Move around
- [Ctrl + Arrows] Rotate
- [Backspace] Pause/Resume (mainly for resizing the console window)
- [R] Reload visuals
- [S] Save statistics
- [Tab] Show statistics
- [Esc] Save and exit
### In statistics
- [Up|Down Arrow] Next|Previus time
- [Pg Up|Down] Move by one page at a time
- [Home|End] Go to top|End
- [Enter] Try scramble
- [+|-] Next|Previous move
- [Spacebar] Reset moves
- [R] Reload visuals
- [S] Save
- [Tab] Back to game
- [Esc] Save and exit
### While trying scramble
Same as in game
- [Esc|Tab] Back

## Color scheme
Recommended color scheme for Windows Terminal:
```
{
    "background": "#0C0C0C",
    "black": "#0C0C0C",
    "blue": "#0037DA",
    "brightBlack": "#767676",
    "brightBlue": "#3B78FF",
    "brightCyan": "#2A7081",
    "brightGreen": "#3DD435",
    "brightPurple": "#B4009E",
    "brightRed": "#E4771E",
    "brightWhite": "#F2F2F2",
    "brightYellow": "#F9F1A5",
    "cursorColor": "#FFFFFF",
    "cyan": "#49C7C7",
    "foreground": "#CCCCCC",
    "green": "#13A10E",
    "name": "Loopover",
    "purple": "#881798",
    "red": "#C50F1F",
    "selectionBackground": "#FFFFFF",
    "white": "#CCCCCC",
    "yellow": "#E7BA04"
},
```
The defult colors should be usable as well.<br>
I used `Consolas` font in my terminal, but any monospace font where character size ratio is `width : height = 1 : 2` should be fine.

## Inspiration
[Carykh's loopover](https://openprocessing.org/sketch/580366)<br>
[Loopover by Janis Pritzkau](https://loopover.xyz)
