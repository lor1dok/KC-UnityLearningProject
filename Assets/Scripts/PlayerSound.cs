using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    [SerializeField] private Player player;

    private float footstepTimer;
    private float footstepTimerMax = 0.1f;
    private float footstepVolume = 1.0f;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if(player.IsWalking()) {
            footstepTimer -= Time.deltaTime;
            if(footstepTimer <= 0) {
                footstepTimer = footstepTimerMax;
                SoundManager.Instance.PlayFootSteps(transform.position, footstepVolume);
            }
        }
        
    }
}
