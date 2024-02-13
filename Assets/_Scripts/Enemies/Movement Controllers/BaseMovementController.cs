using UnityEngine;

public abstract class BaseMovementController : MonoBehaviour
{
    public float RandomMovementSpeedOffset;
    public float RandomSinusOffset;

    public abstract void ApplyForceToRigidbody(Rigidbody2D rigidbody2D, float maxMoveSpeed, bool reverseMovement);
}
