using System.Collections.Generic;
using UnityEngine;

public class CharacterManager {

    private static CharacterManager characterManager;
    public static CharacterManager Singleton {
        get {
            if (characterManager == null) {
                characterManager = new CharacterManager();
            }
            return characterManager;
        }
    }

    public Transform Container {
        get;
        private set;
    }

    public Hero Hero {
        get;
        private set;
    }

    public List<BaseCharacter> Characters {
        get;
        private set;
    }

    private StartGame startGame;

    public void Init(Transform container, StartGame startGame) {
        this.Container = container;
        Characters = new List<BaseCharacter>();
        this.startGame = startGame;
    }

    public void AddCharacter(BaseCharacter baseCharacter) {
        Characters.Add(baseCharacter);
    }

    public void RemoveAllCharacter() {
        Characters.Clear();
        Hero = null;
    }

    public void GenerateHero() {
        Hero = new Hero(Container);
        Characters.Add(Hero);
        lastHeroPosition = Hero.currentPosition;
    }

    private Position lastHeroPosition;
    public void Update() {
        if (Hero != null && Hero.currentPosition != lastHeroPosition) {
            CheckIfReachDeKuChi();
            CheckIfInPath();
            lastHeroPosition = Hero.currentPosition;
        }
    }

    /// <summary>
    /// 检查是否到达出口
    /// </summary>
    private void CheckIfReachDeKuChi() {
        if (Hero != null && Hero.currentPosition == DungeonManager.Singleton.CurrentDungeon.EndPoint) {
            startGame.RestartAll();
        }
    }

    /// <summary>
    /// 检查是否在通路中
    /// </summary>
    /// <value>The check if in path.</value>
    private void CheckIfInPath() {
        //if (DungeonManager.Singleton.CurrentDungeon.IsInPath(Hero.currentPosition)) {
        //    startGame.mask.enabled = true;
        //} else {
        //    startGame.mask.enabled = false;
        //}
    }
}
