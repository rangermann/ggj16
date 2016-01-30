using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameStateLoading : AbstractState {



  public GameStateLoading(string stateName)
    : base(stateName) {
  }

  protected override void OnInitialize() {
		//GameController.Instance.StartScreen; 
		GameObject stScreen = GameObject.Instantiate(GameController.Instance.StartScreen, Vector3.zero, Quaternion.identity) as GameObject;
  }

  protected override void OnEnter(object onEnterParams = null) {
	GameController.Instance.Background.Reset ();
	GameController.Instance.Background.Start ();
  }

  protected override void OnLeave() {

  }

  protected override void OnUpdate() {

		if (Input.anyKey){
			Debug.Log("A key or mouse click has been detected");
			GameController.Instance.ChangeState("GameStatePlaying");
		}

  }
}

