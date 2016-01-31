using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

public class GameStatePlaying : AbstractState
{

	private Transform TransformLevelCamera { get; set; }

	private GameConfig GameConfig { get; set; }

	private int part = 0;

	private bool inSoundCoroutine = false;

	private SpriteRenderer backgroundSprite1 = GameObject.Find ("bg1").GetComponent<SpriteRenderer> ();
	private SpriteRenderer backgroundSprite2 = GameObject.Find ("bg2").GetComponent<SpriteRenderer> ();

	private AudioSource audio = GameObject.Find ("background_music").GetComponent<AudioSource> ();

	public GameStatePlaying (string stateName)
		: base (stateName)
	{
	}

	protected override void OnInitialize ()
	{
		GameConfig = GameController.Instance.GameConfig;
	}



	protected override void OnEnter (object onEnterParams = null)
	{

		//GameController.Instance.StartCoroutine (playEngineSound (0));
	    audio = GameObject.Find ("background_music").GetComponent<AudioSource> ();
		audio.Stop ();
		audio.loop = false;
		part = 0;

		// reset camera
		TransformLevelCamera = GameController.Instance.TransformLevelCamera;
		GameController.Instance.ResetCamera ();

		// spawn player
		SpawnPlayer ();

		// start level generation
		int seed = 1337; // TODO: use seed from onEnterParams
		GameController.Instance.LevelGenerator.CleanUp ();
		GameController.Instance.LevelGenerator.StartGenerating (seed);

		GameController.Instance.Background.Reset ();
		GameController.Instance.Background.Start ();

		GameController.Instance.RoundStartTime = Time.time;
		GameController.Instance.CurrentlyPlaying = true;
	}

	protected override void OnLeave ()
	{

		GameController.Instance.Background.Stop ();
		GameController.Instance.CurrentlyPlaying = false;
		GameController.Instance.Player.IsMoving = false;
		GameController.Instance.Player.DestroyAllPriests ();
	}

	protected override void OnUpdate ()
	{
		MoveCamera ();

		var player = GameController.Instance.Player;
		if (GameConfig.enableWinLoseConditions) {
			if (player.Followers.Count < GameConfig.followersMin) {
				GameController.Instance.ChangeState ("GameStateGameOver");
			} else if (player.Followers.Count >= GameConfig.followersToWin) {
				GameController.Instance.ChangeState ("GameStateLevelFinished");
			}
		}
 		

		//I don't regret the following part ... 
		//TODO add color-change
		//TODO fix first loop jump (audio file)
		if(player.Followers.Count <= GameConfig.changeMusicFollowerCount && part == 5) {
			Debug.Log ("1");
			part = 0;
		} else if(player.Followers.Count > GameConfig.changeMusicFollowerCount && part == 2){
			Debug.Log ("2");
			part = 3;
		}

		if (audio.isPlaying == false) {
			if (part == 0) {
				//backgroundSprite1.color = new Color (255,255,255,255);
				//backgroundSprite2.color = new Color (255,255,255,255);
				audio.clip = GameConfig.intro_choir_soft;
				part = 1;
			}
			else if (part == 1 || part == 2) {
				audio.clip = GameConfig.choir_loop;
				part = 2;
			}
			else if (part == 3) {
				//backgroundSprite1.color = new Color (226,51,51,255);
				//backgroundSprite2.color = new Color (226,51,51,255);
				audio.clip = GameConfig.intro_metal;
				part = 4;
			}
			else if (part == 4 || part == 5) {
				audio.clip = GameConfig.metal_loop;
				part = 5;
			}
			audio.Play ();
		}
			

	}

	private void MoveCamera ()
	{
		Vector3 camPos = TransformLevelCamera.position;
		camPos.x += GameConfig.cameraMovementSpeed * Time.deltaTime;
		TransformLevelCamera.position = camPos;
	}

	private void SpawnPlayer ()
	{
		GameObject goPlayer = GameObject.Instantiate (GameController.Instance.PrefabPlayer, Vector3.zero, Quaternion.identity) as GameObject;
		var player = goPlayer.GetComponent<Player> ();

		GameController.Instance.Player = player;

		// add initial followers
		for (int i = 0; i < GameConfig.followersMin; i++) {
			GameObject goFollower = GameObject.Instantiate (GameController.Instance.PrefabFollower) as GameObject;
			Follower follower = goFollower.GetComponent<Follower> ();
			player.AddFollower (follower, true);
		}
		player.RegroupFollowers ();
	}

	/*IEnumerator playEngineSound (int part)
	{
		GameObject.Find ("background_music").GetComponent<AudioSource> ().loop = true;
		//AudioSource audio = GameObject.Find ("background_music").GetComponent<AudioSource> ();
		inSoundCoroutine = true;

		if (part == 0) {
			audio.Stop ();
			audio.clip = GameConfig.intro_choir_soft;
			audio.Play ();
			yield return new WaitForSeconds (audio.clip.length - 0.3f);
			audio.clip = GameConfig.choir_loop;
			audio.Play ();
		}
		else if (part == 1) {
			//audio.Stop ();
			audio.Stop ();
			audio.clip = GameConfig.intro_metal;
			audio.Play ();
			yield return new WaitForSeconds (audio.clip.length - 0.3f);
			audio.clip = GameConfig.metal_loop;
			audio.Play ();
		}
		inSoundCoroutine = false;

	}*/
}
