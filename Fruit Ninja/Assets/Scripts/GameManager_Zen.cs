using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_Zen : MonoBehaviour
{
    public Text scoreText;
    public Text hpText;
    public Text timeText;

    private Blade blade;
    private Spawner spawner; 
    private CountDownTimer countDownTimer;
    private AudioManager audioManager;

    private int score;
    private int hp;


    private void Awake() {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        countDownTimer = FindObjectOfType<CountDownTimer>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        NewGame();
    }

    private void Update() {
        timeText.text = countDownTimer.min_str + ":" + countDownTimer.sec_str + ":" + countDownTimer.centisec_str;    

        if (hp <= 0 || countDownTimer.currentTime <= 0) {
            GameOver();
        }
    }

    private void NewGame() {
        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = "Score: " + score.ToString();

        hp = 3;
        hpText.text = "HP: " + hp.ToString();

        ClearScene();
    }

    private void GameOver() {
        countDownTimer.Stop();
        spawner.enabled = false;
        ClearScene();
        StartCoroutine(SwitchScene());
    }
    
    private IEnumerator SwitchScene() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SelectionScreen");
        audioManager.Play("Game Over");
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

    public void AddHP(int points) {
        hp += points;
    }

    public void SubHP(int points) {
        if (points > hp) {
            hp = 0;
        }
        else {
            hp -= points;
        }
        hpText.text = "HP: " + hp.ToString();
    }
}