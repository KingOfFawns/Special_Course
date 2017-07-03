using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class achieveButton : MonoBehaviour {

	private Button b;
	private CanvasGroup cg;
	private Text monitorText;

	public void Start(){
		b = this.GetComponent<Button> ();

		cg = b.GetComponent<CanvasGroup> ();

		if (b.name == "Achievement_Button (1)" && AppControl.control.firstTestCleared) {
			cg.alpha = 1;
		}
		else if (b.name == "Achievement_Button (5)" && AppControl.control.fiveTestsCleared > 0) {
			cg.alpha = 1;
			monitorText = transform.GetChild (3).GetComponent<Text> ();
			monitorText.text = "Achievement opnået " + AppControl.control.fiveTestsCleared + " gange.";
		} 
		else if (b.name == "Achievement_Button (10)" && AppControl.control.tenTestsCleared > 0) {
			cg.alpha = 1;
			monitorText = transform.GetChild (3).GetComponent<Text> ();
			monitorText.text = "Achievement opnået " + AppControl.control.tenTestsCleared + " gange.";
		} 
		else if (b.name == "Achievement_Button (20)" && AppControl.control.twentyTestsCleared > 0) {
			cg.alpha = 1;
			monitorText = transform.GetChild (3).GetComponent<Text> ();
			monitorText.text = "Achievement opnået " + AppControl.control.twentyTestsCleared + " gange.";
		} 
		else if (b.name == "Achievement_Button (50)" && AppControl.control.fiftyTestsCleared > 0) {
			cg.alpha = 1;
			monitorText = transform.GetChild (3).GetComponent<Text> ();
			monitorText.text = "Achievement opnået " + AppControl.control.fiftyTestsCleared + " gange.";
		}
	}
}
