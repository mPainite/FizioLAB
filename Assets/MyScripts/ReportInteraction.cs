using UnityEngine;
using TMPro; // TextMeshPro bileþenine eriþmek için

public class ReportInteraction : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public TextMeshProUGUI reportStatusText; // "Deney Raporunu Ýncele" yazýsý

    private bool isExamined = false;

    private void OnMouseDown()
    {
        ExamineReport();
    }

    void ExamineReport()
    {
        // Yazý rengini yeþile çevir
        if (!isExamined && reportStatusText != null)
        {
            reportStatusText.color = Color.green;
            isExamined = true;
            Debug.Log("Deney raporu incelendi, yazý yeþile döndü.");

            // Ýstersen burada raporun detaylarýný gösteren bir Panel de açabilirsin
        }
    }
}