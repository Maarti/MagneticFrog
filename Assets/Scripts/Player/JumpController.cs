using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class JumpController : MonoBehaviour {

    public delegate void JumpDelegate();
    public static event JumpDelegate OnJump;
    public float timeBetweenJumps = 1.2f;                       // time to wait between each jump
    public float stunIncrementer = .75f;                        // increment the stunMultiplier by this value at each stun
    [HideInInspector] public float lastJump = -1f;              // time the last jump happened
    [SerializeField] float jumpForce = 250f;                    // multiplier of the x vector
    [SerializeField] float horizontalForce = 200f;              // multiplier of the y vector
    [SerializeField] bool movingRelativeToPlayer = true;        // when touching the screen, move relatively to the player or to the middle of the screen
    [Header("Player Canvas")]
    [SerializeField] GameObject playerCanvas;
    [SerializeField] Transform playerCanvasAnchor;
    [SerializeField] Image jumpCooldownImg;
    [SerializeField] Image stunCooldownImg;
    [Header("Sounds")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] float pitchMin = 1f;
    [SerializeField] float pitchMax = 2f;
    Rigidbody2D rb;
    Animator animator;
    bool isStuned = false;                                      // Can't jump while stuned
    float stunMultiplier = .75f;                                // multiply the stun duration, increase at each stun
    float lastStun = -10f;                                      // time the last stun happened
    float totalStunDuration;                                    // total duration of the last stun

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    int nbJumpForCurrentTouch = 0;                              // We have to re-touch the screen to jump again
#endif

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init() {
        animator = GetCurrentAnimator();
        isStuned = false;
        jumpCooldownImg.gameObject.SetActive(false);
        stunCooldownImg.gameObject.SetActive(false);
        InitAgility();
        InitStamina();
        stunMultiplier = stunIncrementer;
        SetRotation(Vector2.zero);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        nbJumpForCurrentTouch = 0;
#endif
    }

    void OnDisable() {
        rb.velocity = Vector2.zero;
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
        UpdateJumpCooldownImg();
        UpdateStunCooldownImg();
    }

    void LateUpdate() {
        UpdatePlayerCanvasPosition();
    }

    Animator GetCurrentAnimator() {
        return ApplicationController.ac.characters[CharacterSelector.currentCharacter].skin.GetComponent<Animator>();
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

        // Velocity and rotation
        rb.velocity = Vector2.zero;
        rb.AddForce(direction);
        SetRotation(direction);

        // Animation and sound
        animator.SetTrigger("swim");
        PlaySwimmingSound();

        lastJump = Time.time;
        if (OnJump != null)
            OnJump();
    }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    void ResetNbJump() {
        nbJumpForCurrentTouch = 0;
    }
#endif

    // Rotate the frog on the z axis with an angle between -40 and +40 depending of the direction
    void SetRotation(Vector2 direction) {
        float xDirection = Mathf.Clamp(direction.x, -130f, 130f);
        float rotation = Mathf.Lerp(40f, -40f, (xDirection + 130f) / 260f);
        rb.rotation = rotation;
    }

    void UpdatePlayerCanvasPosition() {
        playerCanvas.gameObject.transform.position = playerCanvasAnchor.position;
    }

    void UpdateJumpCooldownImg() {
        float fillValue = (timeBetweenJumps - (Time.time - lastJump)) / timeBetweenJumps;
        if (jumpCooldownImg.gameObject.activeSelf && fillValue <= 0)
            jumpCooldownImg.gameObject.SetActive(false);
        else if (!jumpCooldownImg.gameObject.activeSelf && fillValue >= 0.01f)
            jumpCooldownImg.gameObject.SetActive(true);
        jumpCooldownImg.fillAmount = fillValue;
    }

    void UpdateStunCooldownImg() {
        if (!isStuned) {
            if (stunCooldownImg.gameObject.activeSelf)
                stunCooldownImg.gameObject.SetActive(false);
            return;
        }
        float fillValue = (totalStunDuration - (Time.time - lastStun)) / totalStunDuration;
        if (!stunCooldownImg.gameObject.activeSelf && fillValue != 0)
            stunCooldownImg.gameObject.SetActive(true);
        stunCooldownImg.fillAmount = fillValue;
    }

    public void Stun(float mineStunDuration) {
        totalStunDuration = mineStunDuration * stunMultiplier;
        StartCoroutine(StunForSeconds(totalStunDuration));
        // We increment the stun multiplier at each stun, so next time, it will be longer
        IncrementStunMultiplier();
    }

    void IncrementStunMultiplier() {
        stunMultiplier += stunIncrementer;
    }

    IEnumerator StunForSeconds(float duration) {
        isStuned = true;
        lastStun = Time.time;
        lastJump = -10f; // when stuned, we reset the jump cooldown
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        isStuned = false;
    }

    void InitAgility() {
        switch (ApplicationController.ac.characters[CharacterSelector.currentCharacter].agility) {
            case 0:
                timeBetweenJumps = 1.2f;
                break;
            case 1:
                timeBetweenJumps = 1f;
                break;
            case 2:
                timeBetweenJumps = .8f;
                break;
            case 3:
                timeBetweenJumps = .6f;
                break;
        }
    }

    void InitStamina() {
        switch (ApplicationController.ac.characters[CharacterSelector.currentCharacter].stamina) {
            case 0:
                stunIncrementer = 1f;
                break;
            case 1:
                stunIncrementer = .75f;
                break;
            case 2:
                stunIncrementer = .5f;
                break;
            case 3:
                stunIncrementer = .375f;
                break;
        }
    }

    void PlaySwimmingSound() {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.Play();
    }


}
