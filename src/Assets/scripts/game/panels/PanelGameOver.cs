using System.Collections;
using UnityEngine;

public class PanelGameOver : AbstractPanelDeclaration {

  #region Inspector

  [SerializeField]
  private Transform transformClouds;

  #endregion

  protected override void OnEnter(object onEnterParams) {

    StartCoroutine(MoveClouds());

  }

  private IEnumerator MoveClouds() {

    // hack
    Vector3 start = transformClouds.position;
    Vector3 end = start;
    start.x = -1000;
    end.x = 1000;

    float timeTaken = 0;
    float duration = GameController.Instance.GameConfig.gameOverDuration;

    StartCoroutine(KillPlayerDelayed(duration * 0.5f));

    while (timeTaken < duration) {
      transformClouds.position = Vector3.Lerp(start, end, timeTaken / duration);

      timeTaken += Time.deltaTime;
      yield return null;
    }

    GameController.Instance.ChangeState("GameStateMenu");
  }

  private IEnumerator KillPlayerDelayed(float duration) {
    yield return new WaitForSeconds(duration);
    GameController.Instance.KillPlayer();
  }
}
