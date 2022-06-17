using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem juiceParticleEffect;
    private AudioManager audioManager;


    private void Awake() {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceParticleEffect = GetComponentInChildren<ParticleSystem>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force) {
        if (SceneManager.GetActiveScene().name == "Zen") {
            FindObjectOfType<GameManager_Zen>().AddPoints(1);
        }
        else if (SceneManager.GetActiveScene().name == "Classic") {
            FindObjectOfType<GameManager_Classic>().AddPoints(1);
        }

        whole.SetActive(false);
        sliced.SetActive(true);

        fruitCollider.enabled = false;
        juiceParticleEffect.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody slice in slices) {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
        PlaySound();
    }

    private void OnTriggerEnter(Collider other) {
        Blade blade = other.GetComponent<Blade>();
        if (other.CompareTag("Player")) {
            Slice(blade.direction, blade.transform.position, blade.sliceForce);

            if (SceneManager.GetActiveScene().name == "SelectionScreen") {
                audioManager.Play("Watermelon Sliced");
                StartCoroutine(SwitchScenes());
            }
        }
    }

        private IEnumerator SwitchScenes() {
        yield return new WaitForSeconds(1f);

        if (tag == "Menu Fruit Zen") {
            SceneManager.LoadScene("Zen");
        }

        else if (tag == "Menu Fruit Classic") {
            SceneManager.LoadScene("Classic");
        }
        audioManager.Play("Game Start");
    }

    private void PlaySound() {
        if (tag == "Apple") {
            audioManager.Play("Apple Sliced");
        }
        else if (tag == "Kiwi") {
            audioManager.Play("Kiwi Sliced");
        }
        else if (tag == "Orange") {
            audioManager.Play("Orange Sliced");
        }
        else if (tag == "Lemon") {
            audioManager.Play("Lemon Sliced");
        }
        else if (tag == "Watermelon") {
            audioManager.Play("Watermelon Sliced");
        }
    }
}