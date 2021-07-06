using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController2D controller;
    public float runSpeed = 40f;
    public Animator anim;

    private float horizontalMovement = 0;
    private bool jump = false;
    private static readonly int Walking = Animator.StringToHash("walking");
    private Vector3 startPos;
    
    public static int deathCounter = 0;

    private void Start() {
        startPos = transform.position;
    }

    private void Update() {
        horizontalMovement = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Math.Abs(horizontalMovement) > 0.0001) {
            anim.SetBool(Walking, true);
        }
        else {
            anim.SetBool(Walking, false);
        }
        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMovement * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Spike")) {
            transform.position = startPos;
            deathCounter++;
        }
    }
}