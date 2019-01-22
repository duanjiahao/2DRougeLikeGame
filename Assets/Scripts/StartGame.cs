using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ResourceManager.Singleton.Init();

        Dungeon dungeon = new Dungeon(20, 5, 20, 0);

        float currentTime = Time.realtimeSinceStartup;
        dungeon.GenerateDungeon();
        Debug.LogWarning(Time.realtimeSinceStartup - currentTime);
        dungeon.DrawDungeon(transform);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
