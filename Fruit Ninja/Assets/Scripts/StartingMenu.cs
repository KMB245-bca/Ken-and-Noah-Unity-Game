using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Start() {
        SceneManager.LoadScene("SelectionScreen");
    }
}
