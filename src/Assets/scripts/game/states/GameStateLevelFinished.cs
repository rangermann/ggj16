using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateLevelFinished : AbstractState {

  private GameObject GoHellEffect { get; set; }

  private bool IsSpinning { get; set; }

  public GameStateLevelFinished(string stateName)
    : base(stateName) {
  }

  protected override void OnInitialize() {

  }

  protected override void OnEnter(object onEnterParams = null) {
    // spawn hell effect
    GoHellEffect = GameObject.Instantiate(GameController.Instance.PrefabHellEffect, GameController.Instance.Player.transform.position, Quaternion.identity) as GameObject;

    // rotate player
    Rigidbody2D rigidbody = GameController.Instance.Player.GetComponent<Rigidbody2D>();
    GameController.Instance.StartCoroutine(SpinPlayer(rigidbody));
    GameController.Instance.StartCoroutine(MoveFollowersToHell());
  }

  private IEnumerator MoveFollowersToHell() {
    var gameConfig = GameController.Instance.GameConfig;
    yield return new WaitForSeconds(gameConfig.playerHellFollowerInitialDelay);
    List<Follower> followers = new List<Follower>(GameController.Instance.Player.Followers);
    for (int i = 0; i < followers.Count; i++) {
      yield return new WaitForSeconds(gameConfig.playerHellFollowerDelay);
      GameController.Instance.Player.RemoveFollower(followers[i]);
    }
    followers.Clear();
  }

  private IEnumerator SpinPlayer(Rigidbody2D rigidbody) {
    var gameConfig = GameController.Instance.GameConfig;
    rigidbody.angularDrag = 0;
    rigidbody.angularVelocity = 0;
    IsSpinning = true;
    float time = 0;
    while (IsSpinning) {
      rigidbody.angularVelocity = gameConfig.playerHellEffectSpinCurve.Evaluate(time);
      time += Time.deltaTime;
      yield return null;
    }
  }

  protected override void OnLeave() {
    IsSpinning = false;

    // kill the player
    var player = GameController.Instance.Player;
    player.ClearFollowers();
    GameObject.Destroy(player.gameObject);
    GameController.Instance.Player = null;

    // destroy hell effect
    GameObject.Destroy(GoHellEffect);
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

