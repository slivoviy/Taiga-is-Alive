using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class LevelManager : MonoBehaviour {
    public static readonly Vector3Int Zero = new Vector3Int(-53, -33, 0);
    public static int timeToNextMutation = 10;

    public RuleTile tile;
    public Tile spikeTile;
    public Tile movingSpikeTile;
    public SpikeMovement movingSpikesUpdater;
    public Tile bridgeTile;
    public Tile grassTile;

    private bool[][] startTiles;
    private Dictionary<Vector3Int, string> floorTiles;

    private string[] types = {"Spike", "Moving Spike", "Bridge", "Not Ground"};

    public Tilemap level;
    public Tilemap spikes;
    public Tilemap movingSpikes;
    public Tilemap bridges;
    public Tilemap grass;

    public List<Vector3Int> safeZone;

    void Start() {
        floorTiles = new Dictionary<Vector3Int, string>();
        startTiles = new bool[level.size.x][];
        for (int i = 0; i < level.size.x; ++i) {
            startTiles[i] = new bool[level.size.y];
            for (int j = 0; j < level.size.y; ++j) {
                if (level.HasTile(new Vector3Int(i, j, 0) + Zero)) {
                    startTiles[i][j] = true;
                    if (!level.HasTile(new Vector3Int(i, j + 1, 0) + Zero)) {
                        floorTiles.Add(new Vector3Int(i, j, 0) + Zero, "Ground");
                    }
                }
                else {
                    startTiles[i][j] = false;
                }
            }
        }

        foreach (var pos in safeZone) {
            floorTiles.Remove(pos);
        }

        StartCoroutine(ChooseTile());
    }

    private IEnumerator ChooseTile() {
        while (true) {
            yield return new WaitForSeconds(timeToNextMutation);

            foreach (var pair in floorTiles) {
                RevertToGround(pair.Key, new Vector3Int(pair.Key.x, pair.Key.y + 1, pair.Key.z), pair.Value);
            }
            movingSpikesUpdater.UpdateMap();
            
            var rand = new Random();
            var dangers = rand.Next(120, 171);
            var nums = Enumerable.Range(0, 431).ToArray().OrderBy(x => rand.Next()).ToArray();
            
            var tilesList = floorTiles.ToList();

            for (int i = 0; i < dangers; ++i) {
                var pair = tilesList[nums[i]];
                string danger = types[rand.Next(0, 4)];
                Vector3Int higherPos = new Vector3Int(pair.Key.x, pair.Key.y + 1, pair.Key.z);

                if (danger != pair.Value) {
                    Mutate(pair.Key, higherPos, danger, pair.Value);
                    floorTiles[pair.Key] = danger;
                }

            }
            
            movingSpikesUpdater.UpdateMap();
            
            if (timeToNextMutation > 5) {
                timeToNextMutation -= 1;
            }
        }
    }

    private void Mutate(Vector3Int pos, Vector3Int higherPos, string danger, String prevDanger) {
        switch (danger) {
            case "Spike":
                if (prevDanger == "Moving Spike")
                    movingSpikes.SetTile(higherPos, null);
                else if (prevDanger == "Bridge" || prevDanger == "Not Ground") {
                    if (prevDanger == "Bridge")
                        bridges.SetTile(pos, null);

                    for (int k = pos.y - Zero.y; k >= 0; --k) {
                        if (startTiles[pos.x - Zero.x][k]) {
                            level.SetTile(new Vector3Int(pos.x - Zero.x, k, 0) + Zero, tile);
                        }
                        else {
                            k = -1;
                        }
                    }
                }
                
                spikes.SetTile(higherPos, spikeTile);
                break;

            case "Moving Spike":
                if (prevDanger == "Spike")
                    spikes.SetTile(higherPos, null);
                else if (prevDanger == "Bridge" || prevDanger == "Not Ground") {
                    if (prevDanger == "Bridge")
                        bridges.SetTile(pos, null);

                    for (int k = pos.y - Zero.y; k >= 0; --k) {
                        if (startTiles[pos.x - Zero.x][k]) {
                            level.SetTile(new Vector3Int(pos.x - Zero.x, k, 0) + Zero, tile);
                        }
                        else {
                            k = -1;
                        }
                    }
                }

                movingSpikes.SetTile(higherPos, movingSpikeTile);
                break;

            case "Bridge":
                if (prevDanger == "Not Ground") {
                    bridges.SetTile(pos, bridgeTile);
                }
                else {
                    if (prevDanger == "Spike") {
                        spikes.SetTile(higherPos, null);
                    }
                    else if (prevDanger == "Moving Spike") {
                        movingSpikes.SetTile(higherPos, null);
                    }

                    if (grass.HasTile(higherPos)) {
                        grass.SetTile(higherPos, null);
                    }
                    
                    level.SetTile(pos, null);
                    bridges.SetTile(pos, bridgeTile);

                    for (int k = pos.y - Zero.y - 1; k >= 0; --k) {
                        if (startTiles[pos.x - Zero.x][k]) {
                            level.SetTile(new Vector3Int(pos.x - Zero.x, k, 0) + Zero, null);
                        }
                        else {
                            k = -1;
                        }
                    }
                }

                break;
            case "Not Ground":
                if (prevDanger == "Bridge") {
                    bridges.SetTile(pos, null);
                }
                else {
                    if (prevDanger == "Spike") {
                        spikes.SetTile(higherPos, null);
                    }
                    else if (prevDanger == "Moving Spike") {
                        movingSpikes.SetTile(higherPos, null);
                    }

                    if (grass.HasTile(higherPos)) {
                        grass.SetTile(higherPos, null);
                    }

                    for (int k = pos.y - Zero.y; k >= 0; --k) {
                        if (startTiles[pos.x - Zero.x][k]) {
                            level.SetTile(new Vector3Int(pos.x - Zero.x, k, 0) + Zero, null);
                        }
                        else {
                            k = -1;
                        }
                    }
                }

                break;
        }
    }

    private void RevertToGround(Vector3Int pos, Vector3Int higherPos, String danger) {
        if (danger == "Spike") {
            spikes.SetTile(higherPos, null);
        }
        else if (danger == "Moving Spike") {
            movingSpikes.SetTile(higherPos, null);
        }
        else if (danger == "Bridge" || danger == "Not Ground") {
            if (danger == "Bridge")
                bridges.SetTile(pos, null);

            if (!grass.HasTile(higherPos)) {
                grass.SetTile(higherPos, grassTile);
            }

            for (int k = pos.y - Zero.y - 1; k >= 0; --k) {
                if (startTiles[pos.x - Zero.x][k]) {
                    level.SetTile(new Vector3Int(pos.x - Zero.x, k, 0) + Zero, tile);
                }
                else {
                    k = -1;
                }
            }
        }

        level.SetTile(pos, tile);
    }
}