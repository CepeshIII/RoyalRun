using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerType", order = 2)]
public class PlayerType : ScriptableObject
{
    public GameObject PlayerBody;
    public Avatar avatar;
    public RuntimeAnimatorController animatorController;
    public bool applyRootMotion = false;
}
