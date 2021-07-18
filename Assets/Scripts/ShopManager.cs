using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static int[] isOpened = { 2, 2, 2 };//순서대로 스킬1,2,3
    public static int[] mat = { 0, 0, 0 }; // 종이, 석탄, 표지판
    public static int coins = 0;//코인
    public static int presentStage = 0;//어느 스테이지서 왔는지
    private int[] costs = { 20, 30, 40, 20 };//순서대로 스킬1,2,3, 목숨값
    private int[] ResourcesCosts = { 5, 5, 5 };
    public GameObject[] Costinfo;

    // Start is called before the first frame update
    void Start()
    {
        if (presentStage == 1)
        {
            costs[0] = 10;
            ResourcesCosts[1] = 10;
        }
        else if (presentStage == 2)
        {
            costs[1] = 10;
            ResourcesCosts[2] = 10;
        }
        else if (presentStage == 3)
        {
            costs[2] = 10;
            ResourcesCosts[0] = 10;
        }

        for(int i=0; i<4; i++)//가격표 갱신
        {
            Costinfo[i].transform.Find("cost").GetComponent<Text>().text = "cost : " + costs[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SellAll()
    {
        int tmp = mat[0] * ResourcesCosts[0] + mat[1] * ResourcesCosts[1] + mat[2] * ResourcesCosts[2];
        coins += tmp;
        mat[0] = 0;
        mat[1] = 0;
        mat[2] = 0;
    }
    public void PurchaseSkill(int num)//1번스킬 -> num=1, index=0
    {
        if (coins >= costs[num-1])//구매성사
        {
            if (num != 4)//스킬구매
            {
                isOpened[num - 1] += 1;
            }
            else//목숨구매
            {
                if (Player.hp == 3)//최대체력 3 초과 불가
                {
                    Debug.Log("체력이 꽉찼다!");
                    return;
                }
                Player.hp += 1;
            }
            coins -= costs[num - 1];
        }
        else//못 사 임마!
        {
            Debug.Log(num + " 돈 이 없 다!");
        }
    }
}
