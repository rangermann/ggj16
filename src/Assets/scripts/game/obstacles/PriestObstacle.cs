using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PriestObstacle : AbstractObstacle {
  public bool IsMoving { get; private set; }
  private bool isDestroyed;
  public Transform attachedTo;

  public void Awake(){
    IsMoving = false;
    isDestroyed = false;
  }

  protected override void OnPlayerEnter() {
    Debug.Log("Priest entered player");
    
    if (IsMoving) {
      // ignore this hit!
      return;
    }
    
    // Detach from Level
    transform.parent = null;

    StartCoroutine(MoveToPlayerCenter(GetPlayerTransform()));
  }

  protected override void OnFollowerEnter(GameObject follower){
    Debug.Log("Follower hit follower");
  }


  private IEnumerator MoveToPlayerCenter(Transform transformInPlayer){
    attachedTo = null;
    while (IsMoving) {
      yield return null;
    }
    IsMoving = true;
    
    float duration = 2.0f;
    float timeTaken = 0.0f;

    var gameConfig = GameController.Instance.GameConfig;
    var startPosition = transform.position;
    Player player = GameController.Instance.Player;

    player.AddPriest (this);
    while (timeTaken < duration && !isDestroyed) {
      var currentPosition = Vector3.Lerp(startPosition, transformInPlayer.position, (timeTaken / duration));
      transform.position = currentPosition;

      timeTaken += Time.deltaTime;
      yield return null;
    }

    if (!isDestroyed) {
      player.MovePriest (this);
    } else {
      Debug.Log ("Will not add destroyed Priest");
    }

    IsMoving = false;
  }
  
  public void Update(){
    if (attachedTo != null) {
      transform.position = attachedTo.position;
    }
  }

  public void DetachFromPlayer(){
    attachedTo = null;
  }
    

  public void DestroyPriest(){
    isDestroyed = true;

    GameObject goExplosion = GameObject.Instantiate (GameController.Instance.PrefabExplosion1);
    goExplosion.transform.position = transform.position;

    GameObject.Destroy (transform.gameObject);
  }

  private Transform GetPlayerTransform(){
    return GameController.Instance.Player.transform;
  }
}
