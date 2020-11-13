using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public bool isGrounded;
    public float verticalForce;
    public float horizontalForce;

    private Vector3 direction;

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
        if(isGrounded)
        {
            m_rigidBody2D.AddForce(direction * horizontalForce * Time.deltaTime);

            if (Mathf.Abs(m_rigidBody2D.velocity.x) > 0.1F)
            {
                m_animator.SetInteger("AnimState", 1);
            }

            m_rigidBody2D.velocity *= 0.95F;
        }

        if (Mathf.Approximately(m_rigidBody2D.velocity.x, 0.0F))
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

            default:
                direction = Vector2.zero;
                break;
        }

        m_animator.SetInteger("AnimState", 1);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Debug.Log("On Jump");

        // ForceMode2D was added
        if(isGrounded)
            m_rigidBody2D.AddForce(Vector2.up * verticalForce * Time.deltaTime, ForceMode2D.Impulse);

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