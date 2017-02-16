﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kyru.UI
{
	public class LangSelector : MonoBehaviour
	{
		private void Start () 
		{
			GetComponent<Dropdown> ().value = Localization.lang;
		}

		public void ChangeLang ( int lang ) 
		{
			Localization.lang = lang;
			Localization.LoadTexts ();
			Localization.UpdateAllTexts ();
		}
	} 
}
