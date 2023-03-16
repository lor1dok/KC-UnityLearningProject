using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() { 
        return kitchenObjectSO; 
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }
    
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {

        if(this.kitchenObjectParent != null) { 
            this.kitchenObjectParent.ClearKitchenObject();
        }

        if(kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError(kitchenObjectParent + " already has Kitchen Object!");
        }

        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public void DestroyItself() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, 
        IKitchenObjectParent kitchenObjectParent) {

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.kitchenObjectPrefab,
                kitchenObjectParent.GetKitchenObjectFollowTransform());
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if(this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {
            plateKitchenObject = null;
            return false;
        }

    }
}
