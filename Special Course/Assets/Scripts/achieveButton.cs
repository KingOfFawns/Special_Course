using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class achieveButton : MonoBehaviour {

	private Button b;
	private CanvasGroup cg;

	public void Start(){
		b = this.GetComponent<Button> ();

		cg = b.GetComponent<CanvasGroup> ();

		if (b.name == "Achievement_Button (1)" && AppControl.control.firstTestCleared) {
			cg.alpha = 1;
		}
		else if (b.name == "Achievement_Button (10)" && AppControl.control.tenTestsCleared) {
			cg.alpha = 1;
		} 
		else if (b.name == "Achievement_Button (50)" && AppControl.control.fiftyTestsCleared) {
			cg.alpha = 1;
		} 
		else if (b.name == "Achievement_Button (100)" && AppControl.control.hundredTestsCleared) {
			cg.alpha = 1;
		} 
		else if (b.name == "Achievement_Button (150)" && AppControl.control.hundredfiftyTestsCleared) {
			cg.alpha = 1;
		}
	}
}
