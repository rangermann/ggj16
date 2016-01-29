using UnityEngine;

public class Player : MonoBehaviour {

	public Vector2 velocity;

  private GameConfig GameConfig { get; set; }

  public void Awake() {
    GameConfig = GameController.Instance.GameConfig;
  }

	// Update is called once per frame
	public void Update () {
    Vector3 translation = new Vector3(GameConfig.cameraMovementSpeed * Time.deltaTime, 0, 0);
    transform.Translate(translation);
		if (Input.GetMouseButton (0)) {
      transform.localScale += new Vector3(-GameConfig.playerScaleDownFactor, -GameConfig.playerScaleDownFactor, 0);
		} else {
      transform.localScale += new Vector3(GameConfig.playerScaleUpFactor, GameConfig.playerScaleUpFactor, 0);
		}
	}
}
