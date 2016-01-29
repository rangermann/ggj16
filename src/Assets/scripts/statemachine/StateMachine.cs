using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

  private Dictionary<string, AbstractState> States { get; set; }

  private AbstractState CurrentState { get; set; }

  public string CurrentStateName {
    get {
      return CurrentState.StateName;
    }
  }
  
  public void Init(List<AbstractState> states, string initialStateName = null) {
    States = new Dictionary<string, AbstractState>();
    states.ForEach(state => States[state.StateName] = state);
    if (initialStateName != null) {
      CurrentState = States[initialStateName];
    } else {
      CurrentState = states[0];
    }
  }

  public void Awake() {
    DontDestroyOnLoad(gameObject);
  }

  public void Start() {
    CurrentState.Enter();
  }

  public void ChangeState(string stateName, object onEnterParams = null) {
    if (stateName != CurrentState.StateName) {
      StartCoroutine(ChangeStateCoroutine(stateName, onEnterParams));
    }
  }

  private IEnumerator ChangeStateCoroutine(string stateName, object onEnterParams) {
    yield return new WaitForEndOfFrame();

    if (States.ContainsKey(stateName)) {
      Debug.Log("Leaving: " + CurrentState.StateName);
      CurrentState.Leave();
      CurrentState = States[stateName];
      Debug.Log("Entering: " + CurrentState.StateName);
      CurrentState.Enter(onEnterParams);
    }
  }

  public void Update() {
    CurrentState.Update();
  }

  public void OnGUI() {
    CurrentState.OnGUI();
  }

  public static StateMachine Create(string stateMachineName, List<AbstractState> states, string initialStateName = null) {
    GameObject go = new GameObject(stateMachineName);
    StateMachine stateMachine = go.AddComponent<StateMachine>();
    stateMachine.Init(states, initialStateName);
    return stateMachine;
  }
}
