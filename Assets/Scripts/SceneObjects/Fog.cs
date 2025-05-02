using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField] Vector2 _position;
    [SerializeField] float _distance = 40f;
    [SerializeField] Player _player;

    public void Initialize(Player player)
    {
        _player = player;
    }

    private void LateUpdate()
    {
        if (_player != null)
            transform.position = new Vector3(_position.x, _position.y, _player.transform.position.z + _distance);
    }
}
