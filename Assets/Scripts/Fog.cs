using UnityEngine;
using UnityEngine.UIElements;

public class Fog : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Vector2 position;
    [SerializeField] float distance = 40f;

    private void LateUpdate()
    {
        if(player != null)
            transform.position = new Vector3(position.x, position.y, player.transform.position.z + distance);
    }
}
