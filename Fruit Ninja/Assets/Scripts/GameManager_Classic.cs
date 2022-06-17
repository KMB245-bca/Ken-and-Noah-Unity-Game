using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Classic : MonoBehaviour
{
    public Text scoreText;

    public Image fadeImage;

    private Blade blade;
    private Spawner spawner; 
    private AudioManager audioManager;

    private int score;


    private void Awake() {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        NewGame();
    }

    private void NewGame() {
        Time.timeScale = 1f;

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = "Score: " + score.ToString();

        ClearScene();
    }

    public void ClearScene() {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach(Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }


        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach(Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }
    
    public void AddPoints(int points) {
        score += points;
        scoreText.text = "Score: " + score.ToString();
    }

    public void SubPoints(int points) {
        if (points > score) {
            score = 0;
        }
        else {
            score -= points;
        }
        scoreText.text = "Score: " + score.ToString();
    }

    public void Explode() {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence() {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration) {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0f;

        while (elapsed < duration) {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
        SceneManager.LoadScene("SelectionScreen");
        audioManager.Play("Game Over");
    }
}