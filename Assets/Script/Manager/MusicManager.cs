using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public bool isSFXON; // used for sfx toggle in runtime

	public AudioClip[] level_musics;

	private AudioSource music_player;

	void Awake() {
		// have it work as singleton
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		music_player = GetComponent<AudioSource> ();
	}

	void OnLevelWasLoaded(int level){

		AudioClip level_music = level_musics [level];
		if (level_music & music_player) {
			music_player.clip = level_music;
			music_player.loop = true;
			music_player.Play ();
		}
	}

	public void SetVolume(float volume){
		music_player.volume = volume;
	}

	// Update is called once per frame
	void Update () {

	}
}
