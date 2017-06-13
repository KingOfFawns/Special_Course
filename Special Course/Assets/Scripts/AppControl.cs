using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AppControl : MonoBehaviour {

	public static AppControl control;

	// Storage for word recog start and end
	public int word_Recog_Target = 10;
	public float word_Last_Test = 0f;
	public string[] words;
	public string[] chosenWords;
	public int identifiedWords = 0;
	public int falseWords = 0;
	public bool success = false;

	// Storage for N-Back
	public int N = 2;
	public float N_percentage_last = 0f;

	// Storage for Digit Span
	public int digitSpan_DigitLength = 3;

	// Boolean for a first time start up settings screen
	public bool first_Time_Start = true;

	// String used to store logged data
	public string dataString = "";

	// Variables to control time settings
	public float wordRecogStart_WordTimer = 1f;
	public float digitSpan_SequenceTimer = 1f;



	void Awake () {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		} 
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/appData.dat");

		// Data to store
		AppData data = new AppData();

		data.word_Recog_Target = word_Recog_Target;
		data.word_Last_Test = word_Last_Test;

		data.N = N;
		data.N_percentage_last = N_percentage_last;

		data.digitSpan_DigitLength = digitSpan_DigitLength;

		// Store data
		bf.Serialize (file, data);
		file.Close();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/appData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/appData.dat", FileMode.Open);
			AppData data = (AppData)bf.Deserialize (file);
			file.Close ();

			// Load data
			word_Recog_Target = data.word_Recog_Target;
			word_Last_Test = data.word_Last_Test;

			N = data.N;
			N_percentage_last = data.N_percentage_last;

			digitSpan_DigitLength = data.digitSpan_DigitLength;
		}
	}

	public void SaveData(){
		File.AppendAllText (Application.persistentDataPath + "/saveData.txt", dataString + Environment.NewLine);
	}

	public void SendData(){

	}

	public void ClearData(){
		File.Delete (Application.persistentDataPath + "/saveData.txt");
	}
}

[Serializable]
class AppData {
	public int word_Recog_Target = 10;
	public float word_Last_Test = 0f;

	public int N = 2;
	public float N_percentage_last = 0f;

	public int digitSpan_DigitLength;
}
