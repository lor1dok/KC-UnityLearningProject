using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            //This clearCounter doesn't have kitchenObject
            if(player.HasKitchenObject() ) {
                //Player has kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            //This clearCounter has kitchenObject
            if (player.HasKitchenObject()) { 
                //Player has kitchenObject
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroyItself();   
                    }
                } else {
                    //Player isn't holding a plate but something else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        //Counter is holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject()
                            .GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroyItself();
                        }
                    }
                }
            } else {
                //Player doesn't have kitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            GetFunctionName();
        }
    }

    public override void InteractAlternative(Player player) {
        //No alternate actions there
    }
}
