using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Touchstuff : MonoBehaviour, IPointerDownHandler {

	int i;
	public Text score;
	public GameObject cut;

	#region IPointerClickHandler implementation
	public void OnPointerDown (PointerEventData eventData)
	{
		Vector3 worldPos;
		worldPos = Camera.main.ScreenToWorldPoint (eventData.position);
		worldPos.z = 1;
		Instantiate(cut, worldPos, Quaternion.Euler(0, 0, Random.Range (0, 360)));
		this.GetComponent<Animator>().SetBool("tapped", true);
		i += 1;
		score.text = i.ToString();
	}
	#endregion



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void TappedFase() {
		this.GetComponent<Animator>().SetBool("tapped", false);
	}

}