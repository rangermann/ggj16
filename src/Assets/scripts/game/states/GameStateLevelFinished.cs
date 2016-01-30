using UnityEngine;


public class GameStateLevelFinished : AbstractState {

  public GameStateLevelFinished(string stateName)
    : base(stateName) {
  }

  protected override void OnInitialize() {

  }

  protected override void OnEnter(object onEnterParams = null) {

  }

  protected override void OnLeave() {

  }

  protected override void OnUpdate() {

  }

  protected override void OnGUICustom() {
    GUILayout.Label("Level finished");
    if (GUILayout.Button("Next Level")) {
      GameController.Instance.ChangeState("GameStatePlaying");
    }
  }
}

