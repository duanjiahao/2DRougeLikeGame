using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    public Camera UICamera;

    public Transform heroContainer;

    private Hero hero;

	// Use this for initialization
	void Start () {
        float currentTime = Time.realtimeSinceStartup;

        ResourceManager.Singleton.Init();
        InputController.Singleton.Init();

        Dungeon dungeon = new Dungeon(20, 5, 20, 0);
        dungeon.GenerateDungeon();
        dungeon.DrawDungeon(transform);

        hero = new Hero(dungeon.StartPoint, heroContainer, dungeon);
        InputController.Singleton.AddCharacter(hero);

        UICamera.transform.localPosition = new Vector3(dungeon.StartPoint.col * 50f, dungeon.StartPoint.row * 50f, -100f);

        //记录耗时
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
    }

    // Update is called once per frame
	void Update () {
        InputController.Singleton.Update();

        UICamera.transform.localPosition = hero.go.transform.localPosition;
	}
}
