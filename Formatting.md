data.txt formating

On the first row are the channels and colors. It can look something like this:
`3,3,55,72,44,40` "channel in","channel out","pitch","velocity","pitch","velocity"...
This is used to send output, this often controls the lightning on your midi device.

The actual keys are configured as follows.
For a list of all available keys see `Symbols.txt`
Single key action:

midi_key_down_id
  \/
8323216,0,65 <- key_action   //this would output "a"
	/\
 key_indedification

Alternativity you could type the following for the same result:
8323216, 0, 'a'
8323216, 0, " "
8323216, 0, a


Macro actions:

midi_key_down_id
  \/
8323216,1,65,-65,160,66,-66,-160,67,-67,68,-68 <- key_actions   //in this case "aBcd"
        /\
  macro_indedification

A macro is identified by having the midi_key_up_id set to 0.
A positive ID is key_down and a negative is key_up.


Single key action with 'F24' as addon key (useful when using this program together with AutoHotkey):

midi_key_down_id
  \/
8323216,2,66 <- key_action   //in this case 'b'
	/\
 key_indedification