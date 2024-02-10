using UnityEngine;

public class GameplayTimer : MonoBehaviour
{
    public static GameplayTimer Instance;

    public float CurrentTime { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (IsPaused) return;

        CurrentTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        IsPaused = false;
    }

    public void StopTimer()
    {
        IsPaused = true;
    }

    public void ResetTimer() 
    { 
        CurrentTime = 0f; 
    }
}
