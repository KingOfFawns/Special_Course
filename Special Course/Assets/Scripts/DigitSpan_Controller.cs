using System.Collections;
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
	public Text notifyText;

	private int sequenceLength = 3;
	private string digitNumber = "";
	private string inputNumber = "";
	private bool active = false;
	private int correctSequences = 0;
	private int numOfSequences = 0;
	private bool end = false;
	private int counterCorrects = 0;
	private int counterFails = 0;


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
		end = true;

		// Activate canvas representiong the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		shownText.text = numOfSequences.ToString();
		correctText.text = correctSequences.ToString ();

		// Store local data
		AppControl.control.digitSpan_DigitLength = sequenceLength;
		AppControl.control.Save ();

		// Wait for 3 seconds before transitioning to the next test
		yield return new WaitForSeconds (3f);

		// Data to be stored
		string name = "Digit Span";
		string time = System.DateTime.Now.ToString();
		string sLength = sequenceLength.ToString ();
		string totalSequences = numOfSequences.ToString ();
		string cSequences = correctSequences.ToString ();

		// Store data
		AppControl.control.dataString = "Name: " + name + ", Time: " + time + ", Length of Sequences: " + sLength +
			", Num. of Sequences: " + totalSequences + ", Correct Matches: " + cSequences; 
		AppControl.control.csvString = name + ";" + time + ";;;;;;;;" + sLength + ";" + totalSequences + ";;;" + cSequences + ";;";
		AppControl.control.SaveData ();

		// Go to next test
		SceneManager.LoadScene("MainMenu");
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

				// Reset input
				inputNumber = "";

				StartCoroutine (ResetCheck ());

				StartCoroutine(NextNumber ());
			}
		}
	}

	public void Backspace(){
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
		yield return new WaitForSeconds (0.2f);

		UpdateNumber ();
	}

	void UpdateNumber(){
		// If the test is over
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
		if (adjust == 0) {
			notifyText.text = "3 korrekte i træk. Øger sekvens længde.";
		} else {
			notifyText.text = "3 forkerte i træk. Nedsætter sekvens længde.";
		}

		yield return new WaitForSeconds (1f);

		notifyText.text = "";
	}
}
