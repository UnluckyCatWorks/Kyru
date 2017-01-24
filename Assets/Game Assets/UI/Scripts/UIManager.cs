﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// FX and UI controls
/// </summary>
public class UIManager : MonoBehaviour
{
	[Header ("Basic references")]
	public GameObject   globalParent;
	public GameObject	pauseMenu;
	public GameObject   pauseMenuMain;
	public Dropdown		language;
	public GameObject   pauseMenuOptions;
	public GameObject   pauseMenuGraphics;
	public GameObject   pauseMenuAudio;
	public GameObject   pauseMenuControls;
	public Image        pauseMenuBG;

	#region Pause Menu

	#region Graphics
	[Header("Graphics")]
	public Dropdown		resolutions;
	public Toggle       fullscreen;
	public Toggle		vsync;
	public Slider		textures;
	public Slider		postFX;
	public Slider		shadows;
	public Slider		FOV;
	public Toggle		antialiasing;
	#endregion

	#region AUDIO
	[Header ("Audio")]
	public AudioMixer audioMaster;
	public Slider master;
	public Slider music;
	public Slider sfx;
	public Slider ambient;
	public Slider voices;
	#endregion

	#region Controls
	[Header("Controls")]
	public Text[] hotkeys;
	#endregion

	#region FX
	public void UpdateInfoText ( Text info )
	{
		// IDs of quality info texts
		// for translation.
		string[] infoRef =
		{
			"pause_menu:low",
			"pause_menu:medium",
			"pause_menu:high",
			"pause_menu:veryhigh"
		};
		var value = ( int ) info.transform.parent.GetComponent<Slider> ().value;
		info.text = Localizator.GetText ( infoRef[value] );
	}

	public void UpdateInfoInt ( Text info )
	{
		//if ( !info.gameObject.activeInHierarchy ) return;

		var value = ( int ) info.transform.parent.GetComponent<Slider> ().value;

		if ( info.transform.parent.name == "SLD_fov" )	info.text = value.ToString () + " º";
		else											info.text = value.ToString () + " %";
	}

	public void ChangeLang ( int newLang )
	{
		string[] availableLangs =
		{
			"en",	// English
			"es",	// Spanish
			"cat"	// Catalan
		};
		Localizator.LoadLang ( availableLangs[newLang] );
		Localizator.UpdateAll ();

		// Also update Sliders' info
		Game.ui.textures.onValueChanged.Invoke ( Game.ui.textures.value );
		Game.ui.postFX.onValueChanged.Invoke ( Game.ui.postFX.value );
		Game.ui.shadows.onValueChanged.Invoke ( Game.ui.shadows.value );

		// Save
		PlayerPrefs.SetInt ( "Lang", newLang );
		PlayerPrefs.Save ();
	}

	public void Resume ()
	{
		Game.pause.StartCoroutine ( "SwitchPause" );
	}
	
	public void QuitToMainMenu ()
	{
		throw new System.NotImplementedException ( "FALTA MAIN MENU LOL" );
	}
	#endregion

	#endregion

	#region Main Menu

	#region FX
	public void Play ()
	{
		throw new System.NotImplementedException ( "A DONDE SE JUEA ESTO=X? HAHA" );
	}

	public void Exit ()
	{
		Application.Quit ();
	}
	#endregion

	#endregion
}
