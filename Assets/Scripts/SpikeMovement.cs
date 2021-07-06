using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeMovement : MonoBehaviour {
    public Tilemap movingSpikes;
    public Collider2D collider2d;
    public Tilemap map;


    public Tile t0;
    public Tile t1;

    private List<Vector3Int> spikesPos = new List<Vector3Int>();

    private readonly Vector3Int zero = LevelManager.Zero;

    private int sizeX;
    private int sizeY;

    public void Start() {
        sizeX = map.size.x;
        sizeY = map.size.y;

        collider2d.enabled = true;

        StartCoroutine(MoveCoroutine());
    }

    public void UpdateMap() {
        spikesPos.Clear();
        for (var i = 0; i < sizeX; ++i) {
            for (var j = 0; j < sizeY; ++j) {
                if (movingSpikes.HasTile(new Vector3Int(i, j, 0) + zero)) {
                    spikesPos.Add(new Vector3Int(i, j, 0) + zero);
                }
            }
        }
    }

    public IEnumerator MoveCoroutine() {
        while (true) {
            yield return new WaitForSeconds(0.4f);

            foreach (var pos in spikesPos) {
                movingSpikes.SetTile(pos, t0);
            }

            yield return new WaitForSeconds(0.8f);

            foreach (var pos in spikesPos) {
                movingSpikes.SetTile(pos, t1);
            }

            collider2d.enabled = true;


            yield return new WaitForSeconds(0.4f);

            collider2d.enabled = false;

            foreach (var pos in spikesPos) {
                movingSpikes.SetTile(pos, t0);
            }

            yield return new WaitForSeconds(0.4f);

            foreach (var pos in spikesPos) {
                movingSpikes.SetTile(pos, null);
            }
        }
    }
}