using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Q_skill : MonoBehaviour
{
    public AudioSource sound;
    Player player; // 플레이어 스크립트 isRight 받아옴
    public GameObject wSkill; // 강화 취소
    public GameObject bullet; // 총알 프리팹
    public GameObject bulletRainforce; // 강화 총알 프리팹
    public float cooltime; // 쿨타임
    public Image qImg;
    bool isCool = false;
    GameObject tmp;

    private void Start()
    {
        player = GetComponent<Player>();
        isCool = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 mPosition = Input.mousePosition; // 마우스 바라보게 생성
        mPosition.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);
        float dy = target.y - transform.position.y;
        float dx = target.x - transform.position.x;
        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        if (Input.GetMouseButtonDown(0) && !isCool && player.rainforceNextSkill == true) // 강화 총알 발사
        {
            sound.Play();
            wSkill.SetActive(false);
            player.rainforceNextSkill = false;
            tmp = Instantiate(bulletRainforce, transform.position + ((player.isRight)? Vector3.right * 0.5f : Vector3.left * 0.5f), Quaternion.Euler(0f, 0f, rotateDegree));
            if (!player.isRight)
            {
                tmp.GetComponent<SpriteRenderer>().flipY = true; // 반대편으로 쏠 때 회전시킴
            }
            isCool = true;
            StartCoroutine(CoolDown());

        }

        else if (Input.GetMouseButtonDown(0) && !isCool)
        {
            sound.Play();
            tmp = Instantiate(bullet, transform.position + ((player.isRight) ? Vector3.right * 0.5f : Vector3.left * 0.5f), Quaternion.Euler(0f, 0f, rotateDegree));
            if (!player.isRight)
            {
                tmp.GetComponent<SpriteRenderer>().flipY = true; // 반대편으로 쏠 때 회전시킴
            }
            isCool = true;
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown() // 쿨타임 코루틴
    {
        for (float i = 0; i <= cooltime + Time.deltaTime; i += Time.deltaTime)
        {
            qImg.fillAmount = i / cooltime;
            yield return new WaitForEndOfFrame();
        }
        isCool = false;
    }
}
