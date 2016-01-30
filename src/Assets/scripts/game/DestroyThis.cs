using UnityEngine;
using System.Collections;

public class DestroyThis : MonoBehaviour {

	public float waitSecond = 1.7f;
	public AnimationClip anim;

	// Use this for initialization
	void Start () {
		StartCoroutine (KillOnAnimationEnd ());
	}


	private IEnumerator KillOnAnimationEnd() {

		if (anim != null) {
			waitSecond = anim.length;
		}
		yield return new WaitForSeconds (waitSecond);
		Destroy (this.gameObject);
	}

	void Update () {
		//StartCoroutine (KillOnAnimationEnd ());
	}

}
