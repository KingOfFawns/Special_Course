using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	public void Load_Main_Menu () 
	{
		SceneManager.LoadScene (0);
	}

	public void Load_Word_Recog_Start () 
	{
		SceneManager.LoadScene (1);
	}

	public void Load_Word_Recog_End () 
	{
		SceneManager.LoadScene (6);
	}

	public void LoadTestSet1(){
		int ran = Random.Range (0, 2);

		if (ran == 1) {
			Load_N_Back ();
		} else {
			Load_Digit_Span ();
		}
	}

	public void LoadTestSet2(){
		int ran = Random.Range (0, 2);

		if (ran == 1) {
			Load_Stroop_Effect ();
		} else {
			Load_Eriksen_Flanker ();
		}
	}

	public void Load_N_Back () 
	{
		SceneManager.LoadScene (2);
	}

	public void Load_Digit_Span () 
	{
		SceneManager.LoadScene (3);
	}

	public void Load_Stroop_Effect () 
	{
		SceneManager.LoadScene (4);
	}

	public void Load_Eriksen_Flanker () 
	{
		SceneManager.LoadScene (5);
	}
}
