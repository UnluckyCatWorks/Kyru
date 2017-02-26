﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kyru.UI;

[Serializable]
public struct InputSettings
{
	public KeyCode[] keys;

	public bool GetKey ( Key key ) 
	{
		return Input.GetKey ( keys[( int ) key] );
	}

	public bool GetKeyDown ( Key key )
	{
		return Input.GetKeyDown ( keys[( int ) key] );
	}

	public void SetDefaults () 
    {
        keys = new KeyCode[]
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Space,
            KeyCode.LeftShift,
            KeyCode.R,
            KeyCode.Q,
            KeyCode.Mouse2,
            KeyCode.Mouse0,
            KeyCode.E
        };
    }
}

public enum Key 
{
	Forward,
	Backwards,
	Left,
	Right,
	Jump,
	Run,
	Sword,
	Boomerang,
	Lock,
	Attack,
	Interact
}

public class InputManager : MonoBehaviour
{
	#region UI
	public HotkeyButton[] hotkeys;
	#endregion

	HotkeyButton selected;

	public void LoadValues () 
	{
		for ( int k=0; k!=hotkeys.Length; k++ )
		{
			hotkeys[k].info.text = Game.input.keys[(int)hotkeys[k].key].ToString ();
		}
	}

	public void ApplySave () 
	{
		for ( int k = 0; k != hotkeys.Length; k++ )
		{
			Game.input.keys[(int)hotkeys[k].key] = ParseKey ( hotkeys[k].info.text );
		}

		PlayerPrefs.SetString ( "Input", JsonUtility.ToJson ( Game.input ) );
		PlayerPrefs.Save ();
	}

	private KeyCode ParseKey ( string key ) 
	{
		KeyCode code;
		code = ( KeyCode ) Enum.Parse ( typeof ( KeyCode ), key, true );
		return code;
	}

	public void SelectButton ( HotkeyButton button ) 
	{
		selected = button;
		button.esc.gameObject.SetActive ( true );
		button.info.gameObject.SetActive ( false );
	}

	void Update () 
	{
		if ( selected != null )
		{
			foreach ( KeyCode kcode in Enum.GetValues ( typeof ( KeyCode ) ) )
			{
				if ( Input.GetKeyDown ( kcode ) )
				{
					// Check if user pressed ESC or P ( pausing keys )
					if ( kcode != KeyCode.Escape && kcode != KeyCode.P )
					{
						// Save new value
						Game.input.keys[(int)selected.key] = kcode;

						// Show new value
						selected.info.text = kcode.ToString ();
					}

					// Stop waiting input
					selected.info.gameObject.SetActive ( true );
					selected.esc.gameObject.SetActive ( false );
					selected = null;
				}
			}
		}
	}
}
