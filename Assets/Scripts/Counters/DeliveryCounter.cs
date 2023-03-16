using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public static DeliveryCounter Instance { 
        get; 
        private set; 
    }

    private void Awake() {
        Instance = this;
    }

    public override void Interact(Player player) {
        if(player.HasKitchenObject()) {
            //Player holds kitchen object
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                //Player holds Plate
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                Destroy(plateKitchenObject.gameObject);
            }
        }
    }
}
