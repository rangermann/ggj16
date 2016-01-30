using UnityEngine;

public abstract class AbstractObstacle : MonoBehaviour {

  #region Inspector

  [Header("Misc")]
  [SerializeField]
  private bool destroyOnPlayerEnter = true;

  [SerializeField]
  private float destroyDelaySeconds = 0;

  [SerializeField]
  private GameObject prefabDestroyEffect;

  #endregion

  public void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Player") {
      HandlePlayerEnter();
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

  protected abstract void OnPlayerEnter();
}
