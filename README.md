# MIDI Keybord

Use launchpads, MIDI electronic keyboards, and other MIDI devices as computer keyboards.<br>
You can maps keys either by one of the setups processes or by typing directly in the file `data.txt`.

---

### Setup
the avable setups funtaions are `fast setup` and `setup.`<br>
`fast setup` will automatically assign a keyboard key to any MIDI key it detects on the target MIDI device.<br>
`setup.` will allow the user to assign MIDI keys of the target MIDI device to a keyboard key. The limitation of this is that some keys are not assignable in this process. Some of these keys include `escape`, `F1`, and `home`.
#### Manual setup
On the first row if the file is port id and output. Output is usually used to set colors of LEDs in MIDI devices as launchpads. 
Only the input port id is reqired.<br>
On the second row, a format can be spesified. If no format was detected, it will asume everything after the first row as no format. A format can be spesified with the filename of a file in `visualProfiles`. `-input` is used to specify a profile as an input profile and will hide it from the output profile list.<br>
For more details about formating, see `Formatting.txt`. For details about avable keys, see `Symbols.txt`<br>
<b>Here is a example on a `data.txt` using a format</b>
```
0
launchpad(pro)-Input.txt
   ,'1','2','3','4','7','8','9','6',
   ,220,   ,   ,   ,   ,   ,   , 8 ,
   ,   , q , w , e ,   ,   ,   ,   ,
   ,   , a , s , d ,   ,   ,   ,   ,
   ,   ,   ,   ,   ,   ,   ,   ,   ,
   ,   ,   ,   , 13,' ',   ,   ,   ,
   ,   ,   ,   ,   ,   ,   ,   ,   ,
   ,   ,   ,   ,   ,   ,   ,   ,   ,
   ,   ,   ,   ,   ,   ,   ,   ,   ,
   ,   ,   ,   ,   ,   ,   ,   ,   ,
```
<b>Here is a example on a normal `data.txt`</b>
```
0
16016, a
18576,0,81
18832,0,87
16272,0,83
16528,0,68
19088,0,69
16784,0,37
19600,0,38
17040,0,40
17296,0,39
20880,0,167
21392,0,84
21136,0,104
```

---

### Output
When running the output, the program will record your commands and if you want, save them to an existing `data.txt` file.<br>
The `viewer output visual` will allow you to more easily send outputs by displaying a view on the keys and their index.<br>
For more details about formating, see `Formatting.txt`.
