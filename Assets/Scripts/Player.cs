using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; } // This is a property, used for singleton pattern
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one player instance!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction; // Subscribe a listener to the OnInteractAction event
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            // Player is currently facing a clear counter

            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        // Reuse HandleMovement code instead of referencing it because of vastly different use cases
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            // Player is holding an input direction

            // Keep track of the last input direction when player stops pressing anything
            lastInteractDirection = moveDir;
        }

        float maxInteractDistance = 2f;
        
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, maxInteractDistance, countersLayerMask)) // This raycast outputs both bool and collision data within that layer
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) // TryGetComponent automatically handles null checks
            {
                // GameObject hit has ClearCounter component

                // Keep track of the current clearCounter selected
                if (clearCounter != selectedCounter)
                {
                    // Stupid moment: I accidentally passed selectedCounter instead of clearCounter, causing the script to not work while not firing any errors.
                    // it took me 5 minutes to debug this shit
                    SetSelectedCounter(clearCounter);
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

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // Fix diagonal movement on collision
        if (!canMove)
        {
            // Cannot move towards moveDir
            // Try to move either only on the X or Z axes

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized; // Normalized eliminates the case where moveDir.x may return .71f
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized; // Normalized eliminates the case where moveDir.z may return .71f
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction (stuck)
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero; // Returns true or false

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter // These are two different selectedCounter
        });
    }

}
