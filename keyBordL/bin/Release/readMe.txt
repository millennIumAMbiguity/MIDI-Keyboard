data.txt fromating

on the first row is the chanel. after that is all of the combinations that you have made.


single key down and up action:

midi_key_down_id
  \/
8323216,144,9 <- key_action   //in this case 'tab'
	 /\
   midi_key_up_id




single key non-binary action:

midi_key_down_id
  \/
8323216,-2,65 <- key_action   //in this case 'a'
	/\
 non-binary_indedification




macro actions:

midi_key_down_id
  \/
8323216,-1,65,-65,160,66,-66,-160,67,-67,68,-68 <- key_actions   //in this case 'aBcd'
        /\
   macro_indedification

a macro is indedified by having the midi_key_up_id set to 0.
bihinf the ids ther is a list of key_action(s) posetiv is key_down and negativ is key_up