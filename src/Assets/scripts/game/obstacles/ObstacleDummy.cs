using UnityEngine;

public class ObstacleDummy : AbstractObstacle {

  protected override void OnPlayerEnter() {
    Debug.Log("Player entered obstacle");
  }


  protected override void OnFollowerEnter(GameObject follower){
    Debug.Log("Follower entered");
  }
}
