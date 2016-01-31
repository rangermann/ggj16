using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateLevelFinished : AbstractState {

  private GameObject GoHellEffect { get; set; }

  private Coroutine SpinCoroutine { get; set; }


  public GameStateLevelFinished(string stateName)
    : base(stateName) {
  }

  protected override void OnInitialize() {

  }

  protected override void OnEnter(object onEnterParams = null) {
    // rotate player
    Rigidbody2D rigidbody = GameController.Instance.Player.GetComponent<Rigidbody2D>();
    SpinCoroutine = GameController.Instance.StartCoroutine(SpinPlayer(rigidbody));
  }

  private IEnumerator SpinPlayer(Rigidbody2D rigidbody) {

    // spin player
    var gameConfig = GameController.Instance.GameConfig;
    rigidbody.angularDrag = 0;
    rigidbody.angularVelocity = 0;
    float time = 0;
    float spinDuration = 5; // TODO: magic number
    while (time < spinDuration) {
      rigidbody.angularVelocity = gameConfig.playerHellEffectSpinCurve.Evaluate(time);
      time += Time.deltaTime;
      yield return null;
    }

    // open portal
    yield return new WaitForSeconds(gameConfig.playerHellPortalInitialDelay);

    // spawn hell effect
    GoHellEffect = GameObject.Instantiate(GameController.Instance.PrefabHellEffect, GameController.Instance.Player.transform.position, Quaternion.identity) as GameObject;

    // move players to hell
    yield return new WaitForSeconds(gameConfig.playerHellFollowerInitialDelay);
    List<Follower> followers = new List<Follower>(GameController.Instance.Player.Followers);
    for (int i = 0; i < followers.Count; i++) {
      yield return new WaitForSeconds(gameConfig.playerHellFollowerDelay);
      GameController.Instance.Player.RemoveFollower(followers[i]);
      GameController.Instance.Player.RegroupFollowers();
    }
    followers.Clear();

    // scale up portal
    time = 0;
    while (time < gameConfig.playerHellScaleUpDuration) { // no problem in a coroutine with yield
      var scale = GoHellEffect.transform.localScale;
      GoHellEffect.transform.localScale = new Vector3(scale.x + gameConfig.playerHellScaleUpSpeed * Time.deltaTime, scale.y + gameConfig.playerHellScaleUpSpeed * Time.deltaTime, scale.z + gameConfig.playerHellScaleUpSpeed * Time.deltaTime);
      time += Time.deltaTime;
      yield return null;
    }

    GameController.Instance.ChangeState("GameStatePlaying");
  }

  protected override void OnLeave() {
    GameController.Instance.StopCoroutine(SpinCoroutine);

    // kill the player
    GameController.Instance.KillPlayer();

    // destroy hell effect
    GameObject.Destroy(GoHellEffect);
  }

  protected override void OnUpdate() {

  }

  protected override void OnGUICustom() {
    //GUILayout.Label("Level finished");
    //if (GUILayout.Button("Next Level")) {
    //  GameController.Instance.ChangeState("GameStatePlaying");
    //}
  }


}

