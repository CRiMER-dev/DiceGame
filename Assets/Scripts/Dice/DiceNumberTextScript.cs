using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceNumberTextScript : MonoBehaviour {

	TextMeshProUGUI text;
	public static int diceNumber1;
	public static int diceNumber2;
	public static int diceNumber3;
	public static int diceNumber4;
	public static int diceNumber5;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMeshProUGUI> ();
	}

    // Update is called once per frame
    void Update() {
        text.text = diceNumber1.ToString() + "\n" + diceNumber2.ToString() + "\n" + diceNumber3.ToString()
			+ "\n" + diceNumber4.ToString() + "\n" + diceNumber5.ToString();
		//Debug.Log("TEXT");
	}
}
