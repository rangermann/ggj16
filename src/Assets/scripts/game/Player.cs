using System.Collections;
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
  private bool didCollideWithDeathZone;

  public List<Follower> Followers { get; private set; }
  public List<PriestObstacle> Priests { get; private set; }
  public List<PriestObstacle> PriestsToMove { get; private set; }
  public Dictionary<Follower, Transform> FollowerTransforms { get; private set; }

  private bool isMoving;
  public bool IsMoving {
    get {
      return isMoving;
    }
    set {
      rigidBody.angularVelocity = 0;
      rigidBody.velocity = Vector2.zero;
      isMoving = value;
    }
  }

  public void Awake() {
    GameConfig = GameController.Instance.GameConfig;
    rigidBody = GetComponent<Rigidbody2D>();
    Followers = new List<Follower>();
    Priests = new List<PriestObstacle> ();
    PriestsToMove = new List<PriestObstacle> ();

    FollowerTransforms = new Dictionary<Follower, Transform>();
    IsMoving = true;
    didCollideWithDeathZone = false;
  }

  // Update is called once per frame
  public void Update() {
    
    if (IsMoving == false) {
      return;
    }

    float currentLocalScale = GetScale();
    float scaleFactor = 0;

    if (GameConfig.controlByPress) {
      //bool scaleUp = !Input.GetMouseButton(0);

      bool scaleUp = !Input.GetButton("SimpleButton");

      if (!scaleUp && currentLocalScale > GameConfig.playerMinScale) {
        scaleFactor = -GameConfig.playerScaleDownFactor;
      } else if (scaleUp && currentLocalScale < GameConfig.playerMaxScale) {
        scaleFactor = GameConfig.playerScaleUpFactor;
      }
      IncreaseScaleBy(scaleFactor);
    } else {
      //if (Input.GetMouseButtonDown(0)) {
      if (Input.GetButtonDown("SimpleButton")) {
        if (currentLocalScale > GameConfig.playerMinScale) {
          IncreaseScaleBy(-GameConfig.playerScaleDownFactorSP);
        }
      } else {
        if (currentLocalScale < GameConfig.playerMaxScale) {
          IncreaseScaleBy(GameConfig.playerScaleUpFactorSP);
        }
      }
      //transform.localScale += new Vector3 (scaleFactor  Time.deltaTime, scaleFactor  Time.deltaTime, 0);
    }

    UpdatePriests ();

    didCollideWithDeathZone = false;
  }

  public void DidCollideWithDeathZone(){
    didCollideWithDeathZone = true;
  }

  public void AddPriest(PriestObstacle priest){
    Priests.Add (priest);
  }

  public void MovePriest(PriestObstacle priest) {
    PriestsToMove.Add (priest);
  }

  private void UpdatePriests(){
    GameConfig gameConfig = GameController.Instance.GameConfig;

    if (GetScale () < gameConfig.priestConversionPlayerSize) {
      Debug.Log ("Minimum scale achieved - Adding priests as followers");

      Priests.ForEach (priest => {
        GameObject goFollower = GameObject.Instantiate (GameController.Instance.PrefabFollower);
        Follower follower = goFollower.GetComponent<Follower> () as Follower;

        follower.transform.position = priest.transform.position;

        AddFollower (follower, false);
        RegroupFollowers ();

        priest.DestroyPriest ();
      });

      Priests.Clear ();
      PriestsToMove.Clear ();
    } else {
      PriestsToMove.ForEach (priest => {
        priest.transform.position = transform.position;
      });
    }

    if (didCollideWithDeathZone) {
      if (Priests.Count > 0) {
        PriestObstacle priest = Priests [0];
        RemovePriest (priest);
      }
    }

    IncreaseScaleBy (gameConfig.priestScaleIncreasePerFollower * Followers.Count * Priests.Count);
  }

  private void RemovePriest(PriestObstacle priest){
    Priests.Remove (priest);

    if (PriestsToMove.Contains (priest)) {
      PriestsToMove.Remove (priest);
    }

    priest.DetachFromPlayer ();
  }

  public void IncreaseScaleBy(float scaleFactor) {
    transform.localScale += new Vector3(scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime, 0);
  }

  public void FixedUpdate() {
    if (IsMoving == false) {
      return;
    }

    float normedScale = 1 - (GetScale() * GameConfig.playerScaleImpact - GameConfig.playerMinScale) / GameConfig.playerMaxScale;
    float speedVariation = normedScale * (GameConfig.playerMaxVelocityDelta - GameConfig.playerMinVelocityDelta) + GameConfig.playerMinVelocityDelta;


    if (!(GetCameraX() + GameConfig.playerMaxCameraOffset > GetCurrentX() || speedVariation < GameConfig.cameraMovementSpeed)) {
      speedVariation = 0.0f;
    }

    bool tooFar = GetCameraX() + GameConfig.playerMaxCameraOffset < GetCurrentX();

    if (tooFar && speedVariation > 0) {
      speedVariation = 0.0f;
    }

    Vector2 velocity = rigidBody.velocity;
    velocity.x = GameConfig.cameraMovementSpeed + speedVariation;
    rigidBody.velocity = velocity;
  }
  
  public float GetScale() {
    return transform.localScale.x;
  }

  private float GetCameraX() {
    Vector2 cameraPosition = GameController.Instance.TransformLevelCamera.position;

    return cameraPosition.x;
  }

  private float GetCurrentX() {
    //Debug.Log ("Current x: " + transform.position.x);

    return transform.position.x;
  }

  public void AddFollower(Follower follower, bool snap = false) {
    string followerName = "follower_" + (Followers.Count + 1);
    Debug.Log("Adding follower");
    follower.name = followerName;
    Followers.Add(follower);

    GameObject goFollowerTransform = new GameObject("follower_mount") as GameObject;
    goFollowerTransform.transform.SetParent(transformFollowers);
    FollowerTransforms[follower] = goFollowerTransform.transform;

    follower.AttachToCircle(goFollowerTransform.transform, snap);
  }

  public void RemoveFollower(Follower follower) {
    Debug.Log("Removing follower " + follower.name);
    if(FollowerTransforms.ContainsKey(follower)) {
      GameObject.Destroy(FollowerTransforms[follower].gameObject);
      FollowerTransforms.Remove(follower);
      
      follower.RemoveFromCircle();
      Followers.Remove(follower);
    }
  }

  public void ClearFollowers() {
    Followers.ForEach(follower => GameObject.Destroy(follower.gameObject));
    Followers.Clear();
    foreach (var transform in FollowerTransforms.Values) {
       GameObject.Destroy(transform.gameObject);
    }
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

      Follower follower = Followers[i];

      FollowerTransforms[follower].name = "follower_mount_" + i;
      FollowerTransforms[follower].localPosition = new Vector2(x, y);
      FollowerTransforms[follower].rotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg);
    }

    Followers.ForEach(follower => {
      follower.RecreateLineRenderers();
      follower.MoveToPosition();
    });
  }

  public void PushForward() {
    transform.localScale = GameConfig.playerScaleAfterPush;
    Vector2 velocity = rigidBody.velocity;
    velocity.x = GameConfig.cameraMovementSpeed + GameConfig.playerPushVelocityDelta;
    rigidBody.velocity = velocity;
  }
}
