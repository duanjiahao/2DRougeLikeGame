﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class SceneManager {
    private static SceneManager sceneManager;
    public static SceneManager Singleton {
        get {
            if (sceneManager == null) {
                sceneManager = new SceneManager();
            }
            return sceneManager;
        }
    }

    private StartGame startGame;

    public void Init(StartGame startGame) {
        this.startGame = startGame;
    }

    public static StartGame StartGame {
        get {
            return Singleton.startGame;
        }
    }

    public static Camera UICamera {
        get {
            return Singleton.startGame.uiCamera;
        }
    }

    public static Camera MainCamera {
        get {
            return Singleton.startGame.mainCamera;
        }
    }

    /// <summary>
    /// 镜头震动
    /// </summary>
    private bool crash = false;
    public void MainCameraShake(float delay, float duration) {
        this.delay = delay;
        this.duration = duration;
        follow = false;
        crash = true;
        time = 0f;
        origin = startGame.mainCamera.transform.localPosition;
    }

    private float time;
    private Vector2 origin;
    private float delay;
    private float duration;
    private void DoMainCameraShake() {
        if (crash) {
            time += Time.deltaTime;
            if (time < delay) return;

            if (time > delay + duration) {
                crash = false;
                follow = true;
            }
            Vector2 dir = new Vector2(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
            startGame.mainCamera.transform.localPosition = origin + dir;

        }
    }

    private SortedList<float, Action> scheduleList = new SortedList<float, Action>();
    public void DelayCall(float delay, Action action) {
        float myTime = Time.time + delay;
        scheduleList.Add(myTime, action);
    }

    public void RunSchedule() {
        if (scheduleList.Count == 0) {
            return;
        }

        if (Time.time >= scheduleList.Keys[0]) {
            scheduleList.Values[0].Invoke();
            scheduleList.RemoveAt(0);
        }
    }

    public void Update() {
        RunSchedule();
    }

    public void LateUpdate() {
        MainCameraFollowHero();
        DoMainCameraShake();
    }

    private bool follow = true;
    private void MainCameraFollowHero() {
        if (follow && CharacterManager.Singleton.Hero != null && CharacterManager.Singleton.Hero.go != null) {
            startGame.mainCamera.transform.localPosition = new Vector2(CharacterManager.Singleton.Hero.go.transform.localPosition.x, CharacterManager.Singleton.Hero.go.transform.localPosition.y);
        }
    }
}
