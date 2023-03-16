using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private Animator playerAnimator;

    private const string IS_WALKING = "IsWalking";

    private void Start() {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update() {
        playerAnimator.SetBool(IS_WALKING, player.IsWalking());
    }
}
