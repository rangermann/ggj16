using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

  public Transform TransformInCircle { get; private set; }

  private Dictionary<Transform, LineRenderer> LineRenderers { get; set; }

  public void Awake() {
    LineRenderers = new Dictionary<Transform, LineRenderer>();
  }

  public void AttachToCircle(Transform transformInCircle) {
    TransformInCircle = transformInCircle;
  }

  public void RecreateLineRenderers() {

    foreach (var lineRenderer in LineRenderers.Values) {
      GameObject.Destroy(lineRenderer.gameObject);
    }

    LineRenderers.Clear();

    // create line renderers to all other followers
    var player = GameController.Instance.Player;
    var prefab = GameController.Instance.PrefabFollowerLineRenderer;
    player.FollowerTransforms.ForEach(followerTransform => {

      if (followerTransform != TransformInCircle) {

        GameObject goLineRenderer = GameObject.Instantiate(prefab);
        goLineRenderer.transform.SetParent(transform);
        LineRenderer lineRenderer = goLineRenderer.GetComponent<LineRenderer>();
        LineRenderers[followerTransform] = lineRenderer;
        lineRenderer.SetVertexCount(2);
        UpdateLineRenderPositions(followerTransform);
      }

    });
  }

  private void UpdateLineRenderPositions(Transform followerTransform) {
    if (LineRenderers.ContainsKey(followerTransform)) {
      var lineRenderer = LineRenderers[followerTransform];
      lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, lineRenderer.transform.position.z));
      lineRenderer.SetPosition(1, new Vector3(followerTransform.position.x, followerTransform.position.y, lineRenderer.transform.position.z));
    }
  }

  public void RemoveFromCircle() {
    TransformInCircle = null;

    // TODO: animation etc.
    GameObject.Destroy(gameObject);
  }

  public void Update() {
    if (TransformInCircle != null) {
      transform.position = TransformInCircle.position;

      var player = GameController.Instance.Player;
      player.FollowerTransforms.ForEach(followerTransform => {
        UpdateLineRenderPositions(followerTransform);
      });
    }
  }
}
