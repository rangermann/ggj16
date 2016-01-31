using UnityEngine;

public class DeathZone : MonoBehaviour {

  public void OnTriggerEnter2D(Collider2D other) {
    Debug.Log("Trigger enter " + other.name);
    if (other.tag == "Follower") {
      Follower follower = other.GetComponent<Follower>();
      GameController.Instance.Player.RemoveFollower(follower);
      GameController.Instance.Player.RegroupFollowers();

      // push the player to max velocity
      Player player = GameController.Instance.Player;
      player.PushForward();
      player.DidCollideWithDeathZone ();

      
     } else if (other.tag == "Priest") {
      Debug.Log ("Removing Priest");
        GameObject.Destroy (other.gameObject);
      // TODO: destroy other objects if needed
    }
  }

  //public void OnCollisionEnter2D(Collision2D collision) {
  //  Debug.Log("Collision with " + collision.gameObject.name);
  //  if (collision.collider.tag == "Follower") {
  //    Follower follower = collision.gameObject.GetComponent<Follower>();
  //    GameController.Instance.Player.RemoveFollower(follower);
  //  }
  //}
}
