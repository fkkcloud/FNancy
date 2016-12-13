using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] level_musics;

	private AudioSource _musicPlayer;

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
		_musicPlayer = GetComponent<AudioSource> ();
	}

	void OnLevelWasLoaded(int level){

		/*
		AudioClip level_music = level_musics [level];
		if (level_music & _musicPlayer) {
			_musicPlayer.clip = level_music;
			_musicPlayer.loop = true;
			_musicPlayer.Play ();
		}
*/
	}

	public void SetVolume(float volume){
		_musicPlayer.volume = volume;
	}

	// Update is called once per frame
	void Update () {

	}

	public void Stop(){
		_musicPlayer.Stop ();
	}

	public bool IsPlaying()
	{
		return _musicPlayer.isPlaying;
	}

	public void Play(int level){
		AudioClip level_music = level_musics [level];
		if (level_music & _musicPlayer) {
			_musicPlayer.clip = level_music;
			_musicPlayer.loop = true;
			_musicPlayer.Play ();
		}
	}
}
