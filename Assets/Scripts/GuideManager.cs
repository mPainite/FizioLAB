using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuideManager : MonoBehaviour
{
    public static GuideManager Instance;
    private GameObject guidePanel;
    private bool isPanelOpen = false;

    private string[][] stepTitles = new string[][]
    {
        new string[] { "Adım 1 — Cam Çubuğu Yükleme", "Önünüzde cam çubuk, ipek kumaş\nve sarkaç bulunuyor." },
        new string[] { "Adım 2 — Plastik Çubuğu Yükleme", "Önünüzde plastik çubuk, yünlü\nkumaş ve sarkaç bulunuyor." },
        new string[] { "Adım 3 — Zıt Yüklerin Etkileşimi", "İki farklı çubuk ve iki farklı\nkumaş bulunuyor." },
        new string[] { "Adım 4 — Aynı Yüklerin Etkileşimi", "İki cam çubuk ve ipek\nkumaş bulunuyor." }
    };

    private string[][] stepTasks = new string[][]
{
    new string[] { "İpek kumaşı cam çubuğa sürt", "Çubuk yeşile döndü → Yüklendi", "Yüklü çubuğu standa as", "Yükü incelemek için çubuğa tıkla", "Raporu doldur ve Adım 2'ye geç" },
    new string[] { "Yünlü kumaşı plastik çubuğa sürt", "Çubuk yeşile döndü → Yüklendi", "Yüklü çubuğu standa as", "Yükü incelemek için çubuğa tıkla", "Raporu doldur ve Adım 3'e geç" },
    new string[] { "Cam çubuğu ipek kumaşla yükle ve sol standa as", "Plastik çubuğu yünlü kumaşla yükle ve sağ standa as", "İki çubuğun etkileşimini gözlemle", "Raporu doldur ve Adım 4'e geç" },
    new string[] { "Birinci cam çubuğu ipek kumaşla yükle ve sol standa as", "İkinci cam çubuğu ipek kumaşla yükle ve sağ standa as", "İki çubuğun etkileşimini gözlemle", "Raporu doldur ve deneyi tamamla" }
};

    private bool[][] taskCompleted;
    private int currentStep = 0;
    private List<TextMeshProUGUI> taskTexts = new List<TextMeshProUGUI>();
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI sceneDescText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        taskCompleted = new bool[][]
 {
    new bool[] { false, false, false, false, false },
    new bool[] { false, false, false, false, false },
    new bool[] { false, false, false, false },
    new bool[] { false, false, false, false }
 };
    }

    public void SetupPanel(GameObject panel)
    {
        guidePanel = panel;
        guidePanel.SetActive(false);
    }

    public void SetTitleText(TextMeshProUGUI title, TextMeshProUGUI desc)
    {
        titleText = title;
        sceneDescText = desc;
    }

    public void AddTaskText(TextMeshProUGUI text)
    {
        taskTexts.Add(text);
    }

    public void TogglePanel()
    {
        isPanelOpen = !isPanelOpen;
        guidePanel.SetActive(isPanelOpen);
    }

    public void UpdateStep(int step)
    {
        currentStep = step - 1;
        if (currentStep < 0) currentStep = 0;
        if (currentStep >= stepTitles.Length) currentStep = stepTitles.Length - 1;

        if (titleText != null)
            titleText.text = stepTitles[currentStep][0];
        if (sceneDescText != null)
            sceneDescText.text = stepTitles[currentStep][1];

        RefreshTasks();
    }

    private void RefreshTasks()
    {
        string[] tasks = stepTasks[currentStep];
        for (int i = 0; i < taskTexts.Count; i++)
        {
            if (taskTexts[i] == null) continue;
            if (i < tasks.Length)
            {
                taskTexts[i].gameObject.SetActive(true);
                bool done = taskCompleted[currentStep][i];
                taskTexts[i].text = (done ? "<color=#00FF88>✓  " : "<color=#CCCCCC>☐  ") + tasks[i] + "</color>";
            }
            else
            {
                taskTexts[i].gameObject.SetActive(false);
            }
        }
    }

    public void CompleteTask(int step, int taskIndex)
    {
        int s = step - 1;
        if (s < 0 || s >= taskCompleted.Length) return;
        if (taskIndex < 0 || taskIndex >= taskCompleted[s].Length) return;
        taskCompleted[s][taskIndex] = true;
        if (currentStep == s) RefreshTasks();
    }
}