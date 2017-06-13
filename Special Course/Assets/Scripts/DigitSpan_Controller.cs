﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DigitSpan_Controller : MonoBehaviour {

	public Text digitText;
	public Image check;
	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text sequenceLengthShow;
	public Text shownText;
	public Text correctText;

	private int sequenceLength = 3;

	private string digitNumber = "";
	private string inputNumber = "";
	private bool active = false;

	private int correctSequences = 0;
	private int numOfSequences = 0;


	// Use this for initialization
	void Start () {
		// Get digit length from storage
		sequenceLength = AppControl.control.digitSpan_DigitLength;

		// Set sequenceLength in startCanvas
		sequenceLengthShow.text = sequenceLength.ToString();
	}

	public void StartButton(){
		// Stop start canvas
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine(NextNumber ());

		StartCoroutine(Timer());
	}

	IEnumerator Timer(){
		// The timer waits for 60 seconds
		yield return new WaitForSeconds (60f);

		// Activate canvas representiong the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		shownText.text = numOfSequences.ToString();
		correctText.text = correctSequences.ToString ();

		// Special data setting
		string sLength = sequenceLength.ToString ();

		// Calculate data to be stored
		float percentage = correctSequences/numOfSequences * 100;
		if (percentage >= 90) {
			sequenceLength++;
		} else if(percentage < 60) {
			sequenceLength--;
		}

		// Limit sequence length
		if (sequenceLength < 2) {
			sequenceLength = 2;
		} else if (sequenceLength > 15) {
			sequenceLength = 15;
		}

		// Store local data
		AppControl.control.digitSpan_DigitLength = sequenceLength;
		AppControl.control.Save ();

		// Wait for 3 seconds before transitioning to the next test
		yield return new WaitForSeconds (3f);

		// Data to be stored
		string name = "Digit Span";
		string time = System.DateTime.Now.ToString();
		string totalSequences = numOfSequences.ToString ();
		string cSequences = correctSequences.ToString ();

		// Store data
		AppControl.control.dataString = "Name: " + name + ", Time: " + time + ", Length of Sequences: " + sLength +
			", Num. of Sequences: " + totalSequences + ", Correct Matches: " + cSequences + "\n"; 
		AppControl.control.SaveData ();

		// Go to next test
		SceneManager.LoadScene("MainMenu");
	}

	public void InputDigit(string digit){
		if (!active) {

			digitText.text = digit;

			inputNumber = inputNumber + digit;

			if (inputNumber.Length == digitNumber.Length) {

				if (inputNumber == digitNumber) {
					check.sprite = Resources.Load<Sprite>("checkmark");
					correctSequences++;
				} else {
					check.sprite = Resources.Load<Sprite>("failure");
				}

				StartCoroutine (ResetCheck ());

				inputNumber = "";
				StartCoroutine(NextNumber ());
			}
		}
	}

	IEnumerator ResetCheck(){
		// Wait 0.3 seconds, then remove checkmark/X
		yield return new WaitForSeconds (0.3f);
		check.sprite = Resources.Load<Sprite> ("None"); 
	}

	IEnumerator NextNumber(){

		yield return new WaitForSeconds (0.3f);

		UpdateNumber ();
	}

	void UpdateNumber(){
		numOfSequences++;

		// Calculate number
		digitNumber = "";

		for (int i = 0; i < sequenceLength; i++) {
			int ran = Random.Range (1, 10);

			digitNumber = digitNumber + ran.ToString ();
		}

		Debug.Log (digitNumber);

		// Start showing digits
		StartCoroutine (ShowDigits (digitNumber));

	}

	IEnumerator ShowDigits(string number){
		active = true;

		int length = number.Length;

		// Cut string in chars and show in text field
		for (int i = 0; i < length; i++) {
			char digit = number [i];

			digitText.text = digit.ToString();

			yield return new WaitForSeconds (1);

			digitText.text = "";

			if (i != length - 1) {
				yield return new WaitForSeconds (0.3f);
			}
		}

		active = false;
	}
}
