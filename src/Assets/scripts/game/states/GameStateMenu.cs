using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GameStateMenu : AbstractState {

  private Player FakePlayer { get; set; }

  public GameStateMenu(string stateName, AbstractPanelDeclaration panel)
    : base(stateName, panel) {
  }

  protected override void OnInitialize() {
    //GameObject.Instantiate(GameController.Instance.StartScreen, Vector3.zero, Quaternion.identity);
  }

  protected override void OnEnter(object onEnterParams = null) {
    GameController.Instance.Background.Reset();
    //GameController.Instance.Background.Start();

    // create fake player
    GameObject goPlayer = GameObject.Instantiate(GameController.Instance.PrefabPlayer, Vector3.zero, Quaternion.identity) as GameObject;
    FakePlayer = goPlayer.GetComponent<Player>();
    FakePlayer.IsMoving = false;
    GameController.Instance.Player = FakePlayer;
    
    // add initial followers
    for (int i = 0; i < 7; i++) { // TODO: magic number
      GameObject goFollower = GameObject.Instantiate(GameController.Instance.PrefabFollower) as GameObject;
      Follower follower = goFollower.GetComponent<Follower>();
      FakePlayer.AddFollower(follower, true);
    }
    FakePlayer.RegroupFollowers();

    // position fake player
    // TODO magic numbers
    FakePlayer.transform.position = new Vector3(0.65f, -0.89f, 0);

    // rotate fake player
    FakePlayer.transform.rotation = Quaternion.Euler(0, 0, 279.1099f);

    // scale fake player
    FakePlayer.transform.localScale = new Vector3(2.2895f, 2.2895f, 2.2895f);
  }

  protected override void OnLeave() {
    GameController.Instance.KillPlayer();
  }

  protected override void OnUpdate() {
    // TODO: serious hack
    GameController.Instance.Background.Reset();

    if (Input.anyKey) {
      Debug.Log("A key or mouse click has been detected");
      GameController.Instance.ChangeState("GameStatePlaying");
    }

  }
}

