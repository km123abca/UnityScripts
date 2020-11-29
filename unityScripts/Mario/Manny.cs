using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manny : MonoBehaviour
{
    public float speed = 5;
    public bool facingRight = true;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    public float jumpSpeed = 5f;
    bool isJumping = false;
    private float rayCastLength = 0.005f;
    private float width;
    public float height;
    private float jumpButtonPressTime;
    private float maxJumpTime = 1f;
    public float wallJumpY = 10f;

    public bool isWallOnLeft()
    {
        return Physics2D.Raycast(
                                 new Vector2(transform.position.x - width, transform.position.y),
                                 -Vector2.right,
                                 rayCastLength
                                );
    }
    public bool isWallOnRight()
    {
        return Physics2D.Raycast(
                                 new Vector2(transform.position.x + width, transform.position.y),
                                 Vector2.right,
                                 rayCastLength
                                );
    }
    public bool isAWallNearBy()
    {
        return isWallOnRight() || isWallOnLeft();
    }
    public int getWallDir()
    {
        if (isWallOnLeft()) return -1;
        else if (isWallOnRight()) return 1;
        else return 0;
    }
    void FixedUpdate()
    {
        float horzMove = Input.GetAxisRaw("Horizontal");
        Vector2 vect = rb.velocity;
        rb.velocity = new Vector2(horzMove * speed, vect.y);
        if (isAWallNearBy() && !isOnGround() && Mathf.Abs(horzMove) == 1)
        {
            // rb.velocity = new Vector2(getWallDir() * speed * 0.75f, wallJumpY); //Banas's version
            rb.velocity = new Vector2(0, wallJumpY);
            // Debug.Log("Wall Jumping activated");    
        }
        animator.SetFloat("Speed", Mathf.Abs(horzMove));

        if ((horzMove > 0 && !facingRight) || (horzMove < 0 && facingRight))
        {
            FlipManny();
        }

        float vertMove = Input.GetAxis("Jump");
        if (isOnGround() && !isJumping && (vertMove > 0))
        {
            isJumping = true;
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.jump);
        }
        if (jumpButtonPressTime > maxJumpTime)
            vertMove = 0f;
        if (isJumping && (jumpButtonPressTime < maxJumpTime))
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        if (vertMove >= 1f)
            jumpButtonPressTime += Time.deltaTime;
        else
        {
            isJumping = false;
            jumpButtonPressTime = 0f;
        }
    }
    public bool isOnGround()
    {
        bool groundCheck1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - height), -Vector2.up, rayCastLength);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(transform.position.x + (width - 0.2f), transform.position.y - height), -Vector2.up, rayCastLength);
        bool groundCheck3 = Physics2D.Raycast(new Vector2(transform.position.x - (width - 0.2f), transform.position.y - height), -Vector2.up, rayCastLength);
        if (groundCheck1 || groundCheck2 || groundCheck3)
            return true;
        return false;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void FlipManny()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.1f;

    }
    void OnCollisionEnter2D(Collision2D cod)
    {
        // Debug.Log("collision with " + cod.collider.tag);
    }

}
