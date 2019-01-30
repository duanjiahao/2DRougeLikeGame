using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour {

    [SerializeField]
    private Text lvl;
    [SerializeField]
    private Slider exp;
    [SerializeField]
    private Slider life;

    public void UpdateStatusInfo() {

        if (CharacterManager.Singleton.Hero == null) {
            return;
        }

        lvl.text = string.Format("Lv {0}", CharacterManager.Singleton.Hero.Lvl);
        exp.value = (float)CharacterManager.Singleton.Hero.CurExp / CharacterManager.Singleton.Hero.NeedExp;
        exp.fillRect.gameObject.SetActive(exp.value > 0f);
        life.value = (float)CharacterManager.Singleton.Hero.CurLife / CharacterManager.Singleton.Hero.MaxLife;
        life.fillRect.gameObject.SetActive(life.value > 0f);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
