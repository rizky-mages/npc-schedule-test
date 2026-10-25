using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] private float commuteSpeed = 50f; // Fast for travel
    [SerializeField] private float wanderSpeed = 25f;   // Slow for strolling
    [SerializeField] private SpriteRenderer spriteRenderer; 
    
    [Header("Wander Settings")]
    [SerializeField] private float wanderRadius = 30f; 
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;

    private Vector3? primaryDestination;
    private Vector2 currentTarget;
    private bool isWandering;
    private float waitTimer;
    private float interactionPauseTimer = 0f;
    
    // Stuck Detection
    private float stuckTimer = 0f;
    private float maxStuckTime = 10f; 

    private Rigidbody2D rb;

    // Fulfill the IMovable interface
    public bool IsAtDestination => isWandering;

    private void Awake() { rb = GetComponent<Rigidbody2D>(); }

    public void MoveTo(Vector3 destination)
    {
        primaryDestination = destination;
        currentTarget = destination;
        isWandering = false;
        stuckTimer = 0f; // Reset stuck timer on new command
    }

    public void PauseMovement(float duration) { interactionPauseTimer = duration; }

    private void FixedUpdate()
    {
        if (interactionPauseTimer > 0f)
        {
            interactionPauseTimer -= Time.fixedDeltaTime;
            return;
        }

        if (!primaryDestination.HasValue) return;

        if (spriteRenderer != null)
        {
            float directionX = currentTarget.x - rb.position.x;
            Vector3 currentScale = spriteRenderer.transform.localScale;

            if (directionX > 0.05f) 
            {
                currentScale.x = Mathf.Abs(currentScale.x); 
            }
            else if (directionX < -0.05f) 
            {
                currentScale.x = -Mathf.Abs(currentScale.x); 
            }
            
            spriteRenderer.transform.localScale = currentScale;
        }

        float currentSpeed = isWandering ? wanderSpeed : commuteSpeed;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, currentTarget, currentSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(rb.position, currentTarget) < 0.1f)
        {
            if (!isWandering)
            {
                isWandering = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
            }
            else
            {
                waitTimer -= Time.fixedDeltaTime;
                if (waitTimer <= 0f) PickNewWanderPoint();
            }
        }
        else if (!isWandering) 
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer > maxStuckTime)
            {
                isWandering = true;
                stuckTimer = 0f;
                PickNewWanderPoint();
            }
        }
    }

    private void PickNewWanderPoint()
    {
        if (primaryDestination.HasValue)
        {
            Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
            currentTarget = (Vector2)primaryDestination.Value + randomOffset;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }
}