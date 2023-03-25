using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs 
    {
        public BaseCounter selectedCounter;
    }
    public event EventHandler OnPickedSomething;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake() {
        if (Instance != null)
        {
            Debug.LogError("There is more then one player instance");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlterAction += GameInput_OnInteractAlterAction;
    }

    private void Update() {
        HandleMovement();
        HandleInteraction();
    }

    private void GameInput_OnInteractAlterAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) { return; }
        
        selectedCounter?.InteractAlter(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) { return; }
        
        selectedCounter?.Interact(this);
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMoveVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0 , inputVector.y);

        float playerHeight = 2f;
        float playerRadius = .7f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Can't move torward moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0 , 0).normalized;
            canMove = (moveDirX.x < -.5f || moveDirX.x > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            if (!canMove)
            {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0 ,moveDir.z).normalized;
                canMove = (moveDirZ.z < -.5f || moveDirZ.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                
                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteraction() {
        Vector2 inputVector = gameInput.GetMoveVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0 , inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                // Has Counter
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            } 
            else
            {
                SetSelectedCounter(null);
            }
        } 
        else
        {
            SetSelectedCounter(null);
        }
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }
}
