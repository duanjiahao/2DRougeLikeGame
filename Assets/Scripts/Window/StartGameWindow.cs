using UnityEngine;
using UnityEngine.UI;

public class StartGameWindow : BaseWindow, IUpdateWindow {
    private Button nextBtn;
    private Button loadBtn;
    private Slider slider;

    public override void InitUI() {
        base.InitUI();

        //initUI
        nextBtn = Root.Find("nextBtn").GetComponent<Button>();
        loadBtn = Root.Find("loadBtn").GetComponent<Button>();
        slider = Root.Find("slider").GetComponent<Slider>();

        nextBtn.onClick.AddListener(OnNextBtnClick);
        loadBtn.onClick.AddListener(OnLoadBtnClick);
    }

    private void OnLoadBtnClick() {
        loadBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        slider.gameObject.SetActive(true);
        startSlider = true;
        SceneManager.StartGame.RestartAll(true, GameData.Load());
    }

    bool startSlider;
    private void OnNextBtnClick() {
        loadBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        slider.gameObject.SetActive(true);
        startSlider = true;

        SceneManager.StartGame.RestartAll(true, null);
    }

    public void SetContent() {
        loadBtn.gameObject.SetActive(true);
        loadBtn.interactable = FileUtils.ContainsDataFile();
        nextBtn.gameObject.SetActive(true);
        slider.gameObject.SetActive(false);
        slider.value = 0f;
        progress = 0f;
        startSlider = false;
        DungeonManager.Singleton.RecountLayer();
    }

    private float progress;
    public void Update() {
        if (!startSlider) {
            return;
        }

        progress += Time.smoothDeltaTime;

        float limit = (float)SceneManager.StartGame.currentStep / (int)StartGame.RestartStep.AllComplete;
        if (progress > limit) {
            progress = limit;
        }

        slider.value = progress;
        if (progress >= .99f) {
            Hide();
        }
    }
}
