using UnityEngine;

public class Parallax : MonoBehaviour {
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    void Start() {
        startpos = transform.position.x;
        
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void FixedUpdate() {
        var temp = cam.transform.position.x * (1 - parallaxEffect);
        var dist = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startpos + dist, cam.transform.position.y, transform.position.z);

        if (temp > startpos + length) {
            startpos += length;
        } else if (temp < startpos - length) {
            startpos -= length;
        }
    }
}