using System.Collections.Generic;
using System.Collections;
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

    public void RemoveCharacter(BaseCharacter baseCharacter) {
        startGame.StartCoroutine("StartRemoveCharacter", baseCharacter);
    }

    public void RemoveAllCharacter() {
        Characters.Clear();
    }

    public void GenerateHero() {
        if (Hero == null) {
            Hero = new Hero(Container);
            Hero.Init();
        } else {
            Hero.SetHeroData(Container);
        }
        Characters.Add(Hero);
        lastHeroPosition = Hero.currentPosition;
        CheckIfInPath();
    }

    public void GenerateMonsters() {
        List<Position> positions = DungeonManager.Singleton.CurrentDungeon.GenerateMonsterPositons();
        foreach (Position positon in positions) {
            Monster monster = new Monster(Container, positon);
            monster.Init();
            Characters.Add(monster);
        }
    }

    private Position lastHeroPosition;
    public void Update() {
        if (Hero != null && !Hero.isActing && Hero.currentPosition != lastHeroPosition) {
            CheckIfReachDeKuChi();
            CheckIfInPath();
            lastHeroPosition = Hero.currentPosition;
        }
    }

    public BaseCharacter GetCharacterByPositon(Position position) {
        foreach (BaseCharacter character in Characters) {
            if (character.currentPosition == position) {
                return character;
            }
        }
        return null;
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
        if (DungeonManager.Singleton.CurrentDungeon.IsInPath(Hero.currentPosition)) {
            startGame.mask.SetActive(true);
        } else {
            startGame.mask.SetActive(false);
        }
    }

    public bool IsCharacterInPositon(Position position) {
        foreach (BaseCharacter character in Characters) {
            if (position == character.currentPosition) {
                return true;
            }
        }
        return false;
    }
}
