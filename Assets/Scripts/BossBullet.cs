using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyThis());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speed);    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Bullet") && collision.tag.Equals("BulletRainforce"))
        {
            Destroy(gameObject);

        }
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSecondsRealtime(2);
        Destroy(gameObject);
    }
}
