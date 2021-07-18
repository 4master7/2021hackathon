using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int hp = 15;
    public Vector2 pos;
    public bool isStun = false;
    public GameObject eSkill;
    public bool isCorutine = false;
    public GameObject[] bullet;
    public Image hpBar;
    public ParticleSystem particle;
    SpriteRenderer sprite;
    bool isDieCorutine;


    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector2(Random.Range(-17, 18), 14);
        particle = GetComponent<ParticleSystem>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = hp / 15.0f;
        if (hp <= 0 && !isDieCorutine) // 죽을 때 한번 실행
        {
            eSkill.SetActive(false);

            gameObject.layer = 15;
            isStun = false;
            eSkill.SetActive(false);



            GetComponent<BoxCollider2D>().isTrigger = true;
            particle.Play();
            isDieCorutine = true;
            StartCoroutine(DieCorutine());
        }
        else if(hp > 0)
        {
            if (!isStun)
            {
                eSkill.SetActive(false);

                transform.position = Vector2.Lerp(transform.position, pos, 0.1f);
                if (Vector2.Distance(transform.position, pos) < 0.1f && !isCorutine)
                {
                    isCorutine = true;
                    Instantiate(bullet[i++], transform.position + Vector3.down * 0.5f, Quaternion.Euler(Vector3.zero));
                    i %= bullet.Length;

                    StartCoroutine(Cool());
                }
            }
            else if (isStun)
            {
                eSkill.SetActive(true);
            }

        }

    }
    IEnumerator Cool()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        pos = new Vector2(Random.Range(-17, 18), 14);
        isCorutine = false;
    }
    private void OnTriggerEnter2D(Collider2D collision) // 총알 맞으면 삭제
    {
        if (collision.tag.Equals("Bullet"))
        {
            hp -= 1;
        }
        if (collision.tag.Equals("BulletRainforce")) // 강화 총알 맞으면 체력 -2
        {
            hp -= 2;
        }
    }
    IEnumerator DieCorutine() // 죽을 때 페이드 아웃
    {
        for (float i = 0; i < 3; i += Time.deltaTime)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
        }
        SceneManager.LoadScene("Completed");
        Destroy(gameObject);
    }

}
