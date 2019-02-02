using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseWindow : BaseWindow {

    private Button mask;
    private Button saveBtn;
    private Button returnBtn;

    public override void InitUI() {
        base.InitUI();

        mask = Root.Find("mask").GetComponent<Button>();
        saveBtn = Root.Find("saveBtn").GetComponent<Button>();
        returnBtn = Root.Find("returnBtn").GetComponent<Button>();

        mask.onClick.AddListener(OnMaskBtnClick);
        saveBtn.onClick.AddListener(OnSaveBtnClick);
        returnBtn.onClick.AddListener(OnReturnBtnClick);
    }

    public void SetContent() {
        saveBtn.interactable = true;
    }

    private void OnReturnBtnClick() {
        Hide();
        StartGameWindow window = WindowManager.Singleton.ShowWindow<StartGameWindow>(UIWindowType.START_GAME_WINDOW);
        window.SetContent();
    }

    private void OnSaveBtnClick() {
        GameData.Save();
        saveBtn.interactable = false;
    }

    private void OnMaskBtnClick() {
        Hide();
    }
}
