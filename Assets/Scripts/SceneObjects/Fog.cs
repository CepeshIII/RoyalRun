using UnityEngine;
using UnityEngine.UIElements;

public class Fog : MonoBehaviour
{
    [SerializeField] Vector2 position;
    [SerializeField] float distance = 40f;
    [SerializeField] GameManager gameManager;

    private void OnEnable()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void LateUpdate()
    {
        var player = gameManager.Player;
        if (player != null)
            transform.position = new Vector3(position.x, position.y, player.transform.position.z + distance);
    }
}
