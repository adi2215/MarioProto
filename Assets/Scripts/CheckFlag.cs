using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckFlag : MonoBehaviour
{
    public GameObject image;
    public GameObject player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            image.SetActive(true);
            player.GetComponent<PlayerMovement>().enabled = false;
            Invoke("NewScene", 2f);
        }
    }

    private void NewScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
