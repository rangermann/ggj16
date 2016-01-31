using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SetText : MonoBehaviour {

	public bool isTimer = false;

	float startTime;
	Text txt; 
	// Use this for initialization
	void Start () {
		txt = this.GetComponent<Text> ();
		txt.text = " ";
		//t.text = "huii";	

	}
	
	// Update is called once per frame
	void Update () {
		if (!isTimer) {
			string currentText = (GameController.Instance.Player.Followers.Count).ToString ();
			currentText = currentText + " / 10";
			txt.text = currentText;
		} else {
			if(	GameController.Instance.CurrentlyPlaying) {
			startTime = GameController.Instance.RoundStartTime;
			float currentTime = Time.time - startTime;
			string currentText = "Time " + currentTime.ToString("0.0");
			txt.text = currentText;
			}
		}
		//if(GameController.Instance.st) {}

	}
}
