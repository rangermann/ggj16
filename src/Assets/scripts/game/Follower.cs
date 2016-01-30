using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

  [SerializeField]
  private float destroyDelaySeconds = 0;

  [SerializeField]
  private GameObject prefabDestroyEffect;

  public Transform TransformInCircle { get; private set; }

  private Dictionary<Transform, LineRenderer> LineRenderers { get; set; }

  public void Awake() {
    LineRenderers = new Dictionary<Transform, LineRenderer>();
  }

  public void AttachToCircle(Transform transformInCircle, bool snap) {
    TransformInCircle = transformInCircle;
    if (snap) {
      transform.position = TransformInCircle.position;
    } else {
      MoveToPosition();
    }
  }

  public void MoveToPosition() {
    if (TransformInCircle == null) {
      return;
    }
    var transformInCircle = TransformInCircle;
    TransformInCircle = null;
    StartCoroutine(MoveIntoCircle(transformInCircle));
  }

  private IEnumerator MoveIntoCircle(Transform transformInCircle) {
    var gameConfig = GameController.Instance.GameConfig;
    var startPosition = transform.position;
    var distance = Vector3.Distance(startPosition, transformInCircle.position);
    var duration = distance / gameConfig.followersMovementSpeed;
    float timeTaken = 0;
    while (timeTaken < duration) {

      var currentPosition = Vector3.Lerp(startPosition, transformInCircle.position, (timeTaken / duration));
      transform.position = currentPosition;

      UpdateLineRenderers();

      timeTaken += Time.deltaTime;
      yield return null;
    }

    TransformInCircle = transformInCircle;
  }

  public void RecreateLineRenderers() {

    DestroyLineRenderers();

    // create line renderers to all other followers
    var player = GameController.Instance.Player;
    var prefab = GameController.Instance.PrefabFollowerLineRenderer;
    player.Followers.ForEach(follower => {

      var followerTransform = follower.transform;

      if (follower.TransformInCircle != TransformInCircle) {

        GameObject goLineRenderer = GameObject.Instantiate(prefab);
        goLineRenderer.transform.SetParent(transform);
        LineRenderer lineRenderer = goLineRenderer.GetComponent<LineRenderer>();
        LineRenderers[followerTransform] = lineRenderer;
        lineRenderer.SetVertexCount(2);
        UpdateLineRenderPositions(followerTransform);
      }

    });
  }

  private void DestroyLineRenderers() {
    foreach (var lineRenderer in LineRenderers.Values) {
      GameObject.Destroy(lineRenderer.gameObject);
    }

    LineRenderers.Clear();
  }

  private void UpdateLineRenderPositions(Transform followerTransform) {
    if (LineRenderers.ContainsKey(followerTransform)) {
      var lineRenderer = LineRenderers[followerTransform];
      Vector3 position0 = new Vector3(transform.position.x, transform.position.y, lineRenderer.transform.position.z);
      Vector3 position1 = new Vector3(followerTransform.position.x, followerTransform.position.y, lineRenderer.transform.position.z);
      lineRenderer.SetPosition(0, position0);
      lineRenderer.SetPosition(1, position1);
      Vector2 mainTextureOffset = lineRenderer.material.mainTextureOffset;
      mainTextureOffset.x += GameController.Instance.GameConfig.followersLineMovementSpeed * Time.deltaTime;
      lineRenderer.material.mainTextureOffset = mainTextureOffset;
      float distance = Vector3.Distance(position0, position1);
      lineRenderer.material.mainTextureScale = new Vector2(distance / 1.0f, 1);
    }
  }

  public void RemoveFromCircle() {
    TransformInCircle = null;
    DestroyLineRenderers();

    if (prefabDestroyEffect != null) {
      GameObject.Instantiate(prefabDestroyEffect, transform.position, Quaternion.identity);
    }
    GameObject.Destroy(gameObject, destroyDelaySeconds);
  }

  public void Update() {
    if (TransformInCircle != null) {
      transform.position = TransformInCircle.position;
      //transform.position = Vector3.Lerp(transform.position, TransformInCircle.position, GameController.Instance.GameConfig.followersMovementSpeed * Time.deltaTime);
      UpdateLineRenderers();

      transform.rotation = TransformInCircle.rotation;
    }
  }

  private void UpdateLineRenderers() {
    var player = GameController.Instance.Player;
    player.Followers.ForEach(follower => {
      UpdateLineRenderPositions(follower.transform);
    });
  }
}
