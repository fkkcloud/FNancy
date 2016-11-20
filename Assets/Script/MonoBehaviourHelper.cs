using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourHelper : MonoBehaviour 
{
	private GameState _gameState;
	public GameState gameState
	{
		get
		{
			if (_gameState == null)
				_gameState = FindObjectOfType<GameState> ();

			return _gameState;
		}
	}

	private GameCharacter _gameCharacter;
	public GameCharacter gameCharacter
	{
		get
		{
			if (_gameCharacter == null)
				_gameCharacter = FindObjectOfType<GameCharacter> ();

			return _gameCharacter;
		}
	}

	private GameDesignVariables _gameDesignVariables;
	public GameDesignVariables gameDesignVariables
	{
		get
		{
			if (_gameDesignVariables == null)
				_gameDesignVariables = FindObjectOfType<GameDesignVariables> ();

			return _gameDesignVariables;
		}
	}

	private BarAnimator _barAnimator;
	public BarAnimator barAnimator
	{
		get
		{
			if (_barAnimator == null)
				_barAnimator = FindObjectOfType<BarAnimator> ();

			return _barAnimator;
		}
	}

	private MusicManager _musicManager;
	public MusicManager musicManager
	{
		get
		{
			if (_musicManager == null)
				_musicManager = FindObjectOfType<MusicManager> ();

			return _musicManager;
		}
	}

	private StageManager _stageManager;
	public StageManager stageManager
	{
		get
		{
			if (_stageManager == null)
				_stageManager = FindObjectOfType<StageManager> ();

			return _stageManager;
		}
	}

	public Stage currentStage
	{
		get
		{
			return stageManager.GetCurrentStage ();
		}
	}

	public Stage previousStage
	{
		get
		{
			return stageManager.GetPreviousStage ();
		}
	}

	private GameModeClicker _gameModeClicker;
	public GameModeClicker gameModeClicker
	{
		get
		{
			if (_gameModeClicker == null)
				_gameModeClicker = new GameModeClicker ();

			return _gameModeClicker;
		}
	}

	private GameModeTimer _gameModeTimer;
	public GameModeTimer gameModeTimer
	{
		get
		{
			if (_gameModeTimer == null)
				_gameModeTimer = new GameModeTimer ();

			return _gameModeTimer;
		}
	}

	private GameModeSlider _gameModeSlider;
	public GameModeSlider gameModeSlider
	{
		get
		{
			if (_gameModeSlider == null)
				_gameModeSlider = new GameModeSlider ();

			return _gameModeSlider;
		}
	}
}