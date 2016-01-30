using UnityEngine;

public abstract class AbstractObstacle : MonoBehaviour {

  #region Inspector

  [Header("Misc")]
  [SerializeField]
  private bool destroyOnPlayerEnter = true;

  [SerializeField]
  private bool destroyOnFollowerEnter = true;

  [SerializeField]
  private float destroyDelaySeconds = 0;

  [SerializeField]
  private GameObject prefabDestroyEffect;

  #endregion

  public void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      HandlePlayerEnter ();
    } else if (other.tag == "Follower") {
      HandleFollowerEnter (other.gameObject);
    } else {
      Debug.Log ("Collission with object tagged " + other.tag);
    }
  }

  private void HandlePlayerEnter() {
    if (prefabDestroyEffect != null) {
      GameObject.Instantiate(prefabDestroyEffect, transform.position, Quaternion.identity);
    }

    OnPlayerEnter();

    if (destroyOnPlayerEnter) {
      GameObject.Destroy(gameObject, destroyDelaySeconds);
    }
  }

  private void HandleFollowerEnter(GameObject follower){
    OnFollowerEnter(follower);

    if (destroyOnFollowerEnter) {
      GameObject.Destroy(gameObject, destroyDelaySeconds);
    }
  }

  protected abstract void OnPlayerEnter();

  protected abstract void OnFollowerEnter(GameObject follower);
}
