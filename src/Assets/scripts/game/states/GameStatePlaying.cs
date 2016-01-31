using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class GameStatePlaying : AbstractState {

  private Transform TransformLevelCamera { get; set; }

  private GameConfig GameConfig { get; set; }

  private int internalFollowerCount = 0;  

	public GameStatePlaying(string stateName)
    : base(stateName) {
  }

  protected override void OnInitialize() {
    GameConfig = GameController.Instance.GameConfig;
  }


	IEnumerator playEngineSound()
	{
		GameObject.Find ("background_music").GetComponent<AudioSource> ().loop = true;
		 
			AudioSource audio = GameObject.Find ("background_music").GetComponent<AudioSource> ();

			if(internalFollowerCount<7) {

			AudioClip clip = GameConfig.intro_choir_soft;
			audio.Stop ();
			audio.clip = clip;
			audio.Play ();
			yield return new WaitForSeconds (audio.clip.length - 0.3f);
			audio.clip = GameConfig.choir_loop;
			audio.Play ();
		}
			else if(internalFollowerCount>7){
			
				//audio.Stop ();
			    audio.clip = GameConfig.intro_metal;
				audio.Play ();
				yield return new WaitForSeconds (audio.clip.length - 0.3f);
				audio.clip = GameConfig.metal_loop;
				audio.Play ();
			
			}

	}

  protected override void OnEnter(object onEnterParams = null) {
	internalFollowerCount = 3; 

	GameController.Instance.StartCoroutine(playEngineSound());

    // reset camera
    TransformLevelCamera = GameController.Instance.TransformLevelCamera;
    Vector3 camPos = TransformLevelCamera.position;
    camPos.x = 0;
    TransformLevelCamera.position = camPos;

    // spawn player
    SpawnPlayer();

    // start level generation
    int seed = 1337; // TODO: use seed from onEnterParams
    GameController.Instance.LevelGenerator.CleanUp();
    GameController.Instance.LevelGenerator.StartGenerating(seed);

    GameController.Instance.Background.Reset ();
    GameController.Instance.Background.Start ();

  	GameController.Instance.RoundStartTime = Time.time;
	  GameController.Instance.CurrentlyPlaying = true;
  }

  protected override void OnLeave() {

	internalFollowerCount = 3;
    GameController.Instance.Background.Stop ();
	  GameController.Instance.CurrentlyPlaying = false;
    GameController.Instance.Player.IsMoving = false;
    GameController.Instance.Player.DestroyAllPriests ();
  }

  protected override void OnUpdate() {
    MoveCamera ();

		var player = GameController.Instance.Player;
		internalFollowerCount = player.Followers.Count;
    if (GameConfig.enableWinLoseConditions) {
      if (player.Followers.Count < GameConfig.followersMin) {
        GameController.Instance.ChangeState("GameStateGameOver");
      } else if (player.Followers.Count >= GameConfig.followersToWin) {
        GameController.Instance.ChangeState("GameStateLevelFinished");
      }
    }
  }

  private void MoveCamera() {
    Vector3 camPos = TransformLevelCamera.position;
    camPos.x += GameConfig.cameraMovementSpeed * Time.deltaTime;
    TransformLevelCamera.position = camPos;
  }

  private void SpawnPlayer() {
    GameObject goPlayer = GameObject.Instantiate(GameController.Instance.PrefabPlayer, Vector3.zero, Quaternion.identity) as GameObject;
    var player = goPlayer.GetComponent<Player>();

    GameController.Instance.Player = player;

    // add initial followers
    for (int i = 0; i < GameConfig.followersMin; i++) {
      GameObject goFollower = GameObject.Instantiate(GameController.Instance.PrefabFollower) as GameObject;
      Follower follower = goFollower.GetComponent<Follower>();
      player.AddFollower(follower, true);
    }
    player.RegroupFollowers();
  }
}
