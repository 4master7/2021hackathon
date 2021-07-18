using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_R_skill : MonoBehaviour
{
    public AudioSource sound;
    public float cooltime; // 쿨타임
    Player player; // 플레이어 스킬 강화
    bool isCool = false;
    public Text rText;
    SpriteRenderer sprite;
    public Image rImg;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        isCool = false;

    }

    // Update is called once per frame
    void Update()
    {
        rText.text = ShopManager.isOpened[2].ToString();
        if (Input.GetKeyDown(KeyCode.R) && !isCool && ShopManager.isOpened[2] > 0)
        {
            sound.Play();
            if (player.rainforceNextSkill)
            {
                player.moveSpeed = player.skillSpeed;
            }
            GetComponent<Player>().godMod = true;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.3f);
            ShopManager.isOpened[2]--;
            gameObject.layer = 14;
            StartCoroutine(RSetFalse());
            isCool = true;
            StartCoroutine(CoolDown());
        }

    }

    IEnumerator RSetFalse()
    {
        yield return new WaitForSecondsRealtime(3);
        player.moveSpeed = player.normalSpeed;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        GetComponent<Player>().godMod = false;
        gameObject.layer = 10;

    }


    IEnumerator CoolDown() // 쿨타임 코루틴
    {
        for (float i = 0; i <= cooltime + Time.deltaTime; i += Time.deltaTime)
        {
            rImg.fillAmount = i / cooltime;
            yield return new WaitForEndOfFrame();
        }
        isCool = false;
    }

}
