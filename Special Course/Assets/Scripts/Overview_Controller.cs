using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Overview_Controller : MonoBehaviour {

	public Text wordRecog;
	public Text NBack;
	public Text digitSpan;
	public Text stroopEffect;
	public Text eriksenFlanker;

	void Start(){
		// Read data file into string array fileLines
		string[] fileLines = File.ReadAllLines (Application.persistentDataPath + "/.dat1.dat");

		// Variables to store needed data
		int words = 0;
		int nBack = 0;
		int dSpan = 0;
		int eFLanker = 0;
		int sEffect = 0;

		// Go through all data and find the largest of each type of tests appropiate data
		foreach (string s in fileLines) {
			string[] current = s.Split (';');

			if (current [1] == "Word Recognition") {
				int value;
				int.TryParse (current [4], out value);
				if (value > words) {
					words = value;
				}
			}
			else if (current [1] == "N-Back") {
				int value;
				int.TryParse (current [15], out value);

				if (value > nBack) {
					nBack = value;
				}
			}
			else if (current [1] == "Eriksen Flanker") {
				int value;
				int.TryParse (current [14], out value);

				if (value > eFLanker) {
					eFLanker = value;
				}
			}
			else if (current [1] == "Stroop Effect") {
				int value;
				int.TryParse (current [14], out value);

				if (value > sEffect) {
					sEffect = value;
				}
			}
		}

		// Get digit spans max sequence length
		dSpan = AppControl.control.maxSequenceLength;

		// Set text accordingly to data
		wordRecog.text = words.ToString();
		NBack.text = nBack.ToString();
		digitSpan.text = dSpan.ToString();
		stroopEffect.text = sEffect.ToString();
		eriksenFlanker.text = eFLanker.ToString();
	}

	public void GoToMainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}

	public void GoToOverview(){
		SceneManager.LoadScene ("Overview");
	}

	public void GoToAchievements(){
		SceneManager.LoadScene ("Achievements");
	}
}
