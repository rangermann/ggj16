using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameStatePlaying : AbstractState {

  private Transform TransformLevelCamera { get; set; }

  private GameConfig GameConfig { get; set; }

  private Player Player { get; set; }

  public GameStatePlaying(string stateName)
    : base(stateName) {
  }

  protected override void OnInitialize() {
    GameConfig = GameController.Instance.GameConfig;
  }

  protected override void OnEnter(object onEnterParams = null) {
    // reset camera
    TransformLevelCamera = GameController.Instance.TransformLevelCamera;
    Vector3 camPos = TransformLevelCamera.position;
    camPos.x = 0;
    TransformLevelCamera.position = camPos;

    // spawn player
    SpawnPlayer();

    // start level generation
    int seed = 234234; // TODO: use seed from onEnterParams
    GameController.Instance.LevelGenerator.CleanUp();
    GameController.Instance.LevelGenerator.StartGenerating(seed);
  }

  protected override void OnLeave() {
    // kill the player
    Player.ClearFollowers();
    GameObject.Destroy(Player.gameObject);
  }

  protected override void OnUpdate() {
    MoveCamera();

    //if (Player.Followers.Count < GameConfig.followersMin) {
    //  GameController.Instance.ChangeState("GameStateGameOver");
    //} else if (Player.Followers.Count >= GameConfig.followersToWin) {
    //  GameController.Instance.ChangeState("GameStateLevelFinished");
    //}
  }

  private void MoveCamera() {
    Vector3 camPos = TransformLevelCamera.position;
    camPos.x += GameConfig.cameraMovementSpeed * Time.deltaTime;
    TransformLevelCamera.position = camPos;
  }


  private void SpawnPlayer() {
    GameObject goPlayer = GameObject.Instantiate(GameController.Instance.PrefabPlayer, Vector3.zero, Quaternion.identity) as GameObject;
    Player = goPlayer.GetComponent<Player>();

    // add initial followers
    for (int i = 0; i < GameConfig.followersMin; i++) {
      GameObject goFollower = GameObject.Instantiate(GameController.Instance.PrefabFollower) as GameObject;
      Follower follower = goFollower.GetComponent<Follower>();
      Player.AddFollower(follower);
    }
    Player.RegroupFollowers();
  }
}
