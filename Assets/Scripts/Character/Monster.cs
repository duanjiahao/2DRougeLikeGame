using UnityEngine;

public class Monster : Character {

    private Position sightArea = new Position(4, 4);

    private SpriteRenderer life;

    public Monster(Transform container, Position position) {
        currentPosition = position;
        prefabPath = "Monster";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        life = go.transform.Find("life").GetComponent<SpriteRenderer>();
        currentDirction = CharacterDirection.DOWN;
        IsHero = false;
        CurLife = MaxLife = 50;
        Atk = 5;
        Exp = 50;
    }

    public void WhatToDo(Position heroPositon) {

        ClearQueue();

        if (!(heroPositon - currentPosition < sightArea)) {
            SetQueueByActionType((ActionType)UnityEngine.Random.Range(1, 5));
            return;
        }

        if (Utils.IsInAttackRange(currentPosition, heroPositon)) {
            OtherActionQueue.Enqueue(Attack);
            return;
        }

        ActionType type = (ActionType)DungeonManager.Singleton.CurrentDungeon.GetNextDirection(currentPosition);
        SetQueueByActionType(type);
    }

    public override void Death() {
        ResourceManager.Singleton.Unload(go);
        CharacterManager.Singleton.RemoveCharacter(this);
    }

    public void UpdateLife() {
        if (CurLife == MaxLife) {
            life.gameObject.SetActive(false);
            return;
        }
        life.gameObject.SetActive(true);
        Vector3 screenPos = SceneManager.MainCamera.WorldToScreenPoint(life.transform.position);

        life.material.SetFloat("_Progress", (float)CurLife / MaxLife);
    }

    protected override void AttackInit() {
        if (Utils.IsInAttackRange(currentPosition, CharacterManager.Singleton.Hero.currentPosition)) {
            ChangeDirction(Utils.GetDirction(currentPosition, CharacterManager.Singleton.Hero.currentPosition));
        }
        base.AttackInit();
    }
}
