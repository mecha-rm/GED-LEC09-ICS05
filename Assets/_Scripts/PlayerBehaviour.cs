﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public bool isGrounded;
    public float verticalForce; // jump force
    public float horizontalForce; // movement force

    private Vector2 direction; // direction of movement

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // _Move();
    // }

    private void FixedUpdate()
    {

        // if no directional keys are being pressed, the direction is set to (0, 0)
        if(direction != Vector2.zero && (!Keyboard.current.aKey.isPressed && !Keyboard.current.leftArrowKey.isPressed
            && !Keyboard.current.dKey.isPressed && !Keyboard.current.rightArrowKey.isPressed))
        {
            direction = Vector2.zero;
        }
        
        // if the direction is not set to 0, add force for movement.
        // if the direction is 0, there is no more force being applied, so the player will slow down naturally.
        if(direction != Vector2.zero)
        {
            m_rigidBody2D.AddForce(direction * horizontalForce * Time.deltaTime, ForceMode2D.Force);
        }
        // else // this is noot needed since the direction vector is set to Vector2.zero.
        // {
        //     // m_rigidBody2D.velocity = new Vector2(m_rigidBody2D.velocity.x * 0.05F, m_rigidBody2D.velocity.y);
        //     // 
        //     // if(Mathf.Abs(m_rigidBody2D.velocity.x) < 0.05F)
        //     // {
        //     //     m_rigidBody2D.velocity = new Vector2(0.0F, m_rigidBody2D.velocity.y);
        //     // }
        // }


        // Jump
        if (isGrounded)
        {
            // m_rigidBody2D.AddForce(direction * horizontalForce * Time.deltaTime);

            // changes animation if the player is falling, or puts it back to walk if not falling.
            // if (Mathf.Abs(m_rigidBody2D.velocity.x) > 0.1F)
            if (Mathf.Abs(m_rigidBody2D.velocity.y) > 0.5F) // original value made it change too soon.
            {
                m_animator.SetInteger("AnimState", 2);
            }
            else
            {
                m_animator.SetInteger("AnimState", 1);
            }
        }

        // standing position if not moving (originally only checked x, now checks for x and y)
        if (Mathf.Approximately(m_rigidBody2D.velocity.x, 0.0F)
            && m_rigidBody2D.velocity.y == 0.0F)
        {
            m_animator.SetInteger("AnimState", 0);
        }
    }

    // void _Move()
    // {
    // 
    // }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Debug.Log("On Move");
        // direction = context.ReadValue<Vector2>();

        // checks for direction for both sprite and movement
        switch(context.control.name)
        {
            case "a":
            case "leftArrow":
                m_spriteRenderer.flipX = true;
                // m_rigidBody2D.AddForce(Vector2.left * horizontalForce);
                direction = Vector2.left;
                break;

            case "d":
            case "rightArrow":
                m_spriteRenderer.flipX = false;
                // m_rigidBody2D.AddForce(Vector2.right * horizontalForce);
                direction = Vector2.right;
                break;
        }

        // m_animator.SetInteger("AnimState", 1);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Debug.Log("On Jump");

        // ForceMode2D.Impulse was added since this is a sudden burst of force.
        // delta time was taken out since this only applies force on one frame anyway.
        // plus the delta time made the jump inconsistent.
        if(isGrounded)
            m_rigidBody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);

        // original (with delta time)
        // m_rigidBody2D.AddForce(Vector2.up * verticalForce * Time.deltaTime, ForceMode2D.Impulse);
        // m_animator.SetInteger("AnimState", 2);

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }
}