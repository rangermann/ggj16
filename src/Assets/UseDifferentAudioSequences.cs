using UnityEngine;
using System.Collections;

public class UseDifferentAudioSequences : MonoBehaviour {

	public AudioClip intro_choir_soft, choir_loop, into_choir_hard, intro_metal, metal_loop, title_loop ;
	private AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		//audio.Stop();
		audio.clip = intro_choir_soft;
		audio.Play();
		/*switch (objectStatus.GetState())
		{
		case 1: //Intro        
			audio.Stop();
			a_source.audio.clip = intro_choir_soft;
			audio.Play();
			break;

		case 2: //Trot
			//a_source.audio.clip = au_trot;
			//audio.Play();
			break;

		case 3: //Gallop              
			a_source.audio.clip = au_gallop;
			audio.Play();
			break;
		}*/
	}
}
