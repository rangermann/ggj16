using UnityEngine;

public class GameStateGameOver : AbstractState {

  public GameStateGameOver(string stateName)
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
    GUILayout.Label("Game over");
    if (GUILayout.Button("Restart")) {
      GameController.Instance.ChangeState("GameStatePlaying");
    }
  }
}
