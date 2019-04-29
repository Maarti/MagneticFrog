using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class CollectedBubble : MonoBehaviour {
    [SerializeField] RectTransform target;
    [SerializeField] Animator anim;
    RectTransform rect;
    bool isStarted = false;

    void Start() {
        rect = GetComponent<RectTransform>();
        isStarted = true;
    }

    void OnEnable() {
        if (!isStarted)
            Start();
        rect.DOAnchorPos(target.anchoredPosition, .4f).SetEase(Ease.OutExpo).OnComplete(OnBubbleTouchGauge);
    }

    void OnBubbleTouchGauge() {
        anim.SetTrigger("bump");
        gameObject.SetActive(false);
    }

    public void Recycle(Vector2 screenPosition) {
        if (!isStarted)
            Start();
        rect.position = screenPosition;
        gameObject.SetActive(true);
    }

}
