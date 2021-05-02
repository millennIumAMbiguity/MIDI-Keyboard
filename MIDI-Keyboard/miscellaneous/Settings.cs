using System;
using System.Collections.Generic;
using System.IO;

namespace MIDIKeyboard.Miscellaneous
{
	public class Data
	{
		public bool   force_load_data_on_run { get; set; } = false;
		public string key_data_path          { get; set; } = "data.txt";

		public string visual_profiles_path { get; set; } = @"visualProfiles\";
		//public bool save_all { get; set; } = false;
	}

	public class Settings
	{
		private static readonly string path = "settings.txt";

		public static Data data = new Data();

		public static bool Read()
		{
			if (!File.Exists(path))
				return false;

			string[] file = File.ReadAllLines(path);
			foreach (string row in file)
				if (row.Split(':').Length > 1) {
					string value = row.Split(':')[1].Trim(' ', '\n', '\r'); //clenup
					string name  = row.Split(':')[0].Trim(' ', '\n', '\r');

					var valueProperties = data.GetType().GetProperties(); //get data variables 
					for (int i = 0; i < valueProperties.Length; i++)
						if (name == valueProperties[i].Name) {
							Type t = valueProperties[i].GetType();
							if (valueProperties[i].GetMethod.ReturnType.Equals(typeof(string)))
								valueProperties[i].SetValue(data, value); //save data to variable as string
							else if (valueProperties[i].GetMethod.ReturnType.Equals(typeof(float)))
								valueProperties[i].SetValue(data, float.Parse(value)); //save data to variable
							else if (valueProperties[i].GetMethod.ReturnType.Equals(typeof(int)))
								valueProperties[i].SetValue(data, int.Parse(value)); //save data to variable
							else if (valueProperties[i].GetMethod.ReturnType.Equals(typeof(bool)))
								valueProperties[i].SetValue(data, bool.Parse(value)); //save data to variable
							else
								throw new NotImplementedException();
						}
				}

			return true;
		}

		public static void Save()
		{
			//Data defaultData = new Data();
			var createText = new List<string>();

			var valueProperties = data.GetType().GetProperties(); //get data values
			//PropertyInfo[] defaultValueProperties = defaultData.GetType().GetProperties(); //get default data values
			for
			(int i = 0;
			 i < valueProperties.Length;
			 i++) //if (data.save_all || defaultValueProperties[i].GetValue(defaultData, null).ToString() != valueProperties[i].GetValue(data, null).ToString()) //check if data value differs from default data value
				createText.Add(
					valueProperties[i].Name + ':' +
					valueProperties[i].GetValue(data, null)); //saves data value to string list

			File.WriteAllLines(path, createText); //saves data in string list to file
		}
	}
}