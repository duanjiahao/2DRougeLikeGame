using UnityEngine;

public class Hero : Character {

    public string littleMapImage = "Hero_littleMap";

    public Hero(Transform container) {
        currentPosition = DungeonManager.Singleton.CurrentDungeon.StartPoint;
        prefabPath = "Hero";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        currentDirction = CharacterDirection.DOWN;
        IsHero = true;
        CurLife = MaxLife = 100;
        Atk = 10;
    }

    public void SetHeroData(Transform container) {
        currentPosition = DungeonManager.Singleton.CurrentDungeon.StartPoint;
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        currentDirction = CharacterDirection.DOWN;
    }

    public override void Death() {
        // GameOver
        GameOverWindow window = WindowManager.Singleton.ShowWindow<GameOverWindow>(UIWindowType.GAME_OVER_WINDOW);
        window.SetContent();
        CharacterManager.Singleton.RemoveCharacter(this);
    }
}
