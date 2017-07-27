using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour {

	public void TestOfWordRecog(){
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Word_Recog_Start");
	}

	public void TestOfEriksenFlanker(){
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Eriksen_Flanker");
	}

	public void TestOfNBack(){
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("N_Back");
	}

	public void TestOfDigitSpan(){
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Digit_Span");
	}

	public void TestOfStroopEffect(){
		AppControl.control.testOfTest = true;
		SceneManager.LoadScene ("Stroop_Effect");
	}
}
