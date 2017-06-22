using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Adjust_Controller : MonoBehaviour {

	public Text wordRecogAdjust;
	public Text digitSpanAdjust;
	public Text NBackAdjust;

	private double counter;

	public void Awake(){
		wordRecogAdjust.text = AppControl.control.wordRecogStart_WordTimer.ToString ();
		digitSpanAdjust.text = AppControl.control.digitSpan_SequenceTimer.ToString ();
		NBackAdjust.text = AppControl.control.NBack_Timer.ToString ();
	}

	public void ReturnToStart(){
		SceneManager.LoadScene ("Settings");
	}

	public void AdjustWordRecog(string symbol){
		if (symbol == "+") {
			AppControl.control.wordRecogStart_WordTimer += 0.1d;
		} else if (AppControl.control.wordRecogStart_WordTimer > 0.2d){
			AppControl.control.wordRecogStart_WordTimer -= 0.1d;
		}

		wordRecogAdjust.text = AppControl.control.wordRecogStart_WordTimer.ToString ();

		AppControl.control.Save ();
	}

	public void AdjustDigitSpan(string symbol){
		if (symbol == "+") {
			AppControl.control.digitSpan_SequenceTimer = 
				AppControl.control.digitSpan_SequenceTimer + 0.1d;
		} else if (AppControl.control.digitSpan_SequenceTimer > 0.2d){
			AppControl.control.digitSpan_SequenceTimer = 
				AppControl.control.digitSpan_SequenceTimer - 0.1d;
		}
		digitSpanAdjust.text = AppControl.control.digitSpan_SequenceTimer.ToString ();

		AppControl.control.Save ();
	}

	public void AdjustNBack(string symbol){
		if (symbol == "+") {
			AppControl.control.NBack_Timer = 
				AppControl.control.NBack_Timer + 0.1d;
		} else if (AppControl.control.NBack_Timer > 0.2d){
			AppControl.control.NBack_Timer = 
				AppControl.control.NBack_Timer - 0.1d;
		}
		NBackAdjust.text = AppControl.control.NBack_Timer.ToString ();

		AppControl.control.Save ();
	}
}
