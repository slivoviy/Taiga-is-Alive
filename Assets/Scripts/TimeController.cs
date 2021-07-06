using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {
    public Image fillment;
    public TextMeshProUGUI timeLeft;

    private int time;
    private Vector3 newScale;
    private Vector3 startScale;
    private float speed;
    
    void Start() {
        speed = 496f / LevelManager.timeToNextMutation;
        startScale = fillment.transform.localScale;
        newScale = new Vector3(496f, startScale.y, startScale.z);
        time = LevelManager.timeToNextMutation;
        StartCoroutine(CountTime());
    }

    private IEnumerator CountTime() {
        while (true) {
            yield return new WaitForSeconds(1);
            
            time -= 1;
            timeLeft.text = time + "s";
            fillment.transform.localScale = new Vector3(fillment.transform.localScale.x + speed, fillment.transform.localScale.y, fillment.transform.localScale.z);

            if (time == 0) {
                time = LevelManager.timeToNextMutation;
                timeLeft.text = time + "s";
                speed = 496f / time;
                fillment.transform.localScale = startScale;
            }
        }
    }
}