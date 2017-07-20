using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Help_Controller : MonoBehaviour {

	public GameObject content;
	public Text header;
	public GameObject textStart;
	public GameObject textWord;
	public GameObject textNBack;
	public GameObject textDSpan;
	public GameObject textSEffect;
	public GameObject textFlanker;

	private GameObject objectText;


	public void ReturnToStart(){
		SceneManager.LoadScene ("MainMenu");
	}

	void Start(){
		header.text = "Introduktion";

		objectText = Instantiate (textStart, content.transform);
	}

	public void ShowIntroduction(){
		header.text = "Introduktion";

		Destroy (objectText);

		StartCoroutine (WaitALittle (textStart));
	}

	public void ShowWordRecogText(){
		header.text = "Ord-testen";

		Destroy (objectText);

		StartCoroutine (WaitALittle (textWord));
	}

	public void ShowNBackText(){
		header.text = "Billed-testen";

		Destroy (objectText);

		StartCoroutine (WaitALittle (textNBack));
	}

	public void ShowDigitSpanText(){
		header.text = "Tal-testen";

		Destroy (objectText);

		StartCoroutine (WaitALittle (textDSpan));
	}

	public void ShowStroopEffectText(){
		header.text = "Farve-testen";

		Destroy (objectText);

		StartCoroutine (WaitALittle (textSEffect));
	}

	public void ShowEriksenFlankerText(){
		header.text = "Pile-testen";

		Destroy (objectText);

		StartCoroutine (WaitALittle (textFlanker));
	}

	IEnumerator WaitALittle(GameObject text){

		yield return new WaitForSeconds (0.1f);

		objectText = Instantiate (text, content.transform);
	}
}
