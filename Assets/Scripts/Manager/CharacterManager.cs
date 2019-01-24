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
    }

    public void GenerateHero() {
        Hero = new Hero(Container);
        Characters.Add(Hero);
    }

    public void Update() {
        CheckIfReachDeKuChi();
    }

    /// <summary>
    /// 检查是否到达出口
    /// </summary>
    private void CheckIfReachDeKuChi() {
        if (Hero.currentPosition.Equals(DungeonManager.Singleton.CurrentDungeon.EndPoint)) {
            startGame.RestartAll();
        }
    }
}
