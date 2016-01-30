using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  #region Inspector

  [SerializeField]
  private Transform transformFollowers;

  #endregion

  public Vector2 velocity;
  private Rigidbody2D rigidBody;

  private GameConfig GameConfig { get; set; }

  public List<Follower> Followers { get; private set; }

  public List<Transform> FollowerTransforms { get; private set; }

  public void Awake() {
    GameConfig = GameController.Instance.GameConfig;
    rigidBody = GetComponent<Rigidbody2D>();
    Followers = new List<Follower>();
    FollowerTransforms = new List<Transform>();
  }

  // Update is called once per frame
  public void Update() {
    float currentLocalScale = GetScale();
    float scaleFactor = 0;

    if (GameConfig.controlByPress) {
      bool scaleUp = !Input.GetMouseButton(0);


      if (!scaleUp && currentLocalScale > GameConfig.playerMinScale) {
        scaleFactor = -GameConfig.playerScaleDownFactor;
      } else if (scaleUp && currentLocalScale < GameConfig.playerMaxScale) {
        scaleFactor = GameConfig.playerScaleUpFactor;
      }
      transform.localScale += new Vector3(scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime, 0);
    } else {
      if (Input.GetMouseButtonDown(0)) {
        Debug.Log("One button control");
        if (currentLocalScale > GameConfig.playerMinScale) {
          scaleFactor = -GameConfig.playerScaleDownFactor;
          transform.localScale += new Vector3(scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime, 0);
        }
      } else {
        if (currentLocalScale < GameConfig.playerMaxScale) {
          scaleFactor = GameConfig.playerScaleUpFactor;
          transform.localScale += new Vector3(scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime, 0);
        }
      }
      //transform.localScale += new Vector3 (scaleFactor  Time.deltaTime, scaleFactor  Time.deltaTime, 0);
    }
  }

  public void FixedUpdate() {
    float normedScale = 1 - (GetScale() * GameConfig.playerScaleImpact - GameConfig.playerMinScale) / GameConfig.playerMaxScale;
    float speedVariation = normedScale * (GameConfig.playerMaxVelocityDelta - GameConfig.playerMinVelocityDelta) + GameConfig.playerMinVelocityDelta;


    if (!(GetReferenceX() + GameConfig.playerMaxCameraOffset > GetCurrentX() || speedVariation < GameConfig.cameraMovementSpeed)) {
      speedVariation = 0.0f;
    }

    bool tooFar = GetReferenceX() + GameConfig.playerMaxCameraOffset < GetCurrentX();

    if (tooFar && speedVariation > 0) {
      speedVariation = 0.0f;
    }

    Vector2 velocity = rigidBody.velocity;
    velocity.x = GameConfig.cameraMovementSpeed + speedVariation;
    rigidBody.velocity = velocity;
  }

  private float GetReferenceX() {
    Vector2 cameraPosition = GameController.Instance.TransformLevelCamera.position;

    //Debug.Log ("Current cPos: " + cameraPosition.x);

    return cameraPosition.x;
  }

  private float GetScale() {
    return transform.localScale.x;
  }

  private float GetCurrentX() {
    //Debug.Log ("Current x: " + transform.position.x);

    return transform.position.x;
  }

  public void AddFollower(Follower follower) {
    string followerName = "follower_" + (Followers.Count + 1);
    Debug.Log("Adding follower");
    follower.name = followerName;
    Followers.Add(follower);

    GameObject goFollowerTransform = new GameObject("follower_mount") as GameObject;
    goFollowerTransform.transform.SetParent(transformFollowers);
    FollowerTransforms.Add(goFollowerTransform.transform);

    follower.AttachToCircle(goFollowerTransform.transform);
  }


  public void RemoveFollower(Follower follower) {
    Debug.Log("Removing follower " + follower.name);
    FollowerTransforms.RemoveAt(0);
    follower.RemoveFromCircle();
    Followers.Remove(follower);
  }

  public void ClearFollowers() {
    Followers.ForEach(follower => GameObject.Destroy(follower.gameObject));
    Followers.Clear();
    FollowerTransforms.ForEach(t => GameObject.Destroy(t.gameObject));
    FollowerTransforms.Clear();
  }

  public void RegroupFollowers() {
    Debug.Log("Followers.Count: " + Followers.Count);
    for (int i = 0; i < Followers.Count; i++) {
      //multiply 'i' by '1.0f' to ensure the result is a fraction
      float pointNum = (i * 1.0f) / Followers.Count;

      //angle along the unit circle for placing points
      float angle = pointNum * Mathf.PI * 2;

      float x = Mathf.Sin(angle);
      float y = Mathf.Cos(angle);

      FollowerTransforms[i].localPosition = new Vector2(x, y);
    }

    Followers.ForEach(follower => follower.RecreateLineRenderers());
  }
}
