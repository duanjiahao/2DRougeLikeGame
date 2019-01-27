using UnityEngine;

public class Hero : Character {

    public string littleMapImage = "Hero_littleMap";

    public Hero(Transform container) {
        currentPosition = DungeonManager.Singleton.CurrentDungeon.StartPoint;
        prefabPath = "Hero";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        currentDirction = CharacterDirction.DOWN;
    }
}
