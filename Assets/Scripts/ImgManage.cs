using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgManage : MonoBehaviour
{
    public Text[] mat;
    public Text coin;
    public Text q;
    public Text e;
    public Text r;
    public GameObject[] hearts;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            mat[i].text = ShopManager.mat[i].ToString();
        }

        q.text = ShopManager.isOpened[0].ToString();
        e.text = ShopManager.isOpened[1].ToString();
        r.text = ShopManager.isOpened[2].ToString();
        coin.text = ": " + ShopManager.coins.ToString();
        hearts[0].gameObject.SetActive(false);
        hearts[1].gameObject.SetActive(false);
        hearts[2].gameObject.SetActive(false);
        if (Player.hp >= 1)
        {
            hearts[0].gameObject.SetActive(true);
        }
        if (Player.hp >= 2)
        {
            hearts[1].gameObject.SetActive(true);
        }
        if (Player.hp >= 3)
        {
            hearts[2].gameObject.SetActive(true);
        }



    }
}
