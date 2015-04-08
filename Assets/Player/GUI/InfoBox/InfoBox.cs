using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour {

	public GameObject prefabTextElement;

	private string titleText;
	private Texture2D icon;

	private Text titleObject;

	private Transform infoPanel;

	public Dictionary<string, InfoTextElement> activeElements = new Dictionary<string, InfoTextElement> {};

	// Use this for initialization
	void Start () {
		titleObject = GetComponentInChildren<Text> ();
		infoPanel = transform.FindChild ("InfoPanel").transform;
	}

	public void SetTitle (string title) {
		titleObject.text = title;
	}

	public void AppendElement(InfoTextElement element) {
		element.transform.SetParent (infoPanel.transform);
		//activeElements.Add (element);
	}

	// Take a list of key value pairs and then create a Info Panel instance for each
	public void AppendTextElements(List<List<string>> elements) {
		foreach (var each in elements) {
			GameObject uiElement = (GameObject)Instantiate(prefabTextElement);
			InfoTextElement infoTextManager = uiElement.GetComponent<InfoTextElement> ();
			uiElement.transform.SetParent(infoPanel.transform);
			infoTextManager.SetLabel(each[0]);
			infoTextManager.SetValue(each[1]);

			activeElements.Add(each[0], infoTextManager);
		}
	}

	public void UpdateValue(string key, string value) {
		activeElements [key].SetValue (value);
	}

	public void Clear() {
		var children = new List<GameObject>();
		foreach (Transform child in infoPanel.transform) 
			if(child.name !="Filter" && child.name != "InfoTitle")
				children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

		activeElements.Clear ();
		titleObject.text = "";
	}

	public void HideBox () {
		this.enabled = false;
	}
}
