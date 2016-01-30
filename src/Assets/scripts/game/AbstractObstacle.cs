using UnityEngine;

public abstract class AbstractObstacle : MonoBehaviour {

  public void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      
    }
  }


}
