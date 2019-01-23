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

    public void Init(Transform container) {
        this.Container = container;
        Characters = new List<BaseCharacter>();
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
}
