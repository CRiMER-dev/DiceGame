using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour {

	Vector3 diceVelocity;

	// Update is called once per frame
	void FixedUpdate () {
		diceVelocity = DiceScript.diceVelocity;
	}

	void OnTriggerStay(Collider col)
	{
		if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
		{
			string dice = col.gameObject.transform.parent.name;
			switch (col.gameObject.name) {
			case "Side1":
				//DiceNumberTextScript.diceNumber = 6;
				SetDiceNumber(dice, 6);
				break;
			case "Side2":
				//DiceNumberTextScript.diceNumber = 5;
				SetDiceNumber(dice, 5);
				break;
			case "Side3":
				SetDiceNumber(dice, 4);
				//DiceNumberTextScript.diceNumber = 4;
				break;
			case "Side4":
				SetDiceNumber(dice, 3);
				//DiceNumberTextScript.diceNumber = 3;
				break;
			case "Side5":
				SetDiceNumber(dice, 2);
				//DiceNumberTextScript.diceNumber = 2;
				break;
			case "Side6":
				SetDiceNumber(dice, 1);
				//DiceNumberTextScript.diceNumber = 1;
				break;
			}
		}
	}

	void SetDiceNumber(string dice, int num)
    {
		Debug.Log(dice + " : " + num);
		switch (dice)
        {
			case "dice1":
				DiceNumberTextScript.diceNumber1 = num;
				break;
			case "dice2":
				DiceNumberTextScript.diceNumber2 = num;
				break;
			case "dice3":
				DiceNumberTextScript.diceNumber3 = num;
				break;
			case "dice4":
				DiceNumberTextScript.diceNumber4 = num;
				break;
			case "dice5":
				DiceNumberTextScript.diceNumber5 = num;
				break;
		}
    }
}
