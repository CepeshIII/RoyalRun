using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMovable _movement;
    private PlayerInput _playerInput;

    public void OnEnable()
    {
        _movement = GetComponent<IMovable>();
        _playerInput = GetComponent<PlayerInput>();

        //_playerInput.OnClick = () => { _movement.Jump(); };
    }

    public void Update()
    {
        var horizontalInput = _playerInput.MoveInput.x;
        _movement?.Move(1f, Vector3.forward + new Vector3(horizontalInput, 0));
    }


    private void OnDestroy()
    {
        //_playerInput.OnClick = null;
    }
}
