using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour {

	private Rigidbody rb;
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
	private Vector3[] inactive_pos = new Vector3[5];
	private Vector3[] inactive_rotate = new Vector3[6];
	private Camera _mainCamera;
	private float _cameraZDistance;
	public bool MoveMouse;

	// Use this for initialization
	private void Awake () {
		rb = GetComponent<Rigidbody> ();
		thisTransfom = GetComponent<Transform>();
		obj = gameObject;
		_mainCamera = Camera.main;
		_cameraZDistance = _mainCamera.WorldToScreenPoint(thisTransfom.position).z;
	}

    private void Start()
    {
		start_position = thisTransfom.position;
		calc_count = false;
		obj.SetActive(false);

		inactive_pos[0] = new Vector3(-2.6f, 1.6f, 5.65f);
		inactive_pos[1] = new Vector3(-1.2f, 1.6f, 5.65f);
		inactive_pos[2] = new Vector3(0.2f, 1.6f, 5.65f);
		inactive_pos[3] = new Vector3(1.6f, 1.6f, 5.65f);
		inactive_pos[4] = new Vector3(3.0f, 1.6f, 5.65f);

		inactive_rotate[0] = new Vector3(-180, 0, 0);
		inactive_rotate[1] = new Vector3(0, 0, 90);
		inactive_rotate[2] = new Vector3(90, 0, 0);
		inactive_rotate[3] = new Vector3(-90, 0, 0);
		inactive_rotate[4] = new Vector3(0, 0, -90);
		inactive_rotate[5] = new Vector3(0, 0, 0);
	}

    private void Update()
    {
		return;
		/*if (!active)
			return;

		if (MoveMouse)
        {
			Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraZDistance);
			//Vector3 screenPosition = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), _cameraZDistance);

			Vector3 newWorldPosition = _mainCamera.ScreenToWorldPoint(screenPosition);

			thisTransfom.position = newWorldPosition + start_position;
		}*/
    }

    public void Reset()
    {
		thisTransfom.position = start_position;
		thisTransfom.rotation = Quaternion.identity;
		
		active = true;
		diceCount = 0;
		MoveMouse = true;

		rb.isKinematic = false;

		obj.SetActive(false);
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
		thisTransfom.position = start_position;
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

	public void SetInactivePos(int num)
    {
		thisTransfom.position = inactive_pos[num];

		thisTransfom.rotation = Quaternion.identity;

		thisTransfom.Rotate(inactive_rotate[diceCount - 1]);

		rb.isKinematic = true;
	}
}
