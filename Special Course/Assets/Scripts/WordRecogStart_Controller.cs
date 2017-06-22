using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordRecogStart_Controller : MonoBehaviour {

	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text word;

	private int numOFWords = 10;
	private string[] chosenWords;

	// Use this for initialization
	void Start () {
		// Load local data for number of words
		numOFWords = AppControl.control.word_Recog_Target;

		// Setup chosen words
		chosenWords = new string[numOFWords];

		// Get amount of words
		int wordLength = AppControl.control.words.Length;

		// Array to save random numbers for choosing words
		int[] randoms = new int[numOFWords];

		// Choose needed amount of random words
		for (int i = 0; i < numOFWords; i++) {
			int ran = Random.Range (0, wordLength);

			// Check if word is already chosen, and if it is choose a new one
			bool check = false;
			while (!check) {
				check = true;
				foreach (int r in randoms) {
					if (ran == r) {
						check = false;
						ran = Random.Range (0, wordLength);
						break;
					}
				}
			}
			randoms [i] = ran;

			// Store chosen word
			chosenWords [i] = AppControl.control.words [ran];
		}
	}

	public void StartButton(){
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		StartCoroutine (ShowWords ());
	}

	IEnumerator ShowWords(){

		for (int i = 0; i < numOFWords; i++) {
			yield return new WaitForSeconds (0.3f);

			// Show word
			word.text = chosenWords[i];

			// Wait designated time
			yield return new WaitForSeconds ((float)AppControl.control.wordRecogStart_WordTimer);

			// Show blank
			word.text = "";
		}
			
		// Test end
		endCanvas.SetActive (true);

		yield return new WaitForSeconds (3);

		//Store chosen words
		AppControl.control.chosenWords = chosenWords;

		SceneManager.LoadScene ("MainMenu");

	}

}
