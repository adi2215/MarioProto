using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.down)) {
                Flatten();
            }
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EnemyMovement>().enabled = false;
        transform.localScale = new Vector3(1, 0.2f, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Destroy(gameObject, 0.5f);
    }

}
