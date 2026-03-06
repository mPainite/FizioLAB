using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    [Header("Bağlantılar")]
    public ReportManager reportManager;

    private void Awake()
    {
        BuildUI();
    }

    private void BuildUI()
    {
        Canvas canvas = GetComponent<Canvas>();

        // === TOGGLE BUTTON ===
        GameObject toggleBtn = new GameObject("ToggleButton");
        toggleBtn.transform.SetParent(canvas.transform, false);
        RectTransform toggleRect = toggleBtn.AddComponent<RectTransform>();
        toggleRect.anchorMin = new Vector2(1, 1);
        toggleRect.anchorMax = new Vector2(1, 1);
        toggleRect.pivot = new Vector2(1, 1);
        toggleRect.sizeDelta = new Vector2(180, 48);
        toggleRect.anchoredPosition = new Vector2(-20, -20);
        Image toggleImg = toggleBtn.AddComponent<Image>();
        toggleImg.color = new Color(0.05f, 0.08f, 0.18f, 0.95f);
        Button toggleButton = toggleBtn.AddComponent<Button>();

        GameObject toggleTextObj = new GameObject("Text");
        toggleTextObj.transform.SetParent(toggleBtn.transform, false);
        RectTransform ttr = toggleTextObj.AddComponent<RectTransform>();
        ttr.anchorMin = Vector2.zero; ttr.anchorMax = Vector2.one;
        ttr.offsetMin = Vector2.zero; ttr.offsetMax = Vector2.zero;
        TextMeshProUGUI toggleText = toggleTextObj.AddComponent<TextMeshProUGUI>();
        toggleText.text = "Deney Raporu";
        toggleText.fontSize = 15;
        toggleText.fontStyle = FontStyles.Bold;
        toggleText.color = new Color(0.4f, 0.85f, 1f);
        toggleText.alignment = TextAlignmentOptions.Center;

        // === ANA PANEL ===
        GameObject reportPanel = new GameObject("ReportPanel");
        reportPanel.transform.SetParent(canvas.transform, false);
        RectTransform panelRect = reportPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 1);
        panelRect.sizeDelta = new Vector2(520, 700);
        panelRect.anchoredPosition = new Vector2(-20, -80);
        Image panelImg = reportPanel.AddComponent<Image>();
        panelImg.color = new Color(0.04f, 0.07f, 0.14f, 0.98f);

        // Üst mavi şerit
        GameObject topBar = new GameObject("TopBar");
        topBar.transform.SetParent(reportPanel.transform, false);
        RectTransform tbRect = topBar.AddComponent<RectTransform>();
        tbRect.anchorMin = new Vector2(0, 1);
        tbRect.anchorMax = new Vector2(1, 1);
        tbRect.pivot = new Vector2(0.5f, 1);
        tbRect.sizeDelta = new Vector2(0, 5);
        tbRect.anchoredPosition = Vector2.zero;
        Image tbImg = topBar.AddComponent<Image>();
        tbImg.color = new Color(0.3f, 0.75f, 1f);

        // Başlık
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(reportPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.sizeDelta = new Vector2(-40, 50);
        titleRect.anchoredPosition = new Vector2(0, -20);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "ELEKTROSTATİK DENEY RAPORU";
        titleText.fontSize = 17;
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = new Color(0.4f, 0.85f, 1f);
        titleText.alignment = TextAlignmentOptions.Center;

        // Alt başlık
        GameObject subTitleObj = new GameObject("SubTitle");
        subTitleObj.transform.SetParent(reportPanel.transform, false);
        RectTransform stRect = subTitleObj.AddComponent<RectTransform>();
        stRect.anchorMin = new Vector2(0, 1);
        stRect.anchorMax = new Vector2(1, 1);
        stRect.pivot = new Vector2(0.5f, 1);
        stRect.sizeDelta = new Vector2(-40, 30);
        stRect.anchoredPosition = new Vector2(0, -65);
        TextMeshProUGUI subTitle = subTitleObj.AddComponent<TextMeshProUGUI>();
        subTitle.text = "Zit Yuklerin Etkilesimi - Gozlem Tablosu";
        subTitle.fontSize = 13;
        subTitle.color = new Color(0.5f, 0.65f, 0.8f);
        subTitle.alignment = TextAlignmentOptions.Center;

        // Tablo başlıkları
        string[] headers = { "Malzeme 1", "Malzeme 2", "Sonuc" };
        float[] colX = { -160f, 0f, 160f };
        float[] colW = { 140f, 140f, 140f };

        for (int i = 0; i < headers.Length; i++)
        {
            GameObject header = new GameObject("Header_" + headers[i]);
            header.transform.SetParent(reportPanel.transform, false);
            RectTransform hr = header.AddComponent<RectTransform>();
            hr.anchorMin = new Vector2(0.5f, 1);
            hr.anchorMax = new Vector2(0.5f, 1);
            hr.pivot = new Vector2(0.5f, 1);
            hr.sizeDelta = new Vector2(colW[i], 36);
            hr.anchoredPosition = new Vector2(colX[i], -105);
            Image hImg = header.AddComponent<Image>();
            hImg.color = new Color(0.1f, 0.25f, 0.5f, 1f);
            GameObject hTextObj = new GameObject("Text");
            hTextObj.transform.SetParent(header.transform, false);
            RectTransform htr = hTextObj.AddComponent<RectTransform>();
            htr.anchorMin = Vector2.zero; htr.anchorMax = Vector2.one;
            htr.offsetMin = Vector2.zero; htr.offsetMax = Vector2.zero;
            TextMeshProUGUI hText = hTextObj.AddComponent<TextMeshProUGUI>();
            hText.text = headers[i];
            hText.fontSize = 13;
            hText.fontStyle = FontStyles.Bold;
            hText.color = new Color(0.4f, 0.85f, 1f);
            hText.alignment = TextAlignmentOptions.Center;
        }

        // Tablo satırları - 4 satır
        string[,] rows = {
            { "Cam Cubuk\n(Yuklu)", "Cam Cubuk\n(Yuksuz)", "" },
            { "Plastik Cubuk\n(Yuklu)", "Plastik Cubuk\n(Yuksuz)", "" },
            { "Cam Cubuk\n(Yuklu)", "Plastik Cubuk\n(Yuklu)", "" },
            { "Cam Cubuk\n(Yuklu)", "Cam Cubuk\n(Yuklu)", "" }
        };

        TMP_InputField[] inputFields = new TMP_InputField[4];

        for (int row = 0; row < 4; row++)
        {
            float rowY = -148f - (row * 95f);
            Color rowColor = row % 2 == 0
                ? new Color(0.06f, 0.1f, 0.2f, 1f)
                : new Color(0.05f, 0.08f, 0.16f, 1f);

            for (int col = 0; col < 3; col++)
            {
                GameObject cell = new GameObject($"Cell_{row}_{col}");
                cell.transform.SetParent(reportPanel.transform, false);
                RectTransform cr = cell.AddComponent<RectTransform>();
                cr.anchorMin = new Vector2(0.5f, 1);
                cr.anchorMax = new Vector2(0.5f, 1);
                cr.pivot = new Vector2(0.5f, 1);
                cr.sizeDelta = new Vector2(colW[col] - 2, 88);
                cr.anchoredPosition = new Vector2(colX[col], rowY);
                Image cImg = cell.AddComponent<Image>();
                cImg.color = rowColor;

                if (col < 2)
                {
                    GameObject cTextObj = new GameObject("Text");
                    cTextObj.transform.SetParent(cell.transform, false);
                    RectTransform ctr = cTextObj.AddComponent<RectTransform>();
                    ctr.anchorMin = Vector2.zero; ctr.anchorMax = Vector2.one;
                    ctr.offsetMin = new Vector2(5, 5); ctr.offsetMax = new Vector2(-5, -5);
                    TextMeshProUGUI cText = cTextObj.AddComponent<TextMeshProUGUI>();
                    cText.text = rows[row, col];
                    cText.fontSize = 13;
                    cText.color = Color.white;
                    cText.alignment = TextAlignmentOptions.Center;
                }
                else
                {
                    GameObject inputBg = new GameObject("InputBg");
                    inputBg.transform.SetParent(cell.transform, false);
                    RectTransform ibr = inputBg.AddComponent<RectTransform>();
                    ibr.anchorMin = new Vector2(0.05f, 0.1f);
                    ibr.anchorMax = new Vector2(0.95f, 0.9f);
                    ibr.offsetMin = Vector2.zero;
                    ibr.offsetMax = Vector2.zero;
                    Image ibImg = inputBg.AddComponent<Image>();
                    ibImg.color = new Color(0.08f, 0.13f, 0.25f, 1f);

                    GameObject phObj = new GameObject("Placeholder");
                    phObj.transform.SetParent(inputBg.transform, false);
                    RectTransform phr = phObj.AddComponent<RectTransform>();
                    phr.anchorMin = new Vector2(0.05f, 0.05f);
                    phr.anchorMax = new Vector2(0.95f, 0.95f);
                    phr.offsetMin = Vector2.zero; phr.offsetMax = Vector2.zero;
                    TextMeshProUGUI ph = phObj.AddComponent<TextMeshProUGUI>();
                    ph.text = "Yaz...";
                    ph.fontSize = 12;
                    ph.color = new Color(0.4f, 0.5f, 0.65f);
                    ph.fontStyle = FontStyles.Italic;
                    ph.alignment = TextAlignmentOptions.Center;

                    GameObject itObj = new GameObject("InputText");
                    itObj.transform.SetParent(inputBg.transform, false);
                    RectTransform itr = itObj.AddComponent<RectTransform>();
                    itr.anchorMin = new Vector2(0.05f, 0.05f);
                    itr.anchorMax = new Vector2(0.95f, 0.95f);
                    itr.offsetMin = Vector2.zero; itr.offsetMax = Vector2.zero;
                    TextMeshProUGUI it = itObj.AddComponent<TextMeshProUGUI>();
                    it.fontSize = 12;
                    it.color = Color.white;
                    it.alignment = TextAlignmentOptions.Center;

                    TMP_InputField inputField = inputBg.AddComponent<TMP_InputField>();
                    inputField.textComponent = it;
                    inputField.placeholder = ph;
                    inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
                    inputFields[row] = inputField;
                    inputField.interactable = (row == 0);
                }
            }
        }

        // Adım göstergesi
        GameObject stepObj = new GameObject("StepIndicator");
        stepObj.transform.SetParent(reportPanel.transform, false);
        RectTransform sir = stepObj.AddComponent<RectTransform>();
        sir.anchorMin = new Vector2(0, 0);
        sir.anchorMax = new Vector2(1, 0);
        sir.pivot = new Vector2(0.5f, 0);
        sir.sizeDelta = new Vector2(-40, 30);
        sir.anchoredPosition = new Vector2(0, 110);
        TextMeshProUGUI stepText = stepObj.AddComponent<TextMeshProUGUI>();
        stepText.text = "Deney 1    Deney 2    Deney 3    Deney 4";
        stepText.fontSize = 13;
        stepText.color = new Color(0.4f, 0.85f, 1f, 0.8f);
        stepText.alignment = TextAlignmentOptions.Center;

        // === ADIM 1 BUTONU ===
        GameObject nextBtn = new GameObject("Step1NextButton");
        nextBtn.transform.SetParent(reportPanel.transform, false);
        RectTransform nbr = nextBtn.AddComponent<RectTransform>();
        nbr.anchorMin = new Vector2(0, 0);
        nbr.anchorMax = new Vector2(1, 0);
        nbr.pivot = new Vector2(0.5f, 0);
        nbr.sizeDelta = new Vector2(-40, 50);
        nbr.anchoredPosition = new Vector2(0, 50);
        Image nbImg = nextBtn.AddComponent<Image>();
        nbImg.color = new Color(0.15f, 0.4f, 0.8f, 1f);
        Button nextButton = nextBtn.AddComponent<Button>();
        GameObject nbTextObj = new GameObject("Text");
        nbTextObj.transform.SetParent(nextBtn.transform, false);
        RectTransform nbtr = nbTextObj.AddComponent<RectTransform>();
        nbtr.anchorMin = Vector2.zero; nbtr.anchorMax = Vector2.one;
        nbtr.offsetMin = Vector2.zero; nbtr.offsetMax = Vector2.zero;
        TextMeshProUGUI nbText = nbTextObj.AddComponent<TextMeshProUGUI>();
        nbText.text = "Adim 2'ye Gec  ->";
        nbText.fontSize = 16;
        nbText.fontStyle = FontStyles.Bold;
        nbText.color = Color.white;
        nbText.alignment = TextAlignmentOptions.Center;

        // === ADIM 2 BUTONU ===
        GameObject next2Btn = new GameObject("Step2NextButton");
        next2Btn.transform.SetParent(reportPanel.transform, false);
        RectTransform nb2r = next2Btn.AddComponent<RectTransform>();
        nb2r.anchorMin = new Vector2(0, 0);
        nb2r.anchorMax = new Vector2(1, 0);
        nb2r.pivot = new Vector2(0.5f, 0);
        nb2r.sizeDelta = new Vector2(-40, 50);
        nb2r.anchoredPosition = new Vector2(0, 50);
        Image nb2Img = next2Btn.AddComponent<Image>();
        nb2Img.color = new Color(0.15f, 0.4f, 0.8f, 1f);
        Button next2Button = next2Btn.AddComponent<Button>();
        GameObject nb2TextObj = new GameObject("Text");
        nb2TextObj.transform.SetParent(next2Btn.transform, false);
        RectTransform nb2tr = nb2TextObj.AddComponent<RectTransform>();
        nb2tr.anchorMin = Vector2.zero; nb2tr.anchorMax = Vector2.one;
        nb2tr.offsetMin = Vector2.zero; nb2tr.offsetMax = Vector2.zero;
        TextMeshProUGUI nb2Text = nb2TextObj.AddComponent<TextMeshProUGUI>();
        nb2Text.text = "Adim 3'e Gec  ->";
        nb2Text.fontSize = 16;
        nb2Text.fontStyle = FontStyles.Bold;
        nb2Text.color = Color.white;
        nb2Text.alignment = TextAlignmentOptions.Center;
        next2Btn.SetActive(false);

        // === ADIM 3 BUTONU ===
        GameObject next3Btn = new GameObject("Step3NextButton");
        next3Btn.transform.SetParent(reportPanel.transform, false);
        RectTransform nb3r = next3Btn.AddComponent<RectTransform>();
        nb3r.anchorMin = new Vector2(0, 0);
        nb3r.anchorMax = new Vector2(1, 0);
        nb3r.pivot = new Vector2(0.5f, 0);
        nb3r.sizeDelta = new Vector2(-40, 50);
        nb3r.anchoredPosition = new Vector2(0, 50);
        Image nb3Img = next3Btn.AddComponent<Image>();
        nb3Img.color = new Color(0.15f, 0.4f, 0.8f, 1f);
        Button next3Button = next3Btn.AddComponent<Button>();
        GameObject nb3TextObj = new GameObject("Text");
        nb3TextObj.transform.SetParent(next3Btn.transform, false);
        RectTransform nb3tr = nb3TextObj.AddComponent<RectTransform>();
        nb3tr.anchorMin = Vector2.zero; nb3tr.anchorMax = Vector2.one;
        nb3tr.offsetMin = Vector2.zero; nb3tr.offsetMax = Vector2.zero;
        TextMeshProUGUI nb3Text = nb3TextObj.AddComponent<TextMeshProUGUI>();
        nb3Text.text = "Adim 4'e Gec  ->";
        nb3Text.fontSize = 16;
        nb3Text.fontStyle = FontStyles.Bold;
        nb3Text.color = Color.white;
        nb3Text.alignment = TextAlignmentOptions.Center;
        next3Btn.SetActive(false);

        // === REPORT MANAGER BAĞLANTILARI ===
        if (reportManager != null)
        {
            reportManager.reportPanel = reportPanel;
            reportManager.toggleButton = toggleButton;
            reportManager.toggleButtonText = toggleText;
            reportManager.step1Input = inputFields[0];
            reportManager.step1NextButton = nextButton;
            reportManager.step2NextButton = next2Button;
            reportManager.step3NextButton = next3Button;
            reportManager.allInputFields = inputFields;
        }

        reportPanel.SetActive(false);
    }
}