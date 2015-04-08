using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoTextElement : MonoBehaviour {

	public Text label;
	public Text value;

	// Use this for initialization
	void Awake () {
		label = transform.FindChild ("Label").GetComponent<Text> ();
		value = transform.FindChild ("Value").GetComponent<Text> ();
	}
	
	public void SetLabel(string newLabel) {
		if(newLabel != null) 
			label.text = newLabel;
	}

	public void SetValue(string newValue) {
		if(newValue != null)
			value.text = newValue;
	}

}
