using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float time;
    public bool isRainforce;
    void Start()
    {
        StartCoroutine(DestroyBullet()); // 일정 시간 후 삭제
    }


    void Update()
    {
        transform.Translate(transform.InverseTransformDirection((transform.right * speed * Time.deltaTime))); // 총알 이동
    }

    IEnumerator DestroyBullet() // 삭제 코루틴
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision) // 충돌 시 삭제
    {
        if(collision.tag.Equals("EnemyBullet") && isRainforce)
        {
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
