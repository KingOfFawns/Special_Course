using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class StartUp_Manager : MonoBehaviour {

	public TextAsset textFile;

	// Use this for initialization
	void Awake () {

		// Check if storage file exists
		if (!File.Exists (Application.persistentDataPath + "/.dat1.dat")) {
			string header = "PatientNumber;Name;Time;Target words;Identified words;Falsely identified words;Time used in seconds;False negatives;" +
				"True positives;False positives;Length of sequences;Number of sequences;Words displayed;Grids showed;" +
				"Correct matches; N; Images shown";
			File.AppendAllText (Application.persistentDataPath + "/.dat1.dat", header + Environment.NewLine);
			File.SetAttributes (Application.persistentDataPath + "/.dat1.dat", FileAttributes.Hidden);
		}

		// Load textFile with words for word_recog tests
		string text = textFile.text;
		string[] words = text.Split ("\n" [0]);
		AppControl.control.words = words;
	}
}
