using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishController : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI time;

    public void OnCollisionEnter2D(Collision2D other) {
        panel.SetActive(true);
        Time.timeScale = 0f;

        var t = Time.time;
        time.text = ((int) t / 60) + "m:" + ((int) t % 60) + "s";

        deaths.text = PlayerMovement.deathCounter + " deaths";
    }

    public void Again() {
        Time.timeScale = 1f;
        LevelManager.timeToNextMutation = 10;
        SceneManager.LoadScene("MainScene");
    }
}