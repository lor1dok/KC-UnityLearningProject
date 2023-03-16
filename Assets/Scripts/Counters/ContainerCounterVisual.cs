using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;

    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    private void Start() {
        containerCounter.OnPlayerGrabbedKitchenObject += ContainerCounter_OnPlayerGrabbedKitchenObject; 
    }

    private void ContainerCounter_OnPlayerGrabbedKitchenObject(object sender, System.EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
