using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
  private float width;
  private float scrollSpeed;
  private bool moving;

  public void Awake() {
    GameConfig gameConfig = GameController.Instance.GameConfig;
    width = gameConfig.backgroundWidth;
    scrollSpeed = gameConfig.backgroundScrollSpeed;
  }

	public void Start () {
    moving = true;
	}

  public void Stop () {
    moving = false;
  }

  public void Reset() {
    CameraBounds bounds = GameController.Instance.GetCameraXBounds ();

    transform.position = new Vector2 (bounds.start, 0.0f);
  }
	
	public void Update () {
    if (moving) {
      CameraBounds bounds = GameController.Instance.GetCameraXBounds();

      transform.Translate (new Vector2 (scrollSpeed * Time.deltaTime, 0.0f));

      if (bounds.start - transform.position.x > width) {
        transform.Translate (new Vector2 (width, 0.0f));
      }
    }
	}
}
