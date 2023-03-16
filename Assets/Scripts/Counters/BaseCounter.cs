using System.Collections;
using System.Collections.Generic;
using StackTrace = System.Diagnostics.StackTrace;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private bool debugMessages;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact()! This should never be Triggered!");
    }

    public virtual void InteractAlternative(Player player) {
        Debug.LogError("BaseCounter.InteractAlternative()! This should never be Triggered!");

    }

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject() {
        if (kitchenObject != null) {
            return kitchenObject;
        }
        else {
            return null;
        }
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject() {
        if (this.kitchenObject != null) {
            this.kitchenObject = null;
        }
        else {
            Debug.LogWarning("KitchenObject is already Null");
        }
    }

    protected bool GetDebugMessagesEnabled() { 
        return debugMessages;
    }

    protected void GetFunctionName() {
        if (debugMessages) {
            System.Diagnostics.StackTrace stackTrace = new StackTrace(0);
            string methodName = stackTrace.GetFrame(1).GetMethod().Name;
            string previousMethod = stackTrace.GetFrame(2)?.GetMethod().Name;
            if (previousMethod == null) previousMethod = ToString();
            UnityEngine.Debug.Log(methodName + "( ) has been called from " + previousMethod);
        }
    }
}
