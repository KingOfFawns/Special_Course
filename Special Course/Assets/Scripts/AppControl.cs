using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AppControl : MonoBehaviour {

	public static AppControl control;

	// Storage for word recog start and end
	public int word_Recog_Target = 10; // Should be stored
	public float word_Last_Test = 0f; // Should be stored
	public string[] words;
	public string[] chosenWords;
	public int identifiedWords = 0;
	public int falseWords = 0;
	public bool success = false;

	// Storage for N-Back
	public int N = 2; // Should be stored
	public float N_percentage_last = 0f; // Should be stored

	// Storage for Digit Span
	public int digitSpan_DigitLength = 3; // Should be stored

	// Boolean for a first time start up settings screen
	public bool first_Time_Start = true; // Should be stored

	// String used to store logged data
	public string dataString = "";

	// Variables to control time settings
	public float wordRecogStart_WordTimer = 1.0f; // Should be stored
	public float digitSpan_SequenceTimer = 1.0f; // Should be stored
	public float NBack_Timer = 1.0f;


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

		// Word Recog data
		data.word_Recog_Target = word_Recog_Target;
		data.word_Last_Test = word_Last_Test;

		// N-back data
		data.N = N;
		data.N_percentage_last = N_percentage_last;

		// Digit Span data
		data.digitSpan_DigitLength = digitSpan_DigitLength;

		// Timer variables for settings - data
		data.wordRecogStart_WordTimer = wordRecogStart_WordTimer;
		data.digitSpan_SequenceTimer = digitSpan_SequenceTimer;
		data.NBack_Timer = NBack_Timer;

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

			wordRecogStart_WordTimer = data.wordRecogStart_WordTimer;
			digitSpan_SequenceTimer = data.digitSpan_SequenceTimer;
			NBack_Timer = data.NBack_Timer;
		}
	}

	public void SaveData(){
		File.AppendAllText (Application.persistentDataPath + "/saveData.txt", dataString + Environment.NewLine);
	}

	public void SendData(){

	}

	public void ClearData(){
		File.Delete (Application.persistentDataPath + "/saveData.txt");
		File.Delete (Application.persistentDataPath + "/appData.dat");
	}
}

[Serializable]
class AppData {
	public int word_Recog_Target = 10;
	public float word_Last_Test = 0f;

	public int N = 2;
	public float N_percentage_last = 0f;

	public int digitSpan_DigitLength = 3;

	public float wordRecogStart_WordTimer = 1.0f;
	public float digitSpan_SequenceTimer = 1.0f;
	public float NBack_Timer = 1.0f;
}
