using UnityEngine;
using System.Collections;

public class CloudsAnim : MonoBehaviour {

  public float speedX = 1.3f;
  public float speedY = 1.5f;

  public float distX = 0.7f;
  public float distY = 0.5f;

  private Vector3 startPos;
  private float time;
  private bool initialized = false;

	// Use this for initialization
	void Start () {
    time = 0;
	}

	// Update is called once per frame
	void Update () {
    if (!initialized) {
      startPos = transform.localPosition;
      initialized = true;
    }

    transform.localPosition = new Vector2(startPos.x + distX * Mathf.Sin(time * speedX), startPos.y + distY * Mathf.Sin(time * speedY));

    time += Time.deltaTime;
  }
}
