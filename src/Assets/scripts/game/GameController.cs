﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameController : MonoBehaviour {

  #region Inspector

  [SerializeField]
  private GameObject prefabPlayer;

  [SerializeField]
  private GameObject prefabFollower;

  [SerializeField]
  private GameObject prefabFollowerObstacle;

  [SerializeField]
  private GameObject prefabTrapObstacle;

  [SerializeField]
  private Transform transformLevelCamera;

  [SerializeField]
  private GameConfig gameConfig;

  [SerializeField]
  private LevelGenerator levelGenerator;

  [SerializeField]
  private GameObject prefabFollowerLineRenderer;
  
  [SerializeField]
  private GameObject prefabPriestObstacle;

  [SerializeField]
  private GameObject prefabExplosion1;

  [SerializeField]
  private Background background;

  [SerializeField]
  private GameObject startScreen;

  [SerializeField]
  private GameObject endScreen;


  [SerializeField]
  private GameObject prefabHellEffect;

  #endregion

  //private static GameController instance;

  public static GameController Instance {
    get {
      return instance;
    }
  }

  private StateMachine StateMachine { get; set; }


  public float RoundStartTime { get; set; }

  public bool CurrentlyPlaying { get; set; }


  public GameObject PrefabPlayer {
    get {
      return prefabPlayer;
    }
  }

  public GameObject PrefabFollower {
    get {
      return prefabFollower;
    }
  }

  public Transform TransformLevelCamera {
    get {
      return transformLevelCamera;
    }
  }

  public GameConfig GameConfig {
    get {
      return gameConfig;
    }
  }

  public LevelGenerator LevelGenerator {
    get {
      return levelGenerator;
    }
  }

  public Player Player {
    get;
    set;
  }

  public GameObject PrefabFollowerLineRenderer {
    get {
      return prefabFollowerLineRenderer;
    }
  }

  public GameObject PrefabFollowerObstacle {
    get {
      return prefabFollowerObstacle;
    }
  }

  public GameObject PrefabTrapObstacle {
    get {
      return prefabTrapObstacle;
    }
  }
  
  public GameObject PrefabPriestObstacle {
    get {
      return prefabPriestObstacle;
    }
  }

  public GameObject PrefabExplosion1 {
    get {
      return prefabExplosion1;
    }
  }

  public Background Background {
    get {
      return background;
    }
  }

	public GameObject StartScreen {
		get {
			return startScreen;
		}
	}

	public GameObject EndScreen {
		get {
			return endScreen;
		}
	}


  public GameObject PrefabHellEffect {
    get {
      return prefabHellEffect;
    }
  }

  public void Awake() {
    instance = this;
    DontDestroyOnLoad(transform.parent.gameObject);

    List<AbstractState> states = new List<AbstractState>();
    states.Add(new GameStateLoading("GameStateLoading"));
    states.Add(new GameStatePlaying("GameStatePlaying"));
    states.Add(new GameStateGameOver("GameStateGameOver"));
    states.Add(new GameStateLevelFinished("GameStateLevelFinished"));
    // add more game states here

	StateMachine = StateMachine.Create("state_machine", states, "GameStateLoading");
  }

  public void ChangeState(string stateName, object onEnterParams = null) {
    StateMachine.ChangeState(stateName, onEnterParams);
  }

  public void OnGUI() {
#if UNITY_EDITOR
    GUILayout.Label("Current state: " + StateMachine.CurrentStateName);
#endif
  }


  public CameraBounds GetCameraXBounds() {
    Camera mainCamera = Camera.main;
    Vector3 cameraPosition = GameController.Instance.TransformLevelCamera.position;

    float xDist = mainCamera.aspect * mainCamera.orthographicSize;

    return new CameraBounds (cameraPosition.x - xDist, cameraPosition.x + xDist);
  }
}

