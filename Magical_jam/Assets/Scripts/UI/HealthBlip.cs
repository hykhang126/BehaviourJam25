using UnityEngine;
public class HealthBlip : MonoBehaviour
{
    public float frequencyBetweenBlips;
    public Transform endPosition;
    public float blipSpeed = 10f;

    public Core.Timer frequencyTimer;

    public void Update()
    {
        // Move to end position and destroy yourself when reached
        if (Vector3.Distance(transform.position, endPosition.position) < 0.1f)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, blipSpeed * Time.deltaTime);
        }
    }
    
}