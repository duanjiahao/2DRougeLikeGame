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

    public List<Monster> Monsters {
        get;
        private set;
    }

    public void Init(Transform container) {
        this.Container = container;
        Characters = new List<BaseCharacter>();
        Monsters = new List<Monster>();
    }

    public void SetSaveData(SaveData saveData) {
        Hero.Lvl = saveData.level;
        Hero.CurLife = saveData.life;
        Hero.CurExp = saveData.exp;
    }

    public void AddCharacter(BaseCharacter baseCharacter) {
        Characters.Add(baseCharacter);
        if (!baseCharacter.IsHero) {
            Monsters.Add(baseCharacter as Monster);
        }
    }

    public void RemoveCharacter(BaseCharacter baseCharacter) {
        Characters.Remove(baseCharacter);

        if (!baseCharacter.IsHero) {
            Monsters.Remove(baseCharacter as Monster);
        } else {
            Hero = null;
        }
    }

    public void RemoveAllCharacter() {
        Characters.Clear();
        Monsters.Clear();
    }

    public void GenerateHero(SaveData saveData) {
        if (Hero == null) {
            Hero = new Hero(Container);
            Hero.Init();
        } else {
            Hero.SetHeroData(Container);
        }

        if (saveData != null) {
            Hero.Lvl = saveData.level;
            Hero.CurExp = saveData.exp;
            Hero.CurLife = saveData.life;
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
            Monsters.Add(monster);
        }
    }

    private Position lastHeroPosition;
    public void Update() {
        if (Hero != null && !Hero.IsActing && Hero.currentPosition != lastHeroPosition) {
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
            NextLayerWindow window = WindowManager.Singleton.ShowWindow<NextLayerWindow>(UIWindowType.NEXT_LAYER_WINDOW);
            window.SetContent();
        }
    }

    /// <summary>
    /// 检查是否在通路中
    /// </summary>
    /// <value>The check if in path.</value>
    private void CheckIfInPath() {
        if (DungeonManager.Singleton.CurrentDungeon.IsInPath(Hero.currentPosition)) {
            SceneManager.StartGame.mask.SetActive(true);
        } else {
            SceneManager.StartGame.mask.SetActive(false);
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
