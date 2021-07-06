using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeDestroyer : MonoBehaviour {
    public Tilemap bridges;

    public Tile t0;
    public Tile t1;
    public Tile t2;

    private List<Vector3Int> destroyedBridges = new List<Vector3Int>();

    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var pos = new Vector3Int((int) Math.Floor(other.transform.position.x), (int) other.transform.position.y - 1, 0);
            if (!destroyedBridges.Contains(pos) && bridges.HasTile(pos)) {
                destroyedBridges.Add(pos);
                StartCoroutine(DestroyCoroutine(pos));
            }
        }
    }

    public void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var pos = new Vector3Int((int) Math.Floor(other.transform.position.x), (int) other.transform.position.y - 1, 0);
            if (!destroyedBridges.Contains(pos) && bridges.HasTile(pos)) {
                destroyedBridges.Add(pos);
                StartCoroutine(DestroyCoroutine(pos));
            }
        }
    }

    public IEnumerator DestroyCoroutine(Vector3Int pos) {
        bridges.SetTile(pos, t1);
        
        yield return new WaitForSeconds(0.2f);
        
        bridges.SetTile(pos, t2);
        
        yield return new WaitForSeconds(0.2f);
        
        bridges.SetTile(pos, null);
        
        yield return new WaitForSeconds(1.2f);
        
        bridges.SetTile(pos, t0);

        destroyedBridges.Remove(pos);
    }
}