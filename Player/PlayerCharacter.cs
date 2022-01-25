using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public float movePower = 1f;
    public float jumpPower = 1f;
    Vector3 curScale;

    Rigidbody2D rigid;
    Animator animator;

    Vector3 movement;
    bool isJumping;
    bool canJumping = true;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();

        curScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else
            animator.SetInteger("Direction", 0);

       if (Input.GetButtonDown("Jump"))
       {
            if (canJumping == true)
            {
                canJumping = false;
                isJumping = true;
                animator.SetBool("isJumping", true);
                animator.SetTrigger("canJumping");
            }
            
       }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    // 이동관련 함수
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw ("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-curScale.x, curScale.y, curScale.z);
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(curScale.x, curScale.y, curScale.z);
        }


        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping)
            return;

        
        rigid.velocity = Vector2.zero;
       
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;

    }

    // 콜라이더 트리거 판정
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attach : " + collision.gameObject.layer);

        if (collision.gameObject.layer == 6 && rigid.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            canJumping = true;
        }

    }

        private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Detach : " + collision.gameObject.layer);
    }
}
