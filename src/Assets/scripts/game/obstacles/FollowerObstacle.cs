using UnityEngine;

public class FollowerObstacle : AbstractObstacle {
  
  protected override void OnPlayerEnter() {
    AddPlayer ();
  }

  protected override void OnFollowerEnter(GameObject follower) {
    AddPlayer ();
  }


  private void AddPlayer(){
    Player player = GameController.Instance.Player;

    GameObject goFollower = GameObject.Instantiate(GameController.Instance.PrefabFollower) as GameObject;
    Follower follower = goFollower.GetComponent<Follower>();
    player.AddFollower (follower);
    player.RegroupFollowers ();
  }
}
