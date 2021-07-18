using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_E_skill : MonoBehaviour
{
    public AudioSource sound;
    public bool isBoss;
    public Text eText;
    public float cooltime; // 쿨타임
    public Image eImg;
    public LayerMask enemyLayer;
    Player player; // 플레이어 스킬 강화
    bool isCool = false;
    RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        isCool = false;

    }

    // Update is called once per frame
    void Update()
    {
        eText.text = ShopManager.isOpened[1].ToString();

        if (Input.GetKeyDown(KeyCode.E) && !isCool && ShopManager.isOpened[1] > 0)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, enemyLayer);
            if (hit.collider != null && hit.transform.gameObject.layer == 11 && !isBoss)
            {
                sound.Play();
                print(hit.transform.gameObject.layer);
                if(player.rainforceNextSkill)
                {
                    isCool = true;
                    ShopManager.isOpened[1]--;
                    hit.transform.gameObject.GetComponent<Enemy>().isStun = true;
                    hit.transform.gameObject.GetComponent<Animator>().enabled = false;
                    StartCoroutine(RainforceStun(hit));
                    StartCoroutine(CoolDown());

                }
                else
                {
                    isCool = true;
                    ShopManager.isOpened[1]--;
                    hit.transform.gameObject.GetComponent<Enemy>().isStun = true;
                    hit.transform.gameObject.GetComponent<Animator>().enabled = false;
                    StartCoroutine(Stun(hit));
                    StartCoroutine(CoolDown());
                }
            }
            else if (hit.collider != null && hit.transform.gameObject.layer == 11 && isBoss)
            {
                
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, enemyLayer);
                if (hit.collider != null)
                {
                    sound.Play();
                    if (player.rainforceNextSkill)
                    {
                        isCool = true;
                        ShopManager.isOpened[1]--;
                        hit.transform.gameObject.GetComponent<Boss>().isStun = true;
                        StartCoroutine(BossRainforceStun(hit));
                        StartCoroutine(CoolDown());

                    }
                    else
                    {
                        isCool = true;
                        ShopManager.isOpened[1]--;
                        hit.transform.gameObject.GetComponent<Boss>().isStun = true;
                        StartCoroutine(BossStun(hit));
                        StartCoroutine(CoolDown());
                    }
                }

            }
        }
    }

    IEnumerator Stun(RaycastHit2D hit)
    {
        yield return new WaitForSecondsRealtime(1);
        if(GameObject.Find(hit.transform.name) != null)
        {
            hit.transform.gameObject.GetComponent<Enemy>().isStun = false;
            hit.transform.gameObject.GetComponent<Animator>().enabled = true;

        }
    }
    IEnumerator RainforceStun(RaycastHit2D hit)
    {
        yield return new WaitForSecondsRealtime(3);
        if(GameObject.Find(hit.transform.name) != null)
        {
            hit.transform.gameObject.GetComponent<Enemy>().isStun = false;
            hit.transform.gameObject.GetComponent<Animator>().enabled = true;

        }
    }
    IEnumerator BossStun(RaycastHit2D hit)
    {
        yield return new WaitForSecondsRealtime(1);
        if (GameObject.Find(hit.transform.name) != null)
        {
            hit.transform.gameObject.GetComponent<Boss>().isStun = false;

        }
    }
    IEnumerator BossRainforceStun(RaycastHit2D hit)
    {
        yield return new WaitForSecondsRealtime(3);
        if (GameObject.Find(hit.transform.name) != null)
        {
            hit.transform.gameObject.GetComponent<Boss>().isStun = false;

        }
    }

    IEnumerator CoolDown() // 쿨타임 코루틴
    {
        for (float i = 0; i <= cooltime + Time.deltaTime; i += Time.deltaTime)
        {
            eImg.fillAmount = i / cooltime;
            yield return new WaitForEndOfFrame();
        }
        isCool = false;
    }

}
