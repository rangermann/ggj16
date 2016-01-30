using UnityEngine;

public class GameConfig : MonoBehaviour {

  [Header("Camera")]
  public float cameraMovementSpeed = 5.0f;

  [Header("Player")]
  public float playerScaleDownFactor = 0.5f;
  public float playerScaleUpFactor = 0.7f;
  public float playerMinScale = 0.5f;
  public float playerMaxScale = 5.0f;
  public float playerMinVelocityFactor = -3f;
  public float playerMaxVelocityFactor = 0.4f;
}

