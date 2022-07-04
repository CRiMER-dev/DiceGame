﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceNumberTextScript : MonoBehaviour {

	TextMeshProUGUI text;
	public static int diceNumber;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMeshProUGUI> ();
	}

    // Update is called once per frame
    void Update() {
        text.text = diceNumber.ToString();
	}
}
