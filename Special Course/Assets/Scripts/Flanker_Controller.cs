using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flanker_Controller : MonoBehaviour {

	public GameObject arrow;
	public Image check;
	public GameObject startCanvas;
	public GameObject canvas;
	public GameObject endCanvas;
	public Text shownText;
	public Text correctText;

	private int rand_target = -1;
	private int rand_distractors = -1;

	private float xStart = -2.25f;
	private float yStart = 3.7f;

	private int gridsShown = 0;
	private int correctSelections = 0;

	private bool active = false;

	// Use this for initialization
	void Start () {


	}

	public void StartButton(){
		// Stop start canvas
		startCanvas.SetActive(false);

		canvas.SetActive (true);

		// Set up first time arrows
		SetArrows ();

		// Start a timer
		StartCoroutine (Timer ());
	}

	IEnumerator Timer(){
		// The timer waits for 60 seconds
		yield return new WaitForSeconds (60f);

		// Activate canvas representiong the end of the test
		// plus show motivation scores
		endCanvas.SetActive (true);
		shownText.text = gridsShown.ToString();
		correctText.text = correctSelections.ToString ();

		// Data to be stored


		// Wait for 3 seconds before transitioning to the next test
		yield return new WaitForSeconds (3f);
		// Go to next test
		Debug.Log("Transition");
	}

	public void CheckSelection(int dir){
		// Active signifies that a new set of arrows are being spawned
		// nullifies button mashing between arrow sets
		if (!active) {
			// If the selected button is the same as the target, show a check mark and count it
			// Else show an x representing failure
			if (dir == rand_target) {
				check.sprite = Resources.Load<Sprite>("checkmark");
				correctSelections++;
			} else {
				check.sprite = Resources.Load<Sprite>("failure");
			}

			// Start the spawning of the next set of arrows
			StartCoroutine (NextArrowSet ());

			StartCoroutine (ResetCheck ());
		}
	}

	IEnumerator NextArrowSet(){
		active = true; // Spawning in progress

		RemoveArrows ();

		// Wait half a second
		yield return new WaitForSeconds (0.5f);

		SetArrows ();
		active = false; // Spawning done
	}

	IEnumerator ResetCheck(){
		// Wait 0.3 seconds, then remove checkmark/X
		yield return new WaitForSeconds (0.3f);
		check.sprite = Resources.Load<Sprite> ("None"); 
	}

	void SetArrows () {
		gridsShown++; // Count every set of arrows shown

		// Random directions for target arrow and distractor arrows
		rand_target = Random.Range(0, 4);
		rand_distractors = Random.Range(0, 4);

		// Start position for arrow grid
		float x = xStart;
		float y = yStart;

		// Arrow spawnning
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {

				// Distractor arrows
				if(i != 3 || j != 3){
					Vector3 pos = new Vector3 (x, y, 0f);
					Instantiate (arrow, pos, Quaternion.Euler(0,0,90*rand_distractors));
				}
				// Center arrow - target
				else{
					Vector3 pos = new Vector3 (x, y, 0f);
					Instantiate (arrow, pos, Quaternion.Euler(0,0,90*rand_target));
				}

				// Move along grid in y-direction
				y -= 0.75f;

			}
			// Reset y and move to next column
			y = yStart;
			x += 0.75f;
		}
	}

	void RemoveArrows(){
		// Find all arrows spawned
		GameObject[] arrows = GameObject.FindGameObjectsWithTag ("arrow");

		// Remove all found arrows
		foreach (GameObject g in arrows) {
			Destroy (g);
		}
	}
}
