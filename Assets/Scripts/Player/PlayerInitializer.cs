using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] PlayerType playerType;
    [SerializeField] GameObject mainPlayerPrefab;

    private void OnEnable()
    {
        if (mainPlayerPrefab == null || playerType == null) return;
        playerType = Resources.Load<PlayerType>($"AssetDatabase/Players/{PlayerPrefs.GetString("playerType")}");

        gameObject.SetActive(false);
        var mainPart = Instantiate(mainPlayerPrefab, transform.position, Quaternion.identity, transform);
        var body = Instantiate(playerType.PlayerBody, mainPart.transform);
        body.AddComponent<PlayerAnimationEventManager>();

        var animator = body.GetComponent<Animator>();
        if (animator == null) 
        {
            animator = body.AddComponent<Animator>(); 
        }
        animator.avatar = playerType.avatar;
        animator.runtimeAnimatorController = playerType.animatorController;
        animator.applyRootMotion = playerType.applyRootMotion;
        mainPart.transform.parent = null;
        Destroy(gameObject);
    }
}
