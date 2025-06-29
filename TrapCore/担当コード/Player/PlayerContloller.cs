using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    private CapsuleCollider2D boxCollider;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        isGrounded = IsGrounded();
        isTouchingWall = IsTouchingWall();

        float moveInput = 0;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) moveInput = 1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) moveInput = -1;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
     
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        float playerHalfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        // �J�����̍��[�i���[���h���W�j
        float leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;

        // �v���C���[�́u���[�v�����ɏo�Ȃ��悤�ɐ���
        if (pos.x - playerHalfWidth < leftLimit)
        {
            pos.x = leftLimit + playerHalfWidth;
        }

        transform.position = pos;
    }


    bool IsGrounded()
    {
        float colliderWidth = boxCollider.bounds.extents.x;
        float VerticalWidth = boxCollider.bounds.extents.y;
        Vector2 left = new Vector2(transform.position.x - colliderWidth + 0.05f, transform.position.y);
        Vector2 right = new Vector2(transform.position.x + colliderWidth - 0.05f, transform.position.y);

        RaycastHit2D hitLeft = Physics2D.Raycast(left, Vector2.down,VerticalWidth + 0.3f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(right, Vector2.down, VerticalWidth + 0.3f, groundLayer);

        return hitLeft.collider != null || hitRight.collider != null;
    }

    bool IsTouchingWall()
    {
        float colliderHeight = boxCollider.bounds.extents.y;
        Vector2 top = new Vector2(transform.position.x, transform.position.y + colliderHeight - 0.05f);
        Vector2 bottom = new Vector2(transform.position.x, transform.position.y - colliderHeight + 0.05f);

        RaycastHit2D hitLeft = Physics2D.Raycast(top, Vector2.left, 0.6f, groundLayer);
        RaycastHit2D hitLeftBottom = Physics2D.Raycast(bottom, Vector2.left, 0.6f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(top, Vector2.right, 0.6f, groundLayer);
        RaycastHit2D hitRightBottom = Physics2D.Raycast(bottom, Vector2.right, 0.6f, groundLayer);

        return hitLeft.collider != null || hitLeftBottom.collider != null ||
               hitRight.collider != null || hitRightBottom.collider != null;
    }

    private void OnDestroy()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // クリアフラグを触ったら
        if(collision.tag == "Clear")
        {
            // クリアシーンに移行
            SceneManager.LoadScene("Clear");
        }
    }
}
