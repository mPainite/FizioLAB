using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroSceneBuilder : MonoBehaviour
{
    public Sprite schoolLogo;

    void Awake()
    {
        BuildIntroUI();
    }

    void BuildIntroUI()
    {
        // Canvas oluştur
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.AddComponent<GraphicRaycaster>();

        // Arka plan
        GameObject bg = CreatePanel("Background", canvasObj.transform,
            Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero,
            new Color(0.03f, 0.05f, 0.12f, 1f));

        // Üst mavi çizgi
        GameObject topLine = CreatePanel("TopLine", bg.transform,
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, -6), new Vector2(0, 0),
            new Color(0.2f, 0.6f, 1f, 1f));
        topLine.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 4);

        // Alt mavi çizgi
        GameObject botLine = CreatePanel("BotLine", bg.transform,
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0, 6), new Vector2(0, 0),
            new Color(0.2f, 0.6f, 1f, 1f));
        botLine.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 4);

        // Orta kart
        GameObject card = CreatePanel("Card", bg.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            Vector2.zero, Vector2.zero,
            new Color(0.06f, 0.09f, 0.2f, 1f));
        card.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);

        // Kart sol mavi şerit
        GameObject cardAccent = CreatePanel("CardAccent", card.transform,
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(3, 0), new Vector2(0, 0),
            new Color(0.2f, 0.6f, 1f, 1f));
        cardAccent.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 0);

        // Logo
        if (schoolLogo != null)
        {
            GameObject logoObj = new GameObject("Logo");
            logoObj.transform.SetParent(card.transform, false);
            RectTransform lr = logoObj.AddComponent<RectTransform>();
            lr.anchorMin = new Vector2(0.5f, 0.5f);
            lr.anchorMax = new Vector2(0.5f, 0.5f);
            lr.sizeDelta = new Vector2(120, 120);
            lr.anchoredPosition = new Vector2(0, 220);
            Image logoImg = logoObj.AddComponent<Image>();
            logoImg.sprite = schoolLogo;
            logoImg.preserveAspect = true;
        }

        // Okul adı
        CreateTMP("SchoolName", card.transform,
            "FİZİK LABORATUVARI", 13,
            new Color(0.4f, 0.7f, 1f, 0.8f),
            new Vector2(0, 145), new Vector2(500, 30),
            FontStyles.Bold, TextAlignmentOptions.Center);

        // Ana başlık
        CreateTMP("MainTitle", card.transform,
            "ELEKTROSTATİK\nDENEYLERİ", 38,
            Color.white,
            new Vector2(0, 60), new Vector2(500, 110),
            FontStyles.Bold, TextAlignmentOptions.Center);

        // Alt başlık
        CreateTMP("SubTitle", card.transform,
            "Zıt Yüklerin Etkileşimi", 18,
            new Color(0.4f, 0.75f, 1f),
            new Vector2(0, -10), new Vector2(500, 35),
            FontStyles.Normal, TextAlignmentOptions.Center);

        // Ayırıcı çizgi
        CreatePanel("Divider", card.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, -55), Vector2.zero,
            new Color(0.2f, 0.6f, 1f, 0.4f))
            .GetComponent<RectTransform>().sizeDelta = new Vector2(400, 2);

        // Açıklama
        CreateTMP("Description", card.transform,
            "Bu deneyde cam ve plastik çubukları\nfarklı kumaşlarla sürtüp elektriksel\nyüklenmeyi ve etkileşimi gözlemleyeceksiniz.",
            14, new Color(0.65f, 0.75f, 0.9f),
            new Vector2(0, -130), new Vector2(480, 90),
            FontStyles.Normal, TextAlignmentOptions.Center);

        // Deneye Başla butonu
        GameObject btnObj = CreatePanel("StartButton", card.transform,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0, -240), Vector2.zero,
            new Color(0.15f, 0.45f, 0.9f, 1f));
        btnObj.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 60);

        Button startBtn = btnObj.AddComponent<Button>();
        ColorBlock cb = startBtn.colors;
        cb.highlightedColor = new Color(0.2f, 0.55f, 1f);
        cb.pressedColor = new Color(0.1f, 0.35f, 0.8f);
        startBtn.colors = cb;
        startBtn.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        CreateTMP("StartText", btnObj.transform,
            "▶   Deneye Başla", 18,
            Color.white, Vector2.zero, new Vector2(300, 60),
            FontStyles.Bold, TextAlignmentOptions.Center);

        // Sürüm
        CreateTMP("Version", bg.transform,
            "v1.0  |  Fizik Labı Simülasyonu", 11,
            new Color(0.3f, 0.4f, 0.6f),
            new Vector2(0, 15), new Vector2(400, 25),
            FontStyles.Normal, TextAlignmentOptions.Center);
        RectTransform vr = GameObject.Find("Version").GetComponent<RectTransform>();
        vr.anchorMin = new Vector2(0.5f, 0);
        vr.anchorMax = new Vector2(0.5f, 0);
    }

    GameObject CreatePanel(string name, Transform parent,
        Vector2 anchorMin, Vector2 anchorMax,
        Vector2 anchoredPos, Vector2 offsetAdjust, Color color)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform r = obj.AddComponent<RectTransform>();
        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
        r.anchoredPosition = anchoredPos;
        r.offsetMin += offsetAdjust;
        r.offsetMax -= offsetAdjust;
        Image img = obj.AddComponent<Image>();
        img.color = color;
        return obj;
    }

    void CreateTMP(string name, Transform parent, string text, int size,
        Color color, Vector2 pos, Vector2 sizeDelta,
        FontStyles style, TextAlignmentOptions align)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform r = obj.AddComponent<RectTransform>();
        r.anchorMin = new Vector2(0.5f, 0.5f);
        r.anchorMax = new Vector2(0.5f, 0.5f);
        r.anchoredPosition = pos;
        r.sizeDelta = sizeDelta;
        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.fontStyle = style;
        tmp.alignment = align;
    }
}