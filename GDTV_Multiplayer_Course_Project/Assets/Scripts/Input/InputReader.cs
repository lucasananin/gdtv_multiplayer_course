using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "SO/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls _controls = null;

    public event System.Action<bool> OnPrimaryFireEvent = null;
    public event System.Action<Vector2> OnMoveEvent = null;

    private void OnEnable()
    {
        if (_controls is null)
        {
            _controls = new();
            _controls.Player.SetCallbacks(this);
        }

        _controls.Enable();
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        var _value = _context.ReadValue<Vector2>();
        OnMoveEvent?.Invoke(_value);
    }

    public void OnPrimaryFire(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            OnPrimaryFireEvent?.Invoke(true);
        }
        else if (_context.canceled)
        {
            OnPrimaryFireEvent?.Invoke(false);
        }

        //var _value = _context.performed ? true : false;
        //OnPrimaryFireEvent?.Invoke(_context.ReadValue<bool>());
    }
}
