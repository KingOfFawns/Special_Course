﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NBack_Controller : MonoBehaviour {

	public Text nTextStart;
	public Sprite[] images;
	public Button shownImage;
	public Text messageBox;
	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text textFN;
	public Text textTP;
	public Text textFP;

	private int N = 2;

	private int[] sequence = new int[100];
	private int sequenceIndex = -1;

	private bool runImages = true;
	private bool imageActive = false;

	private bool pushed = false;

	private int numOfFN = 0;
	private int numOfTP = 0;
	private int numOfFP = 0;

	private int numOfShown = -1;


	void Start(){
		// Get N from storage
		N = AppControl.control.N;

		// Show N in start
		nTextStart.text = N.ToString();
	}

	public void StartButton(){
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine (ImagePhases ());

		StartCoroutine (Timer ());
	}

	IEnumerator Timer(){
		yield return new WaitForSeconds (90);

		runImages = false;

		// Activate canvas representing the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		textFN.text = numOfFN.ToString ();
		textTP.text = numOfTP.ToString ();
		textFP.text = numOfFP.ToString ();

		// Calculate data to be stored
		float percentage = numOfTP/numOfShown * 100f;

		if (percentage >= 80f && AppControl.control.N_percentage_last >= 80f) {
			N++;
		} else if (percentage < 50f && AppControl.control.N_percentage_last < 50f){
			N--;
		}

		// Limit N
		if (N < 1) {
			N = 1;
		} else if (N > 9) {
			N = 9;
		}

		// Store local data
		AppControl.control.N = N;
		AppControl.control.N_percentage_last = percentage;
		AppControl.control.Save ();

		yield return new WaitForSeconds(3);

		// Data to be stored
		string name = "N-Back";
		string time = System.DateTime.Now.ToString();
		string FN = numOfFN.ToString();
		string TP = numOfTP.ToString();
		string FP = numOfFP.ToString ();

		// Store data
		AppControl.control.dataString = "Name: " + name + ", Time: " + time + 
			", False negatives: " + FN + ", True positives: " + TP + ", False positives: " + FP + "\n"; 
		AppControl.control.SaveData ();

		// test end
		SceneManager.LoadScene("MainMenu");

	}
	
	IEnumerator ImagePhases(){

		while(runImages) {

			shownImage.image.sprite = Resources.Load<Sprite> ("None");

			checkSelection ();

			numOfShown++;

			yield return new WaitForSeconds (0.3f);

			// Return check message to neutral
			messageBox.text = "";
			messageBox.color = Color.white;

			// Calculate random number
			int ran = (int)Random.Range (0, 10);

			// Change image according to random number
			shownImage.image.sprite = images[ran];

			// Store sequence
			sequenceIndex++;
			sequence [sequenceIndex] = ran;

			imageActive = true;
			yield return new WaitForSeconds (1);
			imageActive = false;
		}
	}

	void checkSelection(){
		if (sequenceIndex - N >= 0) {

			Debug.Log (sequence [sequenceIndex - N]);
			Debug.Log (sequence [sequenceIndex]);

			if ((sequence [sequenceIndex - N] != sequence [sequenceIndex]) && !pushed) {
				// True negative
			} else if ((sequence [sequenceIndex - N] == sequence [sequenceIndex]) && !pushed) {
				// False negative
				numOfFN++;

				messageBox.text = "Overset";
				messageBox.color = Color.red;
			} else if ((sequence [sequenceIndex - N] == sequence [sequenceIndex]) && pushed) {
				// True positive
				numOfTP++;

				messageBox.text = "Korrekt";
				messageBox.color = Color.green;
			} else if ((sequence [sequenceIndex - N] != sequence [sequenceIndex]) && pushed) {
				// False positive
				numOfFP++;

				messageBox.text = "Forkert";
				messageBox.color = Color.red;
			}
		} else if(pushed) {
			// False positive
			numOfFP++;

			messageBox.text = "Forkert";
			messageBox.color = Color.red;
		}
		pushed = false;
	}

	public void pushImage(){
		if (imageActive) {
			pushed = true;
		}
		Debug.Log (pushed);
	}
}