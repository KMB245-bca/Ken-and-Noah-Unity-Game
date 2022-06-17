using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboText : MonoBehaviour
{
    public Text comboText;
    public float duration = 1f;

    private void OnEnable() {
        Blade blade = FindObjectOfType<Blade>();
        comboText.text = blade.comboCount.ToString() + " Fruit Combo" + "\n+" + blade.comboCount.ToString();
        comboText.transform.position = FindObjectOfType<Camera>().WorldToScreenPoint(blade.transform.position);
        StartCoroutine(Disable());
    }

    private IEnumerator Disable() {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
