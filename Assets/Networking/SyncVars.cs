using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class SyncVars : MonoBehaviour {

	protected bool SyncAll = false;

	protected List<string> syncFields;

	protected void Awake () {
		if(!SyncAll) {
			FieldInfo[] fields = this.GetType ().GetFields ();
			foreach (FieldInfo field in fields) {
				if(field.GetCustomAttributes(typeof(SyncVarAttribute), false) != null) {

				}
			}
		}
	}




}


public class SyncVarAttribute : Attribute {

}