using UnityEngine;

public class NormalEnemyMovementController : BaseMovementController
{
    private void Start()
    {
        RandomMovementSpeedOffset = Random.Range(0f, 10f);
        RandomSinusOffset = Random.Range(0f, 2f);
    }
    public override void ApplyForceToRigidbody(Rigidbody2D rigidbody2D, float maxMoveSpeed, bool reverseMovement)
    {
        Vector2 horizontalMovement = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad) * RandomSinusOffset, 0);
        if (reverseMovement)
        {
            horizontalMovement *= -1;
        }

        rigidbody2D.AddForce(horizontalMovement * RandomMovementSpeedOffset);

        if (rigidbody2D.velocity.magnitude > maxMoveSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxMoveSpeed;
        }
    }
}
