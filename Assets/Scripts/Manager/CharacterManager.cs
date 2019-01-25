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
        check = true;
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
        check = false;
    }

    private bool check = false;
    public void Update() {
        CheckIfReachDeKuChi();
    }

    /// <summary>
    /// 检查是否到达出口
    /// </summary>
    private void CheckIfReachDeKuChi() {
        if (!check && Hero != null && Hero.currentPosition == DungeonManager.Singleton.CurrentDungeon.EndPoint) {
            startGame.RestartAll();
            check = true;
        }
    }
}
