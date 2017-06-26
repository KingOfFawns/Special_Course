using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class StartUp_Manager : MonoBehaviour {

	public TextAsset textFile;

	// Use this for initialization
	void Start () {
		// Check if storage file exists
		if (!File.Exists (Application.persistentDataPath + "/saveData.csv")) {
			string header = "Name;Time;Target words;Identified words;Falsely identified words;Time used in seconds;False negatives;" +
				"True positives;False positives;Length of sequences;Number of sequences;Words displayed;Grids showed;" +
				"Correct matches; N; Images shown";
			File.AppendAllText (Application.persistentDataPath + "/saveData.csv", header + Environment.NewLine);
		}
			
		// Load local stored data
		AppControl.control.Load ();
		AppControl.control.Save (); // This creates file if not existing

		// Load textFile with words for word_recog tests
		string text = textFile.text;
		string[] words = text.Split ("\n" [0]);
		AppControl.control.words = words;
	}
}
