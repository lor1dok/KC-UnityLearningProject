using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedKitchenObject;

    public override void Interact(Player player) {
        if(!player.HasKitchenObject()) {
            //Player is not carring anything
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.kitchenObjectPrefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            OnPlayerGrabbedKitchenObject?.Invoke(this, EventArgs.Empty);
        }

        if (GetDebugMessagesEnabled()) {
            Debug.Log("Here player should grab kitchenObject");
        }
    }

    public override void InteractAlternative(Player player) {
        //No alternate actions there
    }
}
