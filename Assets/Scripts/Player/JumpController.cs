using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpController : MonoBehaviour {

    public delegate void JumpDelegate();
    public static event JumpDelegate OnJump;
    public float timeBetweenJumps = 1f;                         // time to wait between each jump
    [HideInInspector] public float lastJump = -1f;
    [SerializeField] float jumpForce = 250f;                    // multiplier of the x vector
    [SerializeField] float horizontalForce = 200f;              // multiplier of the y vector
    [SerializeField] bool movingRelativeToPlayer = true;        // when touching the screen, move relatively to the player or to the middle of the screen
    Rigidbody2D rb;
    bool isStuned = false;                                      // Can't jump while stuned

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    int nbJumpForCurrentTouch = 0;                              // We have to re-touch the screen to jump again
#endif

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init() {
        isStuned = false;
        SetRotation(Vector2.zero);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        nbJumpForCurrentTouch = 0;
#endif
    }


    void Update() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0 && nbJumpForCurrentTouch==0) {
            Jump();
        } else if (Input.touchCount == 0 && nbJumpForCurrentTouch > 0)
            ResetNbJump();
#else
        if (Input.GetButtonDown("Jump"))
            Jump();
#endif
    }


    void Jump() {
        if (Time.time - timeBetweenJumps < lastJump || isStuned)
            return;

        Vector2 pointer;            // Position of the cursor or finger
        Vector2 direction;          // Direction of the jump
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        pointer = Input.GetTouch(0).position;
        nbJumpForCurrentTouch++;
#else
        // Keyboard controller
        //xAxis = Input.GetAxisRaw("Horizontal")/2;
        //yAxis = Mathf.Clamp(Input.GetAxisRaw("Vertical"),0.5f,1f);

        // Mouse controller
        pointer = Input.mousePosition;
        pointer.x = Mathf.Clamp(pointer.x, 0f, Screen.width);
        pointer.y = Mathf.Clamp(pointer.y, 0f, Screen.height);
#endif
        if (!movingRelativeToPlayer)
            direction.x = (pointer.x - Screen.width / 2f) / (Screen.width / 2);   // Move relatively to the middle of the screen
        else
            direction.x = (pointer.x - Camera.main.WorldToScreenPoint(transform.position).x) / Screen.width; // Move relatively to the player
        direction.y = 1f;
        direction.Normalize();
        direction = new Vector2(direction.x * horizontalForce, direction.y * jumpForce);

        rb.velocity = Vector2.zero;
        rb.AddForce(direction);
        SetRotation(direction);

        lastJump = Time.time;
        if (OnJump != null)
            OnJump();
    }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    void ResetNbJump() {
        nbJumpForCurrentTouch = 0;
    }
#endif

    public void Stun(float duration) {
        StartCoroutine(StunForSeconds(duration));
    }

    IEnumerator StunForSeconds(float duration) {
        isStuned = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        isStuned = false;
    }

    // Rotate the frog on the z axis with an angle between -40 and +40 depending of the direction
    void SetRotation(Vector2 direction) {
        float xDirection = Mathf.Clamp(direction.x, -130f, 130f);
        float rotation = Mathf.Lerp(40f, -40f, (xDirection + 130f) / 260f);
        rb.rotation = rotation;
    }
}
