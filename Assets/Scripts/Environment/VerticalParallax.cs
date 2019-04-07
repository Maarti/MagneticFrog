using UnityEngine;

public class VerticalParallax : MonoBehaviour
{

    [SerializeField] Camera cam;
    [Tooltip("When the camera reaches this Y coordinate, the element will have reach his target point")]
    [SerializeField] float targetCameraY = 200f;
    [Tooltip("Y coordinate the element will move to")]
    [SerializeField] float targetLocalY = -3f;

    float startLocalY = 0f;
    float startCameraY = 0f;
    float totalCameraDistanceToTravel = 200f;


    void Start()
    {
        startLocalY = transform.localPosition.y;
        startCameraY = cam.transform.position.y;
        totalCameraDistanceToTravel = targetCameraY - startCameraY;
    }
        
    void Update()
    {
        Vector3 newPosition = transform.localPosition;
        float newY = Mathf.Lerp(startLocalY, targetLocalY, (cam.transform.position.y-startCameraY)/totalCameraDistanceToTravel);
        newPosition.y = newY;
        transform.localPosition = newPosition;
    }
}
