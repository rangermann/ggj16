using UnityEngine;

public class GameConfig : MonoBehaviour {

  [Header("Camera")]
  public float cameraMovementSpeed = 5.0f;

  [Header("Player")]
  public float playerScaleDownFactor = 0.03f;
  public float playerScaleUpFactor = 0.01f;
  public float playerMinScale = 0.1f;
  public float playerMaxScale = 5.0f;
}
