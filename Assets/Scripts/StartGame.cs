using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float currentTime = Time.realtimeSinceStartup;

        ResourceManager.Singleton.Init();

        Dungeon dungeon = new Dungeon(20, 5, 20, 0);

        dungeon.GenerateDungeon();
        dungeon.DrawDungeon(transform);

        //记录耗时
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
