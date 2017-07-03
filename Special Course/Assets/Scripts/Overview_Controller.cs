using System.Collections;
using System.Collections.Generic;
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

		int words = 0;
		int nBack = 0;
		int dSpan = 0;
		int eFLanker = 0;
		int sEffect = 0;

		foreach (string s in fileLines) {
			string[] current = s.Split (';');

			if (current [0] == "Word Recognition") {
				int value;
				int.TryParse (current [3], out value);
				if (value > words) {
					words = value;
				}
			}
			else if (current [0] == "N-Back") {
				int value;
				int.TryParse (current [14], out value);

				if (value > nBack) {
					nBack = value;
				}
			}
			else if (current [0] == "Digit Span") {
				int value;
				int.TryParse (current [9], out value);

				if (value > dSpan) {
					dSpan = value;
				}
			}
			else if (current [0] == "Eriksen Flanker") {
				int value;
				int.TryParse (current [13], out value);

				if (value > eFLanker) {
					eFLanker = value;
				}
			}
			else if (current [0] == "Stroop Effect") {
				int value;
				int.TryParse (current [13], out value);

				if (value > sEffect) {
					sEffect = value;
				}
			}
		}

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
