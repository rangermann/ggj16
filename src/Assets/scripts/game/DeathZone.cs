using UnityEngine;

public class DeathZone : MonoBehaviour {

  public void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      GameController.Instance.ChangeState("GameStateGameOver");
    } else {
      // TODO: destroy other objects if needed
    }
  }

  //public void OnCollisionEnter2D(Collision2D collision) {
    
  //}
}
