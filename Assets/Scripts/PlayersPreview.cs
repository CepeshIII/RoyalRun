using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayersPreview : MonoBehaviour
{
    [SerializeField] private GameObject pedestalPrefab;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private RuntimeAnimatorController animatorController;

    private GameObject[] playersVariants;
    private PlayerType[] playerTypes;
    private int currentPlayerIndex = 0;

    void Start()
    {
        LoadPlayerVariants();
        SwitchPlayer(currentPlayerIndex);

        PlayerPrefs.SetString("playerType", playerTypes[currentPlayerIndex].name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchToPreviousPlayer();
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToNextPlayer();
        }
    }

    public void SwitchToNextPlayer()
    {
        SwitchPlayer(currentPlayerIndex - 1);
    }

    public void SwitchToPreviousPlayer()
    {
        SwitchPlayer(currentPlayerIndex + 1);
    }

    private void SwitchPlayer(int index)
    {
        index = Mathf.Clamp(index, 0, playerTypes.Length - 1);

        playersVariants[currentPlayerIndex].SetActive(false);
        currentPlayerIndex = index;
        cinemachineCamera.Target.TrackingTarget = playersVariants[currentPlayerIndex].transform;
        PlayerPrefs.SetString("playerType", playerTypes[currentPlayerIndex].name);
        playersVariants[currentPlayerIndex].SetActive(true);
    }

    private void LoadPlayerVariants()
    {
        playerTypes  = Resources.LoadAll<PlayerType>("AssetDatabase/Players");
        playersVariants = new GameObject[playerTypes.Length];

        for (int i = 0; i < playerTypes.Length; i++) 
        {
            var playerType = playerTypes[i];
            var parent = new GameObject(playerType.name);
            var position = new Vector3(i * 5f, 0f, 0f);
            parent.transform.position = position;

            var body = Instantiate(playerType.PlayerBody, position, 
                Quaternion.identity, parent.transform);

            var animator = body.GetComponent<Animator>();
            if (animator == null)
            {
                animator = body.AddComponent<Animator>();
            }
            animator.avatar = playerType.avatar;
            animator.runtimeAnimatorController = animatorController;
            animator.applyRootMotion = playerType.applyRootMotion;
            animator.SetBool("Falling", false);

            if (pedestalPrefab != null) 
            { 
                Instantiate(pedestalPrefab, new Vector3(0f, -1f, 0f) + position, 
                    Quaternion.identity, parent.transform);
            }

            playersVariants[i] = parent;
            parent.SetActive(false);

        }
    }
}
