using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioSource jumpsound;
    public Text[] mat;
    public bool godMod = false;
    public GameObject[] hearts;
    public static float hp = 1; // 체력
    public bool rainforceNextSkill = false;
    public bool isRight = false;
    Animator anim;
    Rigidbody2D rig;
    SpriteRenderer sprite;
    public float normalSpeed;
    public float skillSpeed;
    public float moveSpeed; // 플레이어 이동속도
    public float raycastDist; // 레이캐스트 거리
    public LayerMask layer; // 바닥 레이어 받기
    public float jumpSpeed; // 점프 스피드
    public float jumpCoolDown; // 점프 쿨타임
    public bool isIdleCorutine;
    bool isJump; // 점프 가능한지 확인
    public float idleTime; // 휴식으로 넘어가는 시간
    IEnumerator idleCorutine;
    bool isDieCorutine;
    ParticleSystem particle;
    public GameObject SceneManager;
    public Text coin;
    public GameObject stageManage;
    private int preCoin = ShopManager.coins;


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = normalSpeed;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        isDieCorutine = false;
        rainforceNextSkill = false;
        anim = GetComponent<Animator>();
        isJump = true;
        sprite = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        idleCorutine = Idle();
        particle = GetComponent<ParticleSystem>();

    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rig.velocity.y); // 이동 로직
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stageManage.SetActive(!stageManage.activeInHierarchy);
        }
        for(int i = 0; i < 3; i++)
        {
            mat[i].text = ShopManager.mat[i].ToString();
        }
        coin.text = ": " + ShopManager.coins.ToString();
        hearts[0].gameObject.SetActive(false);
        hearts[1].gameObject.SetActive(false);
        hearts[2].gameObject.SetActive(false);
        if (hp >= 1)
        {
            hearts[0].gameObject.SetActive(true);
        }
        if (hp >= 2)
        {
            hearts[1].gameObject.SetActive(true);
        }
        if (hp >= 3)
        {
            hearts[2].gameObject.SetActive(true);
        }

        if (hp <= 0 && !isDieCorutine) // 죽을 때 한번 실행
        {
            ShopManager.coins = preCoin;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().isTrigger = true;
            particle.Play();
            isDieCorutine = true;
            StartCoroutine(DieCorutine());

        }
        else if (hp > 0)
        {

            if (rig.velocity == Vector2.zero && !isIdleCorutine) // 일정시간 후 휴식 모션
            {
                anim.SetBool("IsMove", false);
                anim.SetBool("IsMoveBack", false);

                idleCorutine = Idle();
                StartCoroutine(idleCorutine);
            }
            else if (rig.velocity != Vector2.zero) // 이동 관련 모션
            {
                anim.SetBool("IsMove", true);

                if ((isRight == true && rig.velocity.x > 0) || (isRight == false && rig.velocity.x < 0))
                {
                    anim.SetBool("IsMoveBack", false);
                    anim.SetBool("IsMoveForward", true);

                }
                else
                {
                    anim.SetBool("IsMoveForward", false);
                    anim.SetBool("IsMoveBack", true);

                }
                StopCoroutine(idleCorutine);
                anim.SetBool("Idle", false);
                isIdleCorutine = false;
            }
            else
            {
                anim.SetBool("IsMove", false);
                anim.SetBool("IsMoveBack", false);
                anim.SetBool("IsMoveForward", false);

            }
            if (transform.position.x < Camera.main.ScreenToWorldPoint(Input.mousePosition).x) // 스프라이트 로직
            {
                isRight = true;
                sprite.flipX = false;
            }
            else
            {
                isRight = false;
                sprite.flipX = true;
            }
            Debug.DrawRay(transform.position, Vector2.down * raycastDist); // 레이캐스트 디버그
            if (Physics2D.Raycast(transform.position, Vector2.down, raycastDist, layer)) // 점프 레이캐스트
            {
                if (Input.GetAxisRaw("Jump") != 0 && isJump) // 점프
                {
                    jumpsound.Play();
                    StartCoroutine(JumpCoolDown());
                    isJump = false;
                    rig.velocity = new Vector2(rig.velocity.x, 0);
                    rig.AddForce(Vector2.up * jumpSpeed);
                }
            }
        }
    }

   

    IEnumerator JumpCoolDown() // 점프 쿨다운
    {
        yield return new WaitForSecondsRealtime(jumpCoolDown);
        isJump = true;
    }

    IEnumerator Idle() // 휴식 모션 코루틴
    {
        isIdleCorutine = true;
        yield return new WaitForSecondsRealtime(3);
        anim.SetBool("Idle", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("EnemyBullet") && !godMod)
        {
            hp -= 1;
        }
        if (collision.transform.tag.Equals("Finish"))
        {
            SceneManager.GetComponent<SceneChange>().GoShop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Enemy") && !godMod)
        {
            hp = 0;
        }
    }



    IEnumerator DieCorutine() // 죽을 때 페이드 아웃
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        switch (ShopManager.presentStage) {
            case 1:
                SceneManager.GetComponent<SceneChange>().GameOver1();
                break;
            case 2:
                SceneManager.GetComponent<SceneChange>().GameOver2();
                break;
            case 3:
                SceneManager.GetComponent<SceneChange>().GameOver3();
                break;
        }
    }

}
