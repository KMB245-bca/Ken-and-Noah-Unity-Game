using UnityEngine;
using UnityEngine.SceneManagement;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        audioManager.Play("Bomb Fuse");
    }

    private void OnDestroy() {
        audioManager.Stop("Bomb Fuse");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Blade blade = other.GetComponent<Blade>();
        if (other.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "Zen")
            {
                GameManager_Zen gameManager = FindObjectOfType<GameManager_Zen>();

                Instantiate(explosionPrefab, transform.position, transform.rotation);
                audioManager.Play("Bomb Explosion");

                gameManager.SubPoints(10);
                gameManager.ClearScene();
                gameManager.SubHP(1);
            }

            else if (SceneManager.GetActiveScene().name == "Classic") {
                audioManager.Play("Bomb Explosion Classic");
                FindObjectOfType<GameManager_Classic>().Explode();
            }
        }
    }
}
