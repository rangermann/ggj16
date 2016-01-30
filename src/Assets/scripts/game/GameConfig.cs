using UnityEngine;

public class GameConfig : MonoBehaviour {

  [Header("Camera")]
  public float cameraMovementSpeed = 5.0f;

  [Header("Player")]
  public float playerScaleDownFactor = 0.5f; //0.5f for pressing control; else 8 
  public float playerScaleUpFactor = 0.7f; //0.7f for pressing control; else 0..3f 
  public float playerScaleDownFactorSP = 8f; //for the single pressing opt
  public float playerScaleUpFactorSP = 0.4f; //for the single pressing opt 
  public float playerMinScale = 0.5f;
  public float playerMaxScale = 5.0f;
  public float playerMinVelocityDelta = -3f;
  public float playerMaxVelocityDelta = 0.4f;
  public float playerMaxCameraOffset = 4f;
  public float playerScaleImpact = 1.5f;
  public bool controlByPress = false;

  [Header("Followers")]
  public bool enableWinLoseConditions = true;
  public int followersMin = 3;
  public int followersToWin = 10;
  public float followersMovementSpeed = 2;
  
}

