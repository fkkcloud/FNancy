using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] level_musics;

	private AudioSource music_player;

	private static GameObject _instance;

	void Awake() {
		// have it work as singleton
		if (_instance) {
			DestroyImmediate (gameObject);
			return;
		}

		_instance = gameObject;
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

	public void Stop(){
		music_player.Stop ();
	}

	public bool IsPlaying()
	{
		return music_player.isPlaying;
	}

	public void Play(int level){
		AudioClip level_music = level_musics [level];
		if (level_music & music_player) {
			music_player.clip = level_music;
			music_player.loop = true;
			music_player.Play ();
		}
	}
}
