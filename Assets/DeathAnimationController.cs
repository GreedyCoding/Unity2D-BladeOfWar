using UnityEngine;

public class DeathAnimationController : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        this.gameObject.SetActive(false);
    }
}
