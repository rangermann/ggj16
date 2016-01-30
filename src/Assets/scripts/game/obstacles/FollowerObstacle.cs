using UnityEngine;

public class FollowerObstacle : AbstractObstacle {
  public void Awake() {
    transform.Rotate (new Vector3(0.0f, 0.0f, Random.value * 360));
  }

  protected override void OnPlayerEnter() {
    AddPlayer ();
  }

  protected override void OnFollowerEnter(GameObject follower) {
    AddPlayer ();
  }


  private void AddPlayer() {
    Player player = GameController.Instance.Player;

    GameObject goFollower = GameObject.Instantiate(GameController.Instance.PrefabFollower) as GameObject;
    goFollower.transform.position = transform.position;
    Follower follower = goFollower.GetComponent<Follower>();
    player.AddFollower (follower);
    player.RegroupFollowers ();
  }
}
