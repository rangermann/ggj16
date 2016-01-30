using UnityEngine;

public class TrapObstacle : AbstractObstacle {

  protected override void OnPlayerEnter() {
  }

  protected override void OnFollowerEnter(GameObject goFollower){
    Player player = GameController.Instance.Player;

    Follower follower = goFollower.GetComponent<Follower> ();

    player.RemoveFollower (follower);
    player.RegroupFollowers ();
  }
}
