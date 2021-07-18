using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int matIndex;
    public bool isStun;
    public float cooltime;
    public bool isCool;
    public bool isRight = false; // 보는 방향 체크
    public bool isRightInverse = false;
    public float moveSpeed; // 적 이동속도
    RaycastHit2D raycastHit2D;
    SpriteRenderer sprite;
    public LayerMask groundLayerMask; // 땅 레이어
    public LayerMask playerLayerMask; // 플레이어 레이어
    public float offset; // 바닥에서부터 오프셋
    public float raycastOffsetX; // 레이캐스트 오프셋
    public float raycastOffsetY;
    public float hp; // 체력
    public float dist; // 추적 거리
    public GameObject bullet;
    public GameObject eSkill;
    ParticleSystem particle;
    bool isDieCorutine;
    GameObject player;
    RaycastHit2D raycastHit;
    public bool tmp;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 11;
        isCool = false;
        GetComponent<PolygonCollider2D>().isTrigger = false;
        player = GameObject.FindGameObjectWithTag("Player");
        isDieCorutine = false;
        particle = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
        raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down); // 바닥으로부터 오프셋 만큼 띄움
        transform.position = new Vector2(transform.position.x, raycastHit2D.point.y + offset);
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0 && !isDieCorutine) // 죽을 때 한번 실행
        {
            gameObject.layer = 15;
            isStun = false;
            eSkill.SetActive(false);

            ShopManager.coins += Random.Range(1, 4);//1~3골드 랜덤 획득
            if(Random.Range(0, 10) < 3)
            {
                ShopManager.mat[matIndex]++;
            }
            Debug.Log(ShopManager.mat[0] + " " + ShopManager.mat[1] + " " + ShopManager.mat[2]);
            Debug.Log(ShopManager.coins);


            GetComponent<PolygonCollider2D>().isTrigger = true;
            particle.Play();
            isDieCorutine = true;
            StartCoroutine(DieCorutine());
        }
        else if(isStun)
        {
            eSkill.SetActive(true);
        }
        else if(hp > 0)
        {
            eSkill.SetActive(false);

            //raycastHit = Physics2D.Linecast(transform.position, player.transform.position);
            if (Vector2.Distance(transform.position, player.transform.position) <= dist/* && raycastHit != null*/) // 일정 거리 내에 플레이어가 있을 때
            {
                if (transform.position.x < player.transform.position.x)
                {
                    isRight = true;
                    sprite.flipX = isRightInverse;
                }
                else
                {
                    isRight = false;
                    sprite.flipX = !isRightInverse;

                }
                if (!isCool) // 쿨타임이 아니면 총알 발사
                {
                    isCool = true;
                    StartCoroutine(EnemyCoolDown());
                    Vector3 target = player.transform.position;
                    float dy = target.y - transform.position.y;
                    float dx = target.x - transform.position.x;
                    float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
                    Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, rotateDegree));
                }
            }
            else
            {
                if (isRight) // 오른쪽을 볼 때
                {
                    sprite.flipX = isRightInverse; // 스프라이트 회전
                    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); // 이동
                    Debug.DrawRay(transform.position + Vector3.right * raycastOffsetX, Vector2.down * (raycastOffsetY + offset));
                    Debug.DrawRay(transform.position + ((!tmp) ? Vector3.down : Vector3.zero), Vector2.right);

                    if (!Physics2D.Raycast(transform.position + Vector3.right * raycastOffsetX, Vector2.down, raycastOffsetY + offset, groundLayerMask)) // 갈 곳이 없으면 회전
                    {
                        isRight = !isRight;
                    }
                    else if (Physics2D.Raycast(transform.position + ((!tmp) ? Vector3.down : Vector3.zero), Vector2.right, 1, groundLayerMask))
                    {
                        isRight = !isRight;

                    }
                }
                else // 왼쪽을 볼 때
                {
                    sprite.flipX = !isRightInverse;
                    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                    Debug.DrawRay(transform.position + Vector3.left * raycastOffsetX, Vector2.down * (raycastOffsetY + offset));
                    Debug.DrawRay(transform.position + ((!tmp) ? Vector3.down : Vector3.zero), Vector2.left);
                    if (!Physics2D.Raycast(transform.position + Vector3.left * raycastOffsetX, Vector2.down, raycastOffsetY + offset, groundLayerMask))
                    {
                        isRight = !isRight;
                    }
                    else if (Physics2D.Raycast(transform.position + ((!tmp) ? Vector3.down : Vector3.zero), Vector2.left, 1, groundLayerMask))
                    {
                        isRight = !isRight;

                    }

                }

            }
        }
    }

    IEnumerator DieCorutine() // 죽을 때 페이드 아웃
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) // 총알 맞으면 삭제
    {
        if(collision.tag.Equals("Bullet"))
        {
            hp -= 1;
        }
        if(collision.tag.Equals("BulletRainforce")) // 강화 총알 맞으면 체력 -2
        {
            hp -= 2;
        }
    }

    IEnumerator EnemyCoolDown() // 쿨타임 코루틴
    {
        yield return new WaitForSecondsRealtime(cooltime);
        isCool = false;
    }

}
