using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectInfoManager : MonoBehaviour
{
    public static ObjectInfoManager Instance;
    private GameObject infoPanel;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI descText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetupPanel(GameObject panel, TextMeshProUGUI title, TextMeshProUGUI desc)
    {
        infoPanel = panel;
        titleText = title;
        descText = desc;
        infoPanel.SetActive(false);
    }

    public void ShowInfo(string title, string desc)
    {
        if (infoPanel == null) return;
        titleText.text = title;
        descText.text = desc;
        infoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        if (infoPanel == null) return;
        infoPanel.SetActive(false);
    }

    public void ShowRodInfo(DraggableRod rod)
    {
        if (rod == null) { HideInfo(); return; }

        string name = rod.myChargeType == "Glass" ? "Cam Çubuk" : "Plastik Çubuk";
        string charge, color;

        if (rod.currentState == DraggableRod.RodState.Suspended)
        {
            if (GameManager.Instance != null && GuideManager.Instance != null)
            {
                int s = GameManager.Instance.currentStep;
                if (s == 1 || s == 2)
                    GuideManager.Instance.CompleteTask(s, 3);
            }
        }

        if (rod.isCharged)
        {
            if (rod.myChargeType == "Glass")
            {
                charge = "Pozitif (+) Yüklü";
                color = "<color=#FF8C00>";
            }
            else
            {
                charge = "Negatif (-) Yüklü";
                color = "<color=#4488FF>";
            }
        }
        else
        {
            charge = "Yüksüz";
            color = "<color=#AAAAAA>";
        }
        string state = rod.currentState == DraggableRod.RodState.Suspended
            ? "Standa Asılı"
            : "Masa Üstünde";
        ShowInfo(name, color + charge + "</color>\n" + state);
    }

    public void ShowClothInfo(DraggableCloth cloth)
    {
        bool isSilk = cloth.gameObject.name == "SilkCloth";
        string name = isSilk ? "İpek Kumaş" : "Yünlü Kumaş";
        string desc = cloth.isCharged
            ? (isSilk
                ? "<color=#FF8C00>Elektron verdi → Pozitif yüklendi</color>"
                : "<color=#4488FF>Elektron aldı → Negatif yüklendi</color>")
            : "Henüz yüklenmedi";

        ShowInfo(name, desc);
    }
}