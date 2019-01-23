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

    public void Init(Transform dungeonContainer) {
        this.Container = dungeonContainer;
    }

    public void StartNewDungeon() {
        CurrentDungeon = new Dungeon(20, 5, 20, 0);
        CurrentDungeon.GenerateDungeon();
        CurrentDungeon.DrawDungeon(Container);
    }
}
