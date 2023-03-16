using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {


    public static Player Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangeEventArgs> 
        OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArgs : EventArgs {
        public BaseCounter counterSelected;
    }

    public event EventHandler OnPickedUpSomething;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private BaseCounter counterSelected;
    private KitchenObject kitchenObject;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnAlternativeInteractAction += GameInput_OnAlternativeInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;

        counterSelected?.Interact(this);
    }

    private void GameInput_OnAlternativeInteractAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;

        counterSelected?.InteractAlternative(this);
    }

    public void Update() {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions() {

        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if(moveDirection != Vector3.zero ) {
            lastInteractDirection = moveDirection;
        }

        float maxDistance = 2f;

        if(Physics.Raycast(transform.position, lastInteractDirection, 
            out RaycastHit raycastHit, maxDistance, countersLayerMask)) {

            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                ChangeSelectedCounter(baseCounter);
            }
            else {
                ChangeSelectedCounter(null);
            }
        } else {
            ChangeSelectedCounter(null);
        }

    }

    private void HandleMovement() {

        Vector2 inputVector = gameInput.GetNormalizedMovementVector();

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 capsuleSecondPoint = transform.position + Vector3.up * 2;
        
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;

        bool canWalk = !Physics.CapsuleCast(transform.position, capsuleSecondPoint,
            playerRadius, moveDirection, moveDistance);
        
        if (!canWalk) {
            //Cannot move towards Move Direction

            //Check if can move towards X direction
            Vector3 moveDirectionX = new Vector3(inputVector.x, 0, 0).normalized;
            canWalk = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, capsuleSecondPoint, 
                playerRadius, moveDirectionX, moveDistance);

            if (canWalk) {
                //Can move X direction
                moveDirection = moveDirectionX;
            } else {
                //Cannot move X direction
                
                //Check if can move Z direction
                Vector3 moveDirectionY = new Vector3(0, 0, inputVector.y).normalized;
                canWalk = moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, capsuleSecondPoint,
                    playerRadius, moveDirectionY, moveDistance);

                if (canWalk) {
                    //Can move Y direction
                    moveDirection = moveDirectionY;
                }
            }
        } 

        if (canWalk) {
            transform.position += moveDirection * moveDistance;
        }

        isWalking = moveDirection != Vector3.zero;

        float rotateSpeed = 15f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, 
            Time.deltaTime * rotateSpeed);
    }

    private void ChangeSelectedCounter(BaseCounter counterSelected) {
        this.counterSelected = counterSelected;

        OnSelectedCounterChange?.Invoke(this,
            new OnSelectedCounterChangeEventArgs {
                counterSelected = this.counterSelected
            });
    }

    public bool IsWalking() { 
        return isWalking; 
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
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
        if (kitchenObject != null) {
            OnPickedUpSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject() {
        if (this.kitchenObject != null) {
            this.kitchenObject = null;
        }
        else {
            Debug.Log("KitchenObject is Null");
        }
    }
}
