using System;
using UnityEngine;

public class DungeonManager {

    private static DungeonManager dungeonManager;
    public static DungeonManager Singleton {
        get {
            if (dungeonManager == null) {
                dungeonManager = new DungeonManager();
            }
            return dungeonManager;
        }
    }

    public Dungeon CurrentDungeon {
        get;
        private set;
    }

    public Transform Container {
        get;
        private set;
    }

    public int Layer {
        get;
        private set;
    }

    public int Seed {
        get;
        private set;
    }

    public void Init(Transform dungeonContainer) {
        this.Container = dungeonContainer;
        this.Layer = 1;
    }

    public void RecountLayer() {
        this.Layer = 1;
    }

    public void StartNewDungeon(SaveData saveData) {
        if (saveData != null) {
            Seed = saveData.seed;
            Layer = saveData.layer;
            ApplySeed();
        } else {
            GenerateSeed();
        }

        CurrentDungeon = new Dungeon(20, 5, 20, 3);
        CurrentDungeon.GenerateDungeon();
        CurrentDungeon.DrawDungeon(Container);
        Layer++;
    }

    private void GenerateSeed() {
        TimeSpan timeSpan = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long seconds =  (long)timeSpan.TotalSeconds;
        Seed = (int)(seconds * "Tsukki".GetHashCode());
        ApplySeed();
    }

    private void ApplySeed() {
        UnityEngine.Random.InitState(Seed);
    }
}
