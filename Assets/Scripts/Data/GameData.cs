using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData {
    public int level;
    public int exp;
    public int life;
    public int layer;
    public int seed;
}

public class GameData {

    public const string SAVE_DATA = "SAVEDATA.bytes";

    public static void Save() {
        SaveData saveData = new SaveData();
        saveData.level = CharacterManager.Singleton.Hero.Lvl;
        saveData.life = CharacterManager.Singleton.Hero.CurLife;
        saveData.exp = CharacterManager.Singleton.Hero.CurExp;
        saveData.seed = DungeonManager.Singleton.Seed;
        saveData.layer = DungeonManager.Singleton.Layer - 1;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        binaryFormatter.Serialize(memoryStream, saveData);
        FileUtils.WriteAllBytes(memoryStream, FileUtils.CombinePath(Application.streamingAssetsPath, SAVE_DATA));
    }

    public static SaveData Load() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = FileUtils.ReadAllBytes(FileUtils.CombinePath(Application.streamingAssetsPath, SAVE_DATA));
        SaveData saveData = binaryFormatter.Deserialize(memoryStream) as SaveData;
        return saveData;
    }
}
