using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator animator;
    private const string CUT = "Cut";

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    private void Start() {
        cuttingCounter.OnProgressChange += CuttingCounter_OnProgressChange;
    }

    private void CuttingCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        if(e.progressNormalized != 0) {
            animator.SetTrigger(CUT);
        }
    }
}
