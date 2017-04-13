﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SwordSubanceParams : MonoBehaviour
{
	public MeshRenderer sword;
	private ProceduralMaterial mat;

	[Header("Params")]
	[Range (0,1)] public float levelInLow;
	[Range (0,1)] public float levelInHigh;

	private void Update ()
	{
		mat.SetProceduralFloat ( "levelinlow", levelInLow );
		mat.SetProceduralFloat ( "levelinhigh", levelInHigh );
		mat.RebuildTextures ();
	}

	private void OnEnable ()
	{
		mat = sword.sharedMaterial as ProceduralMaterial;
	}
}
