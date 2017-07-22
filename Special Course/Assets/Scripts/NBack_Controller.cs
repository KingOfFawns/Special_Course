using System.Collections;
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
	public Text textN;
	public Scrollbar timeBar;

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
	private int randomizer = 0;


	void Start(){
		// Get N from storage
		N = AppControl.control.N;

		// Show N in start
		nTextStart.text = N.ToString();
	}

	void Update(){
		System.DateTime now = System.DateTime.Now;
		System.DateTime testStart = AppControl.control.testStartDate;

		if (now.Subtract (testStart).TotalSeconds >= 600) {
			AppControl.control.testStarted = true;
			AppControl.control.Save ();

			// Log stop
			string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
			string time = System.DateTime.Now.ToString ();

			AppControl.control.dataString = "Patient Number: " + patientNumber + ", Test udløbet" + ", Time: " + time;
			AppControl.control.csvString = patientNumber + ";Test Stopped;" + time + ";;;;;;;;;;;;;;;;";
			AppControl.control.SaveData ();

			SceneManager.LoadScene ("MainMenu");
		}
	}

	public void StartButton(){
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine (ImagePhases ());

		StartCoroutine (Timer ());
	}

	IEnumerator Timer(){
		// The timer waits for 60 seconds
		for (int i = 1; i < 61; i++) {
			yield return new WaitForSeconds(1);
			timeBar.size = i / 60f;
			timeBar.transform.GetChild (1).GetComponent<Text> ().text = (60 - i).ToString ();
		}
		runImages = false;

		// Activate canvas representing the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		textFN.text = numOfFN.ToString ();
		textTP.text = numOfTP.ToString ();
		textFP.text = numOfFP.ToString ();

		// Special data save
		string Nsave = N.ToString();

		// Calculate data to be stored
		float percentage = numOfTP/numOfShown * 100f;

		if (percentage >= 80f && AppControl.control.N_percentage_last >= 80f) {
			N++;
		} else if (percentage < 50f && AppControl.control.N_percentage_last < 50f){
			N--;
		}

		// Limit N
		if (N < 2) {
			N = 2;
		} else if (N > 9) {
			N = 9;
		}

		textN.text = N.ToString ();

		// Store local data
		AppControl.control.N = N;
		AppControl.control.N_percentage_last = percentage;
		AppControl.control.Save ();

		yield return new WaitForSeconds(3);

		// Data to be stored
		string patientNumber = "#" + AppControl.control.patientNumber.ToString().Substring(1);
		string name = "N-Back";
		string time = System.DateTime.Now.ToString();
		string FN = numOfFN.ToString();
		string TP = numOfTP.ToString();
		string FP = numOfFP.ToString ();

		// Store data
		AppControl.control.dataString = "Patient Number: " + patientNumber + ", Name: " + name + ", Time: " + time + ", N: " + Nsave +
			", False negatives: " + FN + ", True positives: " + TP + ", False positives: " + FP; 
		AppControl.control.csvString = patientNumber + ";" + name + ";" + time + ";;;;;" + FN + ";" + TP + ";" + FP + ";;;;;;" + Nsave + ";" + numOfShown.ToString();
		AppControl.control.SaveData ();

		// test end
		int ran = Random.Range (0, 2);
		if (ran == 0) {
			SceneManager.LoadScene ("Stroop_Effect");	
		} else {
			SceneManager.LoadScene ("Eriksen_Flanker");
		}

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

			// Increase index
			sequenceIndex++;

			// Calculate random number
			int ran = (int)Random.Range (0, 10);

			if (sequenceIndex - N >= 0) {
				// When not maching -N image, increase chance
				if (ran != sequence [sequenceIndex - N]) {
					randomizer++;
				} else {
					randomizer = 0;
				}

				// if randomizer is active, increase chance of matching -N image
				if (randomizer > 0) {
					int ran2 = (int)Random.Range (0, 10 - randomizer);
					if (ran2 == 0) {
						ran = sequence[sequenceIndex - N];
						randomizer = 0;
					}
				}

			}

			// Change image according to random number
			shownImage.image.sprite = images[ran];

			// Store sequence
			sequence [sequenceIndex] = ran;

			imageActive = true;
			yield return new WaitForSeconds ((float)AppControl.control.NBack_Timer);
			imageActive = false;
		}
	}

	void checkSelection(){
		if (sequenceIndex - N >= 0) {

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
	}
}
