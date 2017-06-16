using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Help_Controller : MonoBehaviour {

	public Text header;
	public Text description;


	public void ReturnToStart(){
		SceneManager.LoadScene ("MainMenu");
	}

	void Start(){
		header.text = "Oversigt over de forskellige tests.";

		description.text = "Tryk på en af overstående knapper for at se en beskrivelse af en test.";
	}

	public void ShowWordRecogText(){
		header.text = "Word Recognition Test";

		description.text = "Word Recognition testen består af 2 dele...";
	}

	public void ShowNBackText(){
		header.text = "N-Back Test";

		description.text = "I N-Back testen skal du genkende billeder du har set for N billeder siden...";
	}

	public void ShowDigitSpanText(){
		header.text = "Digit Span Test";

		description.text = "I Digit Span testen vises du en sekvens af tal...";
	}

	public void ShowStroopEffectText(){
		header.text = "Stroop Effect Test";

		description.text = "I Stroop Effect testen skal du identificere enten farven af...";
	}

	public void ShowEriksenFlankerText(){
		header.text = "Eriksen Flanker Test";

		description.text = "I Eriksen Flanker testen skal du identificere retningen af...";
	}
}
