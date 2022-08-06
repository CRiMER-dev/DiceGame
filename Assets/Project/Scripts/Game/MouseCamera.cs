using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    private Vector2 _turn;
    [SerializeField]private float _sensitivity = .5f;
    private Transform _thisTransform;
    private bool _cursorLocked = false;
    private DiceScript[] _dices = new DiceScript[5];
    [SerializeField] private Game _game;

    private void Awake()
    {
        _thisTransform = transform;

        _dices[0] = GameObject.Find("dice1").GetComponent<DiceScript>();
        _dices[1] = GameObject.Find("dice2").GetComponent<DiceScript>();
        _dices[2] = GameObject.Find("dice3").GetComponent<DiceScript>();
        _dices[3] = GameObject.Find("dice4").GetComponent<DiceScript>();
        _dices[4] = GameObject.Find("dice5").GetComponent<DiceScript>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _cursorLocked = !_cursorLocked;

            if (_cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            } 
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (_game.GameState == Game.State.THROWING)
        //        foreach (DiceScript dice in _dices)
        //            dice.MoveMouse = false;
        //}

        if (_cursorLocked)
        {
            _turn.x += Input.GetAxis("Mouse X") * _sensitivity;
            _turn.y += Input.GetAxis("Mouse Y") * _sensitivity;

            _thisTransform.localRotation = Quaternion.Euler(-_turn.y, _turn.x, 0);
        }
    }
}
