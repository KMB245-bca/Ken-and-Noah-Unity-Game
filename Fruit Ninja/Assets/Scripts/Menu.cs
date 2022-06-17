using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("SelectionScreen");
    }
}
