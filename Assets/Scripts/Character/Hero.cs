using UnityEngine;

public class Hero : Character {

    public string littleMapImage = "Hero_littleMap";

    public int LimitRange {
        get {
            return 5;
        }
    }

    private Transform limitSkill;

    public Hero(Transform container) {
        currentPosition = DungeonManager.Singleton.CurrentDungeon.StartPoint;
        prefabPath = "Hero";
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        limitSkill = go.transform.Find("Effect/limitSkill");
        currentDirction = CharacterDirection.DOWN;
        IsHero = true;
        CurLife = MaxLife = 100;
        Atk = 10;
    }

    public override void Init() {
        base.Init();

        onceActionMap.Add(ActionType.LimitSkill, LimitSkillInit);
        updateActionMap.Add(ActionType.LimitSkill, LimitSkillUpdate);
    }

    public void SetHeroData(Transform container) {
        currentPosition = DungeonManager.Singleton.CurrentDungeon.StartPoint;
        go = Utils.DrawCharacter(this, container);
        animator = go.GetComponentInChildren<Animator>();
        limitSkill = go.transform.Find("Effect/limitSkill");
        currentDirction = CharacterDirection.DOWN;
    }

    public override void Death() {
        // GameOver
        GameOverWindow window = WindowManager.Singleton.ShowWindow<GameOverWindow>(UIWindowType.GAME_OVER_WINDOW);
        window.SetContent();
        CharacterManager.Singleton.RemoveCharacter(this);
    }

    private Vector3 endPos;
    private float time;
    private bool hasSet;
    protected virtual void LimitSkillInit() {
        animator.SetTrigger("Attack");
        animator.speed = 0f;
        time = 0f;
        hasSet = false;
        character = null;
        Position endPosition = DungeonManager.Singleton.CurrentDungeon.GetMaxReachPositonAndCharacter(currentPosition, currentDirction, LimitRange, ref character);
        endPos = Utils.GetPosByPositon(go.transform.localPosition, endPosition - currentPosition);
        currentPosition = endPosition;
    }

    protected virtual bool LimitSkillUpdate() {
        if (Utils.IsReachEndPosByDirection(go.transform.localPosition, endPos, currentDirction)) {
            if (!hasSet) {
                go.transform.localPosition = endPos;
                animator.speed = 1f;
                hasSet = true;

            }

            time += Time.smoothDeltaTime;
            if (time > 1f) {
                ShowLimitSkill();
                SceneManager.Singleton.DelayCall(0.5f, () => {
                    limitSkill.gameObject.SetActive(false);
                });
                animator.SetFloat("AttackTime", 2f);
                if (character != null) {
                    PerformAttack();
                    if (character != null) {
                        if (character.IsHero) {
                            SceneManager.Singleton.MainCameraShake(0f, 0.5f);
                        } else {
                            OtherActionQueue.Enqueue(character.BeAttack);
                            if (DungeonManager.Singleton.CurrentDungeon.CanMove(currentDirction, character.currentPosition)) {
                                Position position = Utils.GetPositonByDirction(character.currentPosition, currentDirction);
                                character.go.transform.localPosition = Utils.GetPosByPositon(character.go.transform.localPosition, position - character.currentPosition);
                                character.currentPosition = position;
                            }
                        }
                    }
                }
                return true;
            }
        } else {
            go.transform.localPosition += Utils.GetDirVectorByCharacterDirction(currentDirction) * Time.smoothDeltaTime * 8f * Utils.TILE_SIZE;
        }

        return false;
    }

    private void ShowLimitSkill() {
        limitSkill.gameObject.SetActive(true);
        if (currentDirction == CharacterDirection.LEFT) {
            limitSkill.localRotation = Quaternion.Euler(Vector3.zero);
        } else if (currentDirction == CharacterDirection.DOWN) {
            limitSkill.localRotation = Quaternion.Euler(new Vector3(0, 0, 20f));
        } else if (currentDirction == CharacterDirection.RIGHT) {
            limitSkill.localRotation = Quaternion.Euler(new Vector3(0, -180f, 0));
        } else if (currentDirction == CharacterDirection.UP) {
            limitSkill.localRotation = Quaternion.Euler(new Vector3(180f, 0, 20f));
        }
    }
}
