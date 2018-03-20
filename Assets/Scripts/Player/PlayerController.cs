﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float timeBetweenJumps = .5f;        // time to wait between each jump
    public float jumpForce = 2f;                // multiplier of the x vector
    public float horizontalForce = 2f;          // multiplier of the y vector
    public Canvas canvas;
    public RectTransform magnetCtrlr;
    public GameObject redMagnet, blueMagnet;
    public SpriteRenderer magnetSprite;
    public bool movingRelativeToPlayer = true;  // when toucing the screen, move relatively to the player or to the middle of the screen

    float lastJump = -1f;
    Rigidbody2D rb;
    float screenMid;                            // Middle of the screen width in pixels correct
    bool magnetIsRed = true;                    // True = red (+)  False = blue (-)
    int nbJumpForCurrentTouch = 0;             // We have to re-touch to screen to jump again


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start () {
        SwitchMagnetToRed();
	}
	
	// Update is called once per frame
	void Update () {
#if (UNITY_ANDROID || UNITY_IOS) //&& !UNITY_EDITOR
        if (Input.touchCount > 0 && nbJumpForCurrentTouch==0) {
            Jump();
            float touchY = Input.GetTouch(0).position.y;
            if (touchY < magnetCtrlr.position.y && magnetIsRed)
                SwitchMagnetToBlue();
            else if (touchY > magnetCtrlr.position.y && !magnetIsRed)
                SwitchMagnetToRed();
        } else if (Input.touchCount == 0 && nbJumpForCurrentTouch > 0)
            ResetNbJump();
#else
        if (Input.GetButtonDown("Jump"))
            Jump();

        float touchY = Input.mousePosition.y;
        if (touchY < magnetCtrlr.position.y && magnetIsRed)
            SwitchMagnetToBlue();
        else if (touchY > magnetCtrlr.position.y && !magnetIsRed)
            SwitchMagnetToRed();
        /* 
        if (Input.GetButtonDown("Switch")) {
            if (magnetIsRed)
                SwitchMagnetToBlue();
            else
                SwitchMagnetToRed();
        }*/
#endif   
    }

    void Jump() {
        if (Time.time - timeBetweenJumps < lastJump)
            return;

        Vector2 inputController;    // Position of the cursor or finger
        Vector2 direction;          // Direction of the jump
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        inputController = Input.GetTouch(0).position;
#else
        // Keyboard controller
        //xAxis = Input.GetAxisRaw("Horizontal")/2;
        //yAxis = Mathf.Clamp(Input.GetAxisRaw("Vertical"),0.5f,1f);

        // Mouse controller
        inputController = Input.mousePosition;
        inputController.x = Mathf.Clamp(inputController.x, 0f, Screen.width);
        inputController.y = Mathf.Clamp(inputController.y, 0f, Screen.height);
#endif
        if(!movingRelativeToPlayer)
            direction.x = (inputController.x - Screen.width / 2f) / (Screen.width / 2);   // Move relatively to the middle of the screen
        else
            direction.x = (inputController.x - Camera.main.WorldToScreenPoint(transform.position).x) / Screen.width; // Move relatively to the player
        direction.y = 1f;
        direction.Normalize();
        direction = new Vector2(direction.x * horizontalForce, direction.y * jumpForce);

        rb.velocity = Vector2.zero;
        rb.AddForce(direction);
        lastJump = Time.time;
        nbJumpForCurrentTouch++;
    }

    void ResetNbJump() {
        nbJumpForCurrentTouch = 0;
    }

    void SwitchMagnetToRed() {
        redMagnet.SetActive(true);
        blueMagnet.SetActive(false);
        magnetSprite.color = Color.red;
        magnetIsRed = true;
    }

    void SwitchMagnetToBlue() {
        redMagnet.SetActive(false);
        blueMagnet.SetActive(true);
        magnetSprite.color = Color.blue;
        magnetIsRed = false;
    }
}
