using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameController : MonoBehaviour {

  private static GameController instance;
  public static GameController Instance {
    get {
      return instance;
    }
  }

  private StateMachine StateMachine { get; set; }

  public void Awake() {
    instance = this;
    DontDestroyOnLoad(transform.parent.gameObject);

    List<AbstractState> states = new List<AbstractState>();
    states.Add(new GameStateLoading("GameStateLoading"));
    // add more game states here

    StateMachine = StateMachine.Create("state_machine", states, "GameStateLoading");
  }

  public void ChangeState(string stateName, object onEnterParams = null) {
    StateMachine.ChangeState(stateName, onEnterParams);
  }

  public void OnGUI() {
#if UNITY_EDITOR
    GUILayout.Label("Current state: " + StateMachine.CurrentStateName);
#endif
  }

}

