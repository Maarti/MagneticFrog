using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagnetController : MonoBehaviour {

    [SerializeField] Camera mainCamera;
    [SerializeField] RectTransform magnetCtrlrLayout;
    [SerializeField] GameObject redMagnet, blueMagnet;
    [SerializeField] GameObject redMagnetParticle;
    [SerializeField] GameObject blueMagnetParticle;
    [SerializeField] GameObject magnetModel;
    [SerializeField] GameObject magnetTitlePlaceholder;
    [SerializeField] GameObject magnetModelContainer;
    [SerializeField] GameObject necklaceRendering;
    [SerializeField] GameObject lineRendering;
    [SerializeField] Rigidbody2D magnetRb;
    [SerializeField] DistanceJoint2D magnetJoint;
    [SerializeField] Animator modelAnim;
    [SerializeField] Animator magnetCtrlrAnim;
    [SerializeField] SoundController sound;
    bool magnetIsRed = true;                    // True = red (+)  False = blue (-)
    Vector3 initialModelLocalPosition;
    Coroutine coroutineMovingMagnet;

    public void Awake() {
        initialModelLocalPosition = magnetModel.transform.localPosition;
        SetMagnetToMenuState();
    }

    public void Init() {
        SwitchMagnetToRed();
    }

    void Update() {
        float pointerY;     // Y position of the cursor or touch

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0) {
            pointerY = Input.GetTouch(0).position.y;
            ManageMagnetState(pointerY);  
        }
#else
        pointerY = Input.mousePosition.y;
        ManageMagnetState(pointerY);
#endif
    }

    // Change magnet state according to pointer position
    void ManageMagnetState(float pointerY) {
        if (pointerY < magnetCtrlrLayout.position.y && magnetIsRed)
            SwitchMagnetToBlue();
        else if (pointerY > magnetCtrlrLayout.position.y && !magnetIsRed)
            SwitchMagnetToRed();
    }

    void SwitchMagnetToRed() {
        redMagnet.SetActive(true);
        redMagnetParticle.SetActive(true);
        blueMagnet.SetActive(false);
        blueMagnetParticle.SetActive(false);
        magnetIsRed = true;
        magnetCtrlrAnim.SetBool("isNorth", true);
        modelAnim.SetBool("isRed", true);
        sound.Play();
    }

    void SwitchMagnetToBlue() {
        redMagnet.SetActive(false);
        redMagnetParticle.SetActive(false);
        blueMagnet.SetActive(true);
        blueMagnetParticle.SetActive(true);
        magnetIsRed = false;
        magnetCtrlrAnim.SetBool("isNorth", false);
        modelAnim.SetBool("isRed", false);
        sound.Play();
    }

    public void SetMagnetToMenuState() {
         if(coroutineMovingMagnet!=null)
            StopCoroutine(coroutineMovingMagnet); 
        magnetModel.transform.parent = null;
        magnetModel.transform.position = magnetTitlePlaceholder.transform.position;
        magnetRb.bodyType = RigidbodyType2D.Kinematic;
        magnetJoint.enabled = false;
        magnetRb.velocity = Vector2.zero;
        necklaceRendering.SetActive(false);
        lineRendering.SetActive(false);
        magnetModel.SetActive(false);
    }

    public void SetMagnetToGameState() {
        if (coroutineMovingMagnet != null)
            StopCoroutine(coroutineMovingMagnet);
        magnetModel.transform.parent = magnetModelContainer.transform;
        magnetModel.transform.localPosition = initialModelLocalPosition;
        magnetRb.bodyType = RigidbodyType2D.Dynamic;
        magnetJoint.enabled = true;
        necklaceRendering.SetActive(true);
        lineRendering.SetActive(true);
        magnetModel.SetActive(true);
    }

    public void StartMovingMagnetToFrog() {
        coroutineMovingMagnet = StartCoroutine(MoveMagnetToFrog());               
    }

    IEnumerator MoveMagnetToFrog() {
        magnetModel.SetActive(true);
        magnetModel.transform.localScale = new Vector3(2.2f, 2.2f, 1f);
        magnetModel.transform.rotation = new Quaternion(0f, 0f, 180f, 1f);
        magnetModel.transform.DOScale(Vector3.one, 1f);
        magnetModel.transform.DORotate(Vector3.zero, 1f);
        float speed = 8;
        float step;
        while (Vector3.Distance(magnetModel.transform.position, magnetModelContainer.transform.position) > .01f) {
            step = speed * Time.deltaTime;
            magnetModel.transform.position = Vector3.MoveTowards(magnetModel.transform.position, magnetModelContainer.transform.position, step);
            yield return null;
        }
        magnetModel.transform.position = magnetModelContainer.transform.position;
        SetMagnetToGameState();
    }
}
