using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour {

	public void TestOfWordRecog(){
		// Initialize help's test mode
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Word_Recog_Start");
	}

	public void TestOfEriksenFlanker(){
		// Initialize help's test mode
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Eriksen_Flanker");
	}

	public void TestOfNBack(){
		// Initialize help's test mode
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("N_Back");
	}

	public void TestOfDigitSpan(){
		// Initialize help's test mode
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Digit_Span");
	}

	public void TestOfStroopEffect(){
		// Initialize help's test mode
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Stroop_Effect");
	}
}
