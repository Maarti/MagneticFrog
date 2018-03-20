// https://www.youtube.com/watch?v=QkisHNmcK7Y

using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // General public parameters
    public float backgroundSize, parallaxSpeed;

   
    // General private parameters
    Transform cameraTransform;

    // Horizontal private parameters
    Transform[] layers;
    float viewZone = 4, lastCameraY;
    int botIndex, topIndex;


    void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraY = cameraTransform.position.y;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);
        botIndex = 0;
        topIndex = layers.Length - 1;
        
    }

    void Update() {
        float deltaY = cameraTransform.position.y - lastCameraY;
        transform.position += Vector3.up * (deltaY * parallaxSpeed);
        lastCameraY = cameraTransform.position.y;

        //Debug.Log ("camera = " + cameraTransform.position.x + " limit = " + (layers [botIndex].transform.position.x + viewZone));
        if (cameraTransform.position.y < (layers[botIndex].transform.position.y + viewZone))
            ScrollBot();

        if (cameraTransform.position.y > (layers[topIndex].transform.position.y - viewZone))
            ScrollTop();
    }

    private void ScrollBot() {
        Vector3 top = layers[topIndex].position;
        layers[topIndex].position = new Vector3(top.x, (layers[botIndex].position.y - backgroundSize), top.z);
        botIndex = topIndex;
        topIndex--;
        if (topIndex < 0)
            topIndex = layers.Length - 1;
    }

    private void ScrollTop() {
        Vector3 bot = layers[botIndex].position;
        layers[botIndex].position = new Vector3(bot.x, (layers[topIndex].position.y + backgroundSize), bot.z);
        topIndex = botIndex;
        botIndex++;
        if (botIndex == layers.Length)
            botIndex = 0;
    }
}