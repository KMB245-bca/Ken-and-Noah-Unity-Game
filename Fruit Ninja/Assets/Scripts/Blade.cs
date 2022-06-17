using UnityEngine;
using UnityEngine.SceneManagement;

public class Blade : MonoBehaviour
{
    private UDPSocket input;
    private Camera mainCamera;
    private Collider bladeCollider;
    private TrailRenderer bladeTrail;
    private Rigidbody rb;
    private AudioManager audioManager;

    private bool slicing;

    public Vector3 direction { get; private set; }
    public float sliceForce = 5;
    public float minSliceVelocity = 0.01f;
    private Vector2 pos = new Vector2();

    private float maxComboTime = 0.3f;
    private float elapsedComboTime = 0f;
    public int comboCount = 0;

    public float velocity;
    private float minSoundVelocity = 3f;
    private static float minSoundTime = 1f;
    public float timeSinceSound = minSoundTime;

    public GameObject ComboText;

    private void Awake()
    {
        // input = GameObject.Find("Input Data").GetComponent<UDPSocket>();
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        timeSinceSound += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if (slicing)
        {
            ContinueSlicing();
        }

        // if (StartedSlicing(input.cursorDetected)) {
        //     StartSlicing();
        // }
        // else if (StoppedSlicing(input.cursorDetected)) {
        //     StopSlicing();
        // }
        // else if (slicing) {
        //     ContinueSlicing();
        // }

        if (slicing && timeSinceSound > minSoundTime && velocity > minSoundVelocity) {
            audioManager.Play("Sword Swipe " + Random.Range(1, 8).ToString());
            timeSinceSound = 0f;
        }

        elapsedComboTime += Time.deltaTime;
        if (comboCount >= 3 && elapsedComboTime > maxComboTime)
        {
            if (SceneManager.GetActiveScene().name == "Zen")
            {
                FindObjectOfType<GameManager_Zen>().AddPoints(comboCount);
            }
            else if (SceneManager.GetActiveScene().name == "Classic")
            {
                FindObjectOfType<GameManager_Classic>().AddPoints(comboCount);
            }
            string audioClip = "Combo ";
            if (comboCount < 10) {
                audioClip += comboCount.ToString();
            }
            else {
                audioClip += "10";
            }
            audioManager.Play(audioClip);
            ComboText.SetActive(true);
            comboCount = 0;
        }

        else if (elapsedComboTime > maxComboTime) {
            comboCount = 0;
        }
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    private void StartSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // Vector3 newPosition = input.pos;
        newPosition.z = 0;

        transform.position = newPosition;
        rb.position = newPosition;

        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSlicing()
    {
        // rb.velocity = new Vector2(0, 0);

        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
        timeSinceSound = minSoundTime;
    }

    private void ContinueSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // Vector3 newPosition = input.pos;
        newPosition.z = 0;

        direction = newPosition - transform.position;
        direction = direction.normalized;

        velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minSliceVelocity;

        // rb.position = newPosition;
        // transform.position = newPosition;

        Vector3 force = newPosition - rb.position;
        rb.velocity = force * 35;
    }

    private bool StartedSlicing(bool cursorDetected)
    {
        if (cursorDetected && !slicing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool StoppedSlicing(bool cursorDetected)
    {
        if (!cursorDetected && slicing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Fruit>())
        {
            elapsedComboTime = 0f;
            comboCount++;
        }
    }
}