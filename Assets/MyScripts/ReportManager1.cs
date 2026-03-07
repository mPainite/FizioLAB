using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ReportManager1 : MonoBehaviour
{
    [System.Serializable]
    public struct ExperimentData
    {
        public int dropNo;
        public string mass;
        public string electricField;
        public string charge;
        public int n;
    }

    [Header("UI Referanslarý")]
    public GameObject reportPanel;
    public TextMeshProUGUI reportStatusText; // Sol üstteki görev yazýsý
    public TextMeshProUGUI dataDisplay;      // Paneldeki veri alaný

    [Header("Veri Listesi")]
    public List<ExperimentData> tableData = new List<ExperimentData>();

    private void Start()
    {
        // Tablodaki verileri buraya ekliyoruz (Örnek olarak ilk birkaç tanesi)
        tableData.Add(new ExperimentData { dropNo = 1, mass = "1,18924E-14", electricField = "145830,1368", charge = "8E-19", n = 5 });
        tableData.Add(new ExperimentData { dropNo = 2, mass = "1,28064E-14", electricField = "261730,5575", charge = "4,8E-19", n = 3 });
        tableData.Add(new ExperimentData { dropNo = 8, mass = "6,20458E-15", electricField = "38041,85327", charge = "1,6E-18", n = 10 });
        // Resimdeki diðer 20 veriyi Inspector üzerinden de ekleyebilirsin.
    }

    private void OnMouseDown()
    {
        ShowRandomReport();
    }

    public void ShowRandomReport()
    {
        if (tableData.Count == 0) return;

        // Rastgele bir satýr seç
        int randomIndex = Random.Range(0, tableData.Count);
        ExperimentData selected = tableData[randomIndex];

        // UI Metnini oluþtur
        dataDisplay.text = $"Damla No: {selected.dropNo}\n\n" +
                           $"Kütle (kg): {selected.mass}\n\n" +
                           $"Elektrik Alan E (N/C): {selected.electricField}\n\n" +
                           $"Yük q (C): {selected.charge}\n\n" +
                           $"n (q = n·e): {selected.n}";

        // Paneli aç ve görev yazýsýný yeþil yap
        reportPanel.SetActive(true);
        if (reportStatusText != null) reportStatusText.color = Color.green;
    }

    public void CloseReport()
    {
        reportPanel.SetActive(false);
    }
}