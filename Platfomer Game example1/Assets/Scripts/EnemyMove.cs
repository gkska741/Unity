using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circlecollider;
    public int nextMove;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circlecollider = GetComponent<CircleCollider2D>();
        Invoke("Think", 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Vector2 downvec = new Vector2(0, -2);
        Debug.DrawRay(frontVec, downvec, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1);
        {
            if (rayHit.collider == null)
            {
                Turn();
            }
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);

        float thinktime = Random.Range(2f, 4f);
        Invoke("Think", thinktime);

        if (nextMove != 0)
        {
            Turn();
        }
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        circlecollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("DeActive", 3);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
