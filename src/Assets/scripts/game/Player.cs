using UnityEngine;

public class Player : MonoBehaviour {

	public Vector2 velocity;
	private Rigidbody2D rigidBody;

  private GameConfig GameConfig { get; set; }

  public void Awake() {
    GameConfig = GameController.Instance.GameConfig;
	rigidBody = GetComponent<Rigidbody2D>();
  }

	// Update is called once per frame
	public void Update () {
		float currentLocalScale = GetScale();
		bool scaleUp = !Input.GetMouseButton (0);

		float scaleFactor = 0;
	
		if (!scaleUp && currentLocalScale > GameConfig.playerMinScale) {
			scaleFactor = -GameConfig.playerScaleDownFactor;
		} else if (scaleUp && currentLocalScale < GameConfig.playerMaxScale) {
			scaleFactor = GameConfig.playerScaleUpFactor;
		}

		transform.localScale += new Vector3(scaleFactor * Time.deltaTime, scaleFactor * Time.deltaTime, 0);
	}

	public void FixedUpdate (){
    float speedVariation, normedScale;
    if (GetReferenceX() + GameConfig.playerMaxCameraOffset > GetCurrentX()) {
      normedScale = 1 - (GetScale () * GameConfig.playerScaleImpact - GameConfig.playerMinScale) / GameConfig.playerMaxScale;
      speedVariation = normedScale * (GameConfig.playerMaxVelocityDelta - GameConfig.playerMinVelocityDelta) + GameConfig.playerMinVelocityDelta;
    } else {
      speedVariation = 0f;
    }


		Vector2 velocity = rigidBody.velocity;
		velocity.x = GameConfig.cameraMovementSpeed + speedVariation;
		rigidBody.velocity = velocity;
	}

  private float GetReferenceX() {
    Vector2 cameraPosition = GameController.Instance.TransformLevelCamera.position;

    Debug.Log ("Current cPos: " + cameraPosition.x);

    return cameraPosition.x;
  }

	private float GetScale(){
		return transform.localScale.x;
	}

  private float GetCurrentX() {
    Debug.Log ("Current x: " + transform.position.x);

    return transform.position.x;
  }
}
