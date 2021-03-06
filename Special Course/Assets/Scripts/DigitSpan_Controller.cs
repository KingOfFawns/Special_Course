﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DigitSpan_Controller : MonoBehaviour {

	public Text digitText;
	public Image check;
	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public GameObject transitionCanvas;
	public Text transitionText;
	public Text sequenceLengthShow;
	public Text shownText;
	public Text correctText;
	public Text sequeceLengthEnd;
	public Scrollbar timeBar;

	private int sequenceLength = 3;
	private int maxSequence = 0;
	private string digitNumber = "";
	private string inputNumber = "";
	private bool active = false;
	private int correctSequences = 0;
	private int numOfSequences = 0;
	private bool end = false;
	private int counterCorrects = 0;
	private int counterFails = 0;
	private int timer = 61;
	private float extraTime = 0f;


	// Use this for initialization
	void Start () {
		// Get digit length from storage
		sequenceLength = AppControl.control.digitSpan_DigitLength;

		// Start sequence as 1 lower that it ended on in the last test
		if (sequenceLength > 2) {
			sequenceLength -= 1;
		}

		// Set sequenceLength in startCanvas
		sequenceLengthShow.text = sequenceLength.ToString();
	}

	void Update(){
		// Get time and time of test start
		System.DateTime now = System.DateTime.Now;
		System.DateTime testStart = AppControl.control.testStartDate;

		// If the test started more than 10 minutes ago and the test is not in help's test mode
		if (now.Subtract (testStart).TotalSeconds >= 600 && !AppControl.control.testOfTest) {
			// Set the boolean controlling whether or not the test has been finished
			AppControl.control.testStarted = true;
			AppControl.control.Save ();

			// Log stop
			string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
			string time = System.DateTime.Now.ToString ();

			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Test udløbet" + ", Time: " + time;
			AppControl.control.csvString = patientNumber + ";Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
			AppControl.control.SaveData ();

			// Go to main menu
			SceneManager.LoadScene ("MainMenu");
		}
	}

	public void StartButton(){
		// Stop start canvas
		startCanvas.SetActive(false);

		// Activate game canvas
		canvas.SetActive (true);

		// Start Game
		StartCoroutine(NextNumber ());

		// Start Timer
		StartCoroutine(Timer());
	}

	IEnumerator Timer(){
		// The timer waits for 60 seconds (and 3 seconds more each time the sequence length is updated)
		int i = 1;
		// Run timer and update time bar
		while (i < timer) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / (float)(timer-1);
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = ((timer-1) - i).ToString ();
			i++;
		}
		end = true;

		// Activate canvas representiong the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		shownText.text = numOfSequences.ToString();
		correctText.text = correctSequences.ToString ();
		sequeceLengthEnd.text = sequenceLength.ToString ();

		// If not in help's test mode, store the updated data
		if (!AppControl.control.testOfTest) {
			// Store local data
			AppControl.control.digitSpan_DigitLength = sequenceLength;
			AppControl.control.maxSequenceLength = maxSequence;
			AppControl.control.Save ();
		}

		// Wait for 3 seconds before transitioning to the next test
		yield return new WaitForSeconds (3f);

		// If not in help's test mode
		if (!AppControl.control.testOfTest) {
			// Data to be stored
			string patientNumber = "#" + AppControl.control.patientNumber.ToString ().Substring (1);
			string name = "Digit Span";
			string time = System.DateTime.Now.ToString ();
			string sLength = sequenceLength.ToString ();
			string totalSequences = numOfSequences.ToString ();
			string cSequences = correctSequences.ToString ();

			// Store data
			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Name: " + name + ", Time: " + time + ", Length of Sequences: " + sLength +
			", Num. of Sequences: " + totalSequences + ", Correct Matches: " + cSequences; 
			AppControl.control.csvString = patientNumber + ";" + name + ";" + time + ";;;;;;;;" + sLength + ";" + totalSequences + ";;;" + cSequences + ";;";
			AppControl.control.SaveData ();
		}

		// Go to next test (or return to help if help's test mode is active)
		int ran = Random.Range (0, 2);
		if (AppControl.control.testOfTest) {
			SceneManager.LoadScene ("Help");
		}
		else if (ran == 0) {
			SceneManager.LoadScene ("Stroop_Effect");	
		} else {
			SceneManager.LoadScene ("Eriksen_Flanker");
		}
	}

	public void InputDigit(string digit){
		if (!active) {

			// Add input to sequence
			inputNumber = inputNumber + digit;

			// Show sequence
			digitText.text = inputNumber;

			// Check sequence when it is long enough
			if (inputNumber.Length == digitNumber.Length) {

				// Correct sequence inputtet
				if (inputNumber == digitNumber) {
					check.sprite = Resources.Load<Sprite>("checkmark");
					correctSequences++;
					counterCorrects++;
					counterFails = 0;
				} 
				// Wrong sequence inputtet
				else {
					check.sprite = Resources.Load<Sprite>("failure");
					counterCorrects = 0;
					counterFails++;
				}

				// Update sequence length if 3 fails or 3 corrects in a row 
				if (counterCorrects > 2) {
					counterCorrects = 0;
					sequenceLength++;
					StartCoroutine(notify (0));
				}
				if (counterFails > 2) {
					counterFails = 0;
					sequenceLength--;
					StartCoroutine(notify (1));
				}

				// Limit sequence length
				if (sequenceLength < 2) {
					sequenceLength = 2;
				} else if (sequenceLength > 15) {
					sequenceLength = 15;
				}

				// Save max sequence length
				if (maxSequence < sequenceLength) {
					maxSequence = sequenceLength;
				}

				// Reset input
				inputNumber = "";

				StartCoroutine (ResetCheck ());

				// Continue game
				StartCoroutine(NextNumber ());
			}
		}
	}

	public void Backspace(){
		// When backspace button is pressed, delete last char in the string
		if (inputNumber.Length > 0 && !active) {
			inputNumber = inputNumber.Substring (0, inputNumber.Length - 1);
			digitText.text = inputNumber;
		}
	}

	IEnumerator ResetCheck(){
		// Wait 0.3 seconds, then remove checkmark/X
		yield return new WaitForSeconds (0.3f);
		check.sprite = Resources.Load<Sprite> ("None"); 
	}

	IEnumerator NextNumber(){
		// Wait and remove shown sequence
		yield return new WaitForSeconds (0.1f);
		digitText.text = "";
		yield return new WaitForSeconds (0.4f + extraTime);

		UpdateNumber ();
	}

	void UpdateNumber(){
		// If the test is not over, update number of sequences shown
		if (!end) {
			numOfSequences++;
		}
		// Calculate number
		digitNumber = "";
		for (int i = 0; i < sequenceLength; i++) {
			int ran = Random.Range (1, 10);

			digitNumber = digitNumber + ran.ToString ();
		}
		// Start showing digits
		StartCoroutine (ShowDigits (digitNumber));

	}

	IEnumerator ShowDigits(string number){
		// Show the sequence the specified amount of time, then remove it
		active = true;
		digitText.text = number;
		yield return new WaitForSeconds ((float)AppControl.control.digitSpan_SequenceTimer);
		digitText.text = "";
		active = false;
	}

	IEnumerator notify(int adjust){
		timer += 3;

		// Show notification/transition canvas
		if (adjust == 0) {
			if (sequenceLength >= 15) {
				transitionText.text = "3 rigtige i træk.\nTalrække længde har nået max.";
			} else {
				transitionText.text = "3 rigtige i træk.\nLængden af talrækken øges.";
			}
		} else {
			if (sequenceLength <= 2) {
				transitionText.text = "3 forkerte i træk.\nPrøv igen.";
			} else {
				transitionText.text = "3 forkerte i træk.\nLængden af talrækken reduceres.";
			}
		}
			
		extraTime = 3f;
		transitionCanvas.SetActive (true);
		yield return new WaitForSeconds (3f);
		transitionCanvas.SetActive (false);
		extraTime = 0f;
	}
}
