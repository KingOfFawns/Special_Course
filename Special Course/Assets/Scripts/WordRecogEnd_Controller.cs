﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordRecogEnd_Controller : MonoBehaviour {

	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text startText;

	public Button button;
	public Text wordText;
	public GameObject content;

	public Text targetWords;
	public Text foundWords;

	private int wordLength;
	private int numOFWords = 10;
	private string[] chosenWords;

	private string[] distractorWords;

	// Use this for initialization
	void Awake () {
		// Load needed data from storage
		wordLength = AppControl.control.words.Length;
		numOFWords = AppControl.control.word_Recog_Target;
		chosenWords = AppControl.control.chosenWords;

		// Array for distractor words
		distractorWords = new string[numOFWords];

		// Choose needed amount of distractor words
		int ran = 0;
		string word = "";
		for (int i = 0; i < numOFWords; i++) {
			ran = Random.Range (0, wordLength);

			word = AppControl.control.words [ran];

			// Check if word is already chosen, and if it is choose a new one
			bool check = false;
			while (!check) {
				check = true;
				foreach (string w in chosenWords) {
					if (w == word) {
						check = false;
						ran = Random.Range (0, wordLength);
						word = AppControl.control.words [ran];
						break;
					}
				}
			}

			// Store distractor word
			distractorWords [i] = word;
		}

	}

	void Start(){
		// Set text
		startText.text = numOFWords.ToString();

		// Merge target and distractor words
		string[] allWords = new string[numOFWords*2];
		chosenWords.CopyTo(allWords,0);
		distractorWords.CopyTo (allWords, numOFWords);

		Shuffle (allWords);

		for(int i = 0; i < numOFWords*2; i++) {

			wordText.text = allWords[i];
			Instantiate (button, content.transform);
		}
	}

	void Shuffle<T>(T[] arr){
		// Fisher-Yates shuffle algorithm
		for(int i = arr.Length-1; i > 0; i--) {
			int r = Random.Range (0, i);
			T tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}


	public void StartButton(){
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine (Timer ());
	}

	IEnumerator Timer(){

		int timer = 0;
		while (timer < 30 && !AppControl.control.success) {
			yield return new WaitForSeconds (1);
			timer++;
		}


		// Activate end canvas ansd set visual data
		endCanvas.SetActive (true);
		targetWords.text = numOFWords.ToString ();
		foundWords.text = AppControl.control.identifiedWords.ToString ();

		// Special data setting
		string numOfTargets = numOFWords.ToString ();

		// Calculate data
		float ratio = AppControl.control.identifiedWords / AppControl.control.falseWords;

		if (AppControl.control.word_Last_Test >= 6 && ratio >= 6) {
			numOFWords++;
		} else if(AppControl.control.word_Last_Test < 4 && ratio < 4){
			numOFWords--;
		}
			
		//Store local data
		AppControl.control.word_Recog_Target = numOFWords;
		AppControl.control.word_Last_Test = ratio;
		AppControl.control.Save ();

		yield return new WaitForSeconds (3);

		// Data to be stored
		// Data to be stored
		string name = "Word Recognition";
		string time = System.DateTime.Now.ToString();

		string numOfIndentified = AppControl.control.identifiedWords.ToString ();
		string numOfFalse = AppControl.control.falseWords.ToString ();
		string timerTime = timer.ToString ();


		// Store data
		AppControl.control.dataString = "Name: " + name + ", Time: " + time + 
			", Target words: " + numOfTargets + ", Identified words: " + numOfIndentified + ", Falsely identified words: " 
			+ numOfFalse + ", Time used: " + timerTime + " seconds" + "\n"; 
		AppControl.control.SaveData ();

		// test end
		SceneManager.LoadScene ("MainMenu");
	}
}