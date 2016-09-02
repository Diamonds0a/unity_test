using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BananaHandler : MonoBehaviour {

	int bps;
	public Text bpsText;
	public Button buyBPS;

	// Use this for initialization
	void Start () {
		bps = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void moreBananas() {
		bps += 10;
		bpsText.text = "BPS: " + bps.ToString ();
		if (bps > 30) {
			buyBPS.interactable = false;
		}
	}
}
