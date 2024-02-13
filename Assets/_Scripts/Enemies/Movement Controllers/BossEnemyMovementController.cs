using UnityEngine;

public class BossEnemyMovementController : BaseMovementController
{
    private void Start()
    {
        RandomMovementSpeedOffset = Random.Range(5f, 10f);
        RandomSinusOffset = Random.Range(0f, 2f);
    }
    public override void ApplyForceToRigidbody(Rigidbody2D rigidbody2D, float maxMoveSpeed, bool reverseMovement)
    {
        float randomOffset = Random.Range(0.2f, 1);
        Vector2 horizontalMovement = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad), 0);

        if(reverseMovement)
        {
            horizontalMovement = horizontalMovement * -1f;
        }

        rigidbody2D.AddForce(horizontalMovement * randomOffset * 10f);

        if (rigidbody2D.velocity.magnitude > maxMoveSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxMoveSpeed;
        }

        if (this.transform.position.y <= 2.5f)
        {
            rigidbody2D.gravityScale = 0f;
        }
    }
}
