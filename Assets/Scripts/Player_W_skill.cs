using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_W_skill : MonoBehaviour
{
    public AudioSource sound;
    public Text wText;
    public float cooltime; // 쿨타임
    public GameObject wSkill;
    public Image wImg;
    Player player; // 플레이어 스킬 강화
    bool isCool = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        isCool = false;

    }

    // Update is called once per frame
    void Update()
    {
        wText.text = ShopManager.isOpened[0].ToString();
        if (Input.GetKeyDown(KeyCode.Q) && !isCool && ShopManager.isOpened[0] > 0)
        {
            sound.Play();
            ShopManager.isOpened[0]--;
            wSkill.SetActive(true);
            StartCoroutine(WSetFalse());
            player.rainforceNextSkill = true;
            isCool = true;
            StartCoroutine(CoolDown());
        }

    }

    IEnumerator WSetFalse()
    {

        yield return new WaitForSecondsRealtime(3);
        player.rainforceNextSkill = false;

        wSkill.SetActive(false);

    }

    IEnumerator CoolDown() // 쿨타임 코루틴
    {
        for (float i = 0; i <= cooltime + Time.deltaTime; i += Time.deltaTime)
        {
            wImg.fillAmount = i / cooltime;
            yield return new WaitForEndOfFrame();
        }
        isCool = false;
    }

}
