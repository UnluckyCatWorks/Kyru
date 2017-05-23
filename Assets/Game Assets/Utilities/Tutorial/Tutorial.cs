﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using Kyru.UI;
using Kyru.etc;

public class Tutorial : MonoBehaviour
{
	// Public
	public Collider placeta;
	public Animation square;
	public ParticleSystem fog;
	public Color targetAmbient;
	public Vector3 squarePos;
	public GameObject water;
	public GameObject[] firstWave;
	public GameObject[] secondWave;
	public Color normalAmbient;
	public RangedController firstRanged;
	public Animator cupula;
	public Color cupulaAmbient;
	public RangedController[] firstRangeds;
	public RangedController[] secondRangeds;
	public RangedController[] thirdRangeds;

	// private
	Text tutoText;

	IEnumerator StartTuto ( Collider col ) 
	{
		yield return null;
	}

	IEnumerator ClosePlaceta ( Collider col ) 
	{
		Destroy ( col.gameObject );

		// Turn down ambient
		StartCoroutine ( this.AsyncLerp<RenderSettings> ( "ambientLight", targetAmbient, 4f ) );

		// Close placeta
		placeta.enabled = true;

		// Sword tuto
		Game.dave.canCombat = true;
		Game.dave.canMove =  false;
		Game.dave.Moving = false;
		yield return NewTuto ( AllTexts.Tuto_R_To_Unsheathe, Key.Sword );

		// Close 
		square.Play ( "Fade" );
		yield return new WaitForSeconds ( 0.8f );
		fog.Play ();
		water.SetActive ( false );

		// Return Dave control
		Game.dave.canMove = true;
		StartCoroutine ( NewTuto ( AllTexts.Tuto_Click_To_Attack, Key.Attack_single ) );

		yield return new WaitForSeconds ( 6f );
		square.Play ( "Loop" );
	}

	IEnumerator KilledFirst ()
	{
		yield return new WaitForSeconds ( 1f );
		foreach (var m in firstWave) m.SetActive ( true );
		yield return new WaitForSeconds ( 0.5f );
		StartCoroutine(  NewTuto ( AllTexts.Tuto_Click_To_Attack_Big, Key.Attack_big ) );
	}

	int firstWaveCount;
	IEnumerator FirstWave () 
	{
		if ( ++firstWaveCount == firstWave.Length )
		{
			yield return new WaitForSeconds ( 2f );
			foreach (var m in secondWave) m.SetActive ( true );
		}
	}

	int secondWaveCount;
	IEnumerator SecondWave ()
	{
		if ( ++secondWaveCount == secondWave.Length )
		{
			yield return new WaitForSeconds ( 2f );
			placeta.enabled = false;
			square["Fade"].speed = -2;
			square["Fade"].time = square["Fade"].length;
			square.Play ( "Fade" );
			yield return new WaitForSeconds ( 1f );
			water.SetActive ( true );
			fog.Stop ( false, ParticleSystemStopBehavior.StopEmitting );
			StartCoroutine ( this.AsyncLerp<RenderSettings> ( "ambientLight", normalAmbient, 4f ) );
		}
	}

	IEnumerator Ranged ( Collider col ) 
	{
		col.enabled = false;
		if ( !Game.dave.SwordOut )
		{
			Game.dave.canMove = false;
			Game.dave.Moving = false;
			yield return NewTuto ( AllTexts.Tuto_R_To_Unsheathe, Key.Sword );
			yield return new WaitForSeconds ( 1f );
		}

		Game.dave.canShoot = true;
		yield return NewTuto ( AllTexts.Tuto_Charge_And_Shot, Key.Charge );
		Game.dave.canMove = true;
		firstRanged.Activate ();
	}

	IEnumerator StartCupula ( Collider col )
	{
		col.enabled = false;

		cupula.SetTrigger ( "Close" );
		yield return new WaitForSeconds ( 3.5f );
		cupula.SetTrigger ( "FireOn" );
		StartCoroutine ( this.AsyncLerp<RenderSettings> ( "ambientLight", cupulaAmbient, 2.5f ) );
		yield return new WaitForSeconds ( 3f );
		firstRangeds[0].Activate ();
		yield return new WaitUntil ( () => !firstRangeds[0].active );
		cupula.SetTrigger ( "KilledRangeds_1" );
		foreach (var r in secondRangeds) r.Activate ();
		yield return new WaitUntil ( () => secondRangeds.All ( x => !x.active ) );
		cupula.SetTrigger ( "KilledRangeds_2" );
		foreach (var r in thirdRangeds) r.Activate ();
		yield return new WaitUntil ( () => thirdRangeds.All ( x => !x.active ) );
		cupula.SetTrigger ( "KilledRangeds_3" );
		StartCoroutine ( this.AsyncLerp<RenderSettings> ( "ambientLight", normalAmbient, 4f ) );
	}

	IEnumerator NewTuto ( AllTexts txt, Key key ) 
	{
		Game.ui.SetTrigger ( "NewTuto" );
		tutoText.text = Localization.GetText ( txt );
		yield return new WaitUntil
		( () =>
		{
			if ( key == Key.Attack_big || key == Key.Attack_single )
			{
				return
					Game.dave.SwordOut
					&& Game.input.GetKeyDown
					( Key.Attack_big ) || Game.input.GetKeyDown ( Key.Attack_single );
			}
			else return Game.input.GetKeyDown ( key );
		});

		Game.ui.SetTrigger ( "TutoOver" );
	}

	void Start () 
	{
		//ProceduralMaterial.substanceProcessorUsage = ProceduralProcessorUsage.All;
		tutoText = GameObject.Find ( "TXT_Tuto" ).GetComponent<Text> ();
	}
}
