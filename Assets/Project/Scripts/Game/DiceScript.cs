using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour {

	static Rigidbody rb;
	private Transform thisTransfom;
	private GameObject obj;

	private Vector3 oldPosition;
	private Quaternion oldRotation;

	public static Vector3 diceVelocity;
	[SerializeField] private int diceCount = 0;
	[SerializeField] private bool active = false;
	[SerializeField] private Game game;
	public int GetCount() { return diceCount; }
	public bool GetActive() { return active; }
	public void SetActive(bool value) { active = value; }
	//public void SetActive(bool value) { gameObject.SetActive(value); }
	public bool IsSleep() { return rb.IsSleeping(); }

	private Vector3 start_position;
	private bool calc_count;
	public void SetCalcCount(bool value) { calc_count = value; }

	// Use this for initialization
	private void Awake () {
		rb = GetComponent<Rigidbody> ();
		thisTransfom = GetComponent<Transform>();
		obj = gameObject;
	}

    private void Start()
    {
		start_position = thisTransfom.position;
		calc_count = false;
		obj.SetActive(false);
	}

    public void Reset()
    {
		thisTransfom.position = start_position;
		thisTransfom.rotation = Quaternion.identity;
		
		active = true;
	}

	public void RegularDiceCount()
	{
		if (!active)
			return;

		if (Vector3.Dot(thisTransfom.forward, Vector3.up) > 0.6f)
			diceCount = 4;
		if (Vector3.Dot(-thisTransfom.forward, Vector3.up) > 0.6f)
			diceCount = 3;
		if (Vector3.Dot(thisTransfom.up, Vector3.up) > 0.6f)
			diceCount = 6;
		if (Vector3.Dot(-thisTransfom.up, Vector3.up) > 0.6f)
			diceCount = 1;
		if (Vector3.Dot(thisTransfom.right, Vector3.up) > 0.6f)
			diceCount = 2;
		if (Vector3.Dot(-thisTransfom.right, Vector3.up) > 0.6f)
			diceCount = 5;
	}

	public void Throw()
    {
		if (!active)
			return;

		obj.SetActive(true);

		//diceVelocity = rb.velocity;
		float dirX = Random.Range(0, 500);
		float dirY = Random.Range(0, 500);
		float dirZ = Random.Range(0, 500);
		
		thisTransfom.rotation = Random.rotation;
		//rb.AddForce(transform.up * 500);
		//rb.AddTorque(dirX, dirY, dirZ);

		//Debug.Log("SPACE");

		diceCount = 0;

		oldPosition = thisTransfom.position;
		oldRotation = thisTransfom.rotation;
	}

    public bool Check()
    {
        if (!active)
            return true;

        bool sleep = false;

        if (thisTransfom.position == oldPosition && thisTransfom.rotation == oldRotation)
        {
            sleep = true;
        }
        else
        {
            oldPosition = thisTransfom.position;
            oldRotation = thisTransfom.rotation;
        }

        return sleep;
    }
}
