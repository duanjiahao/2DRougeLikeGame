using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public Camera UICamera;

    public Transform heroContainer;

    public void RestartAll() {
        StartCoroutine("Restart");
    }

    IEnumerator Restart() {
        yield return null;

        ResourceManager.Singleton.UnAllResource();
        DungeonManager.Singleton.StartNewDungeon();
        CharacterManager.Singleton.RemoveAllCharacter();
        CharacterManager.Singleton.GenerateHero();
    }

	// Use this for initialization
	void Start () {
        float currentTime = Time.realtimeSinceStartup;

        ResourceManager.Singleton.Init();
        InputController.Singleton.Init();
        DungeonManager.Singleton.Init(transform);
        CharacterManager.Singleton.Init(heroContainer);

        DungeonManager.Singleton.StartNewDungeon();
        CharacterManager.Singleton.GenerateHero();

        UICamera.transform.localPosition = new Vector3(DungeonManager.Singleton.CurrentDungeon.StartPoint.col * Utils.TILE_SIZE, DungeonManager.Singleton.CurrentDungeon.StartPoint.row * Utils.TILE_SIZE);

        //记录耗时
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
    }

    // Update is called once per frame
    void Update() {
        InputController.Singleton.Update();

        if (CharacterManager.Singleton.Hero.go != null) {
            UICamera.transform.localPosition = CharacterManager.Singleton.Hero.go.transform.localPosition;
        }
	}
}
