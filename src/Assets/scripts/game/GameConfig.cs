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
  public float playerPushVelocityDelta = 10;
  public Vector3 playerScaleAfterPush = Vector3.one;
  public float playerMaxCameraOffset = 4f;
  public float playerScaleImpact = 1.5f;
  public bool controlByPress = false;
  public AnimationCurve playerHellEffectSpinCurve;

  [Header("Followers")]
  public bool enableWinLoseConditions = true;
  public int followersMin = 3;
  public int followersToWin = 10;
  public float followersMovementDuration = 0.2f;
  public float followersLineMovementSpeed = 1f;
  public float playerHellFollowerInitialDelay = 4;
  public float playerHellFollowerDelay = 1;
  public float playerHellPortalInitialDelay = 4;
  
  public float playerHellPortalScaleDelay = 5;
  public float playerHellScaleUpSpeed = 1.0f;
  public float playerHellScaleUpDuration = 3;
  [Header("Priests")]
  public float priestMovementDuration = 1.0f;
  public float priestConversionPlayerSize = 0.6f;
  public float priestScaleIncreasePerFollower = 0.1f;

  [Header("Background")]
  public float backgroundWidth = 29.0f;
  public float backgroundScrollSpeed = 0.5f;

  [Header("Level Generator")]
  public float levelGeneratorSectionPadding = 5.0f;
  public int levelGeneratorTrapsPerSection = 2;
  public int levelGeneratorFollowersPerSection = 4;
  
  public int levelGeneratorPriestsPerSection = 2;


  [Header("Background Music")]
	public AudioClip intro_choir_soft; 
	public AudioClip choir_loop; 
	public AudioClip into_choir_hard; 
	public AudioClip intro_metal;
	public AudioClip metal_loop;
	public AudioClip title_loop;
	public AudioClip win_loop;
   //private AudioSource audio;

}

