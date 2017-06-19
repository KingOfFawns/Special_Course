using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUp_Manager : MonoBehaviour {

	public TextAsset textFile;


	// Use this for initialization
	void Start () {
		// Load local stored data
		AppControl.control.Load ();
		AppControl.control.Save (); // This creates file if not existing

		// Load textFile with words for word_recog tests
		string text = textFile.text;
		string[] words = text.Split ("\n" [0]);
		AppControl.control.words = words;

		// Start new Random notification if app is launched from random notification
				
	}
}
