using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GameStart(int num) // 스테이지 시작
    {
        ShopManager.presentStage = num;
        SceneManager.LoadScene("Stage " + num.ToString());
    }

    public void GameStartFromShop() // 상점에서 다음 스테이지 시작
    {
        ShopManager.presentStage++;
        SceneManager.LoadScene("Stage " + ShopManager.presentStage.ToString());
    }

    public void GoShop() // 상점이동
    {
        SceneManager.LoadScene("Shop");
    }

    public void GameOver1() // 1 스테이지 게임오버
    {
        SceneManager.LoadScene("GameOverScene 1");
        Player.hp = 1;
    }

    public void GameOver2() // 2 스테이지 게임오버
    {
        SceneManager.LoadScene("GameOverScene 2");
        Player.hp = 1;
    }

    public void GameOver3() // 3 스테이지 게임오버
    {
        SceneManager.LoadScene("GameOverScene 3");
        Player.hp = 1;
    }

    public void GameOver4() // 4 스테이지 게임오버
    {
        SceneManager.LoadScene("GameOverScene 4");
        Player.hp = 1;
    }
}
