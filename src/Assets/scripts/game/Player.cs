using UnityEngine;

public class Player : MonoBehaviour {
	public Vector2 velocity;
	public float scaleDownFactor = 0.03f;
	public float scaleUpFactor = 0.01f;

	// Use this for initialization
	void Start () {
		velocity = new Vector2 (0.05f, 0.0f);
	}

	// Update is called once per frame
	void Update () {
		transform.Translate(velocity);
		if (Input.GetMouseButton (0)) {
			transform.localScale += new Vector3 (-scaleDownFactor, -scaleDownFactor, 0);
		} else {
			transform.localScale += new Vector3 (scaleUpFactor, scaleUpFactor, 0);
		}
	}
}
