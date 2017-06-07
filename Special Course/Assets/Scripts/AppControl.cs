using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AppControl : MonoBehaviour {

	public static AppControl control;

	public int DigitSpan_DigitLength = 3;

	// Use this for initialization
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
		data.DigitSpan_DigitLength = DigitSpan_DigitLength;

		// Store data
		bf.Serialize (file, data);
		file.Close();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/appData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/appData-dat", FileMode.Open);
			AppData data = (AppData)bf.Deserialize (file);
			file.Close ();

			// Load data
			DigitSpan_DigitLength = data.DigitSpan_DigitLength;
		}
	}
}

[Serializable]
class AppData {
	public int DigitSpan_DigitLength;
}
