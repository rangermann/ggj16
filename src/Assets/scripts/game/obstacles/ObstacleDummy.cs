using UnityEngine;

public class ObstacleDummy : AbstractObstacle {

  protected override void OnPlayerEnter() {
    Debug.Log("Player entered");
  }


  protected override void OnFollowerEnter(){
    
  }
}
