using UnityEngine;

public class ValueLerper : MonoBehaviour
{
    public int startValue = 1;
    public int endValue = 100;
    public float transitionDuration = 5f; // in seconds

    private bool isLerping = false;
    private float startTime;
    private int currentValue;
    private float timeElapsed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isLerping)
            {
                isLerping = true;
                startTime = Time.time - timeElapsed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLerping = false;
            timeElapsed = Time.time - startTime;
        }
    }

    private void Update()
    {
        if (isLerping)
        {
            float progress = Mathf.Clamp01((Time.time - startTime) / transitionDuration);
            currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, progress));
            Debug.Log("Current Value: " + currentValue);

            if (progress >= 1.0f)
            {
                Debug.Log("End Value: " + endValue);
                isLerping = false;
            }
        }
    }
}
