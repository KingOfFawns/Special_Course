using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {

	public void Load_Recog_Start(){
		SceneManager.LoadScene ("Word_Recog_Start");
	}

	public void Load_Recog_NBack(){
		SceneManager.LoadScene ("N_Back");
	}

	public void Load_Recog_DigitSpan(){
		SceneManager.LoadScene ("Digit_Span");
	}

	public void Load_Recog_StroopEffect(){
		SceneManager.LoadScene ("Stroop_Effect");
	}

	public void Load_Recog_EriksenFlanker(){
		SceneManager.LoadScene ("Eriksen_Flanker");
	}

	public void Load_Recog_End(){
		SceneManager.LoadScene ("Word_Recog_End");
	}

	public void Load_Main_Menu(){
		SceneManager.LoadScene ("MainMenu");
	}
}
