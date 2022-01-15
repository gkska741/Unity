using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public float maxSpeed;
    public float jumpPower;


    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                Debug.Log("ATTACK");
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }
    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Mathf.Abs(rigid.velocity.x) < 0.2)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h * 2, ForceMode2D.Impulse);
        //horizontal movement
        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1)) {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                PlaySound("ATTACK");
                gameManager.stagePoint += 200;
                OnAttack(collision.transform);
            }
            else
                OnDamaged(collision.transform.position);
                PlaySound("DAMAGED");
        }
        else if (collision.gameObject.tag =="Spike")
        {
            OnDamaged(collision.transform.position);
            PlaySound("DAMAGED");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBronze)
            {
                gameManager.stagePoint += 20;
            }
            else if (isSilver)
            {
                gameManager.stagePoint += 50;
            }
            else if (isGold)
            {
                gameManager.stagePoint += 100;
            }

            collision.gameObject.SetActive(false);
            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
            PlaySound("FINISH");
        }
    }
    void OnAttack(Transform enemy)
    {
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPosition)
    {

        gameManager.HealthDown();
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 0.3f);

        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 6;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
