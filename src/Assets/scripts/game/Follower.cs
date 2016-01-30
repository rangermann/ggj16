using UnityEngine;

public class Follower : MonoBehaviour {

  public Transform TransformInCircle { get; private set; }

  public void AttachToCircle(Transform transformInCircle) {
    TransformInCircle = transformInCircle;
  }

  public void RemoveFromCircle() {
    TransformInCircle = null;

    // TODO: animation etc.
    GameObject.Destroy(gameObject);
  }

  public void Update() {
    if (TransformInCircle != null) {
      transform.position = TransformInCircle.position;
    }
  }
}
