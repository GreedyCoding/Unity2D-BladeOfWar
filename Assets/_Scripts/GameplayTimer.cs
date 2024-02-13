using UnityEngine;

public class GameplayTimer : MonoBehaviour
{
    public static GameplayTimer Instance;

    public float CurrentTime { get; private set; }
    public bool IsPaused { get; private set; }

    public float TimeInspector;

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
        TimeInspector = CurrentTime;
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

    public void FreezeGameTime()
    {
        Time.timeScale = 0f;
    }

    public void UnfreezeGameTime()
    {
        Time.timeScale = 1f;
    }
}
