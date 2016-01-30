using UnityEngine;

public class FollowerObstacle : AbstractObstacle {

  protected override void OnPlayerEnter() {
    Player player = GameController.Instance.Player;

    GameObject goFollower = GameObject.Instantiate(GameController.Instance.PrefabFollower) as GameObject;
    Follower follower = goFollower.GetComponent<Follower>();
    player.AddFollower (follower);
    player.RegroupFollowers ();

    Debug.Log ("added follower");
  }

  protected override void OnFollowerEnter() {

  }
}
