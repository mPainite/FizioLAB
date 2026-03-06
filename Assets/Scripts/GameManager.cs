using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Adım Takibi")]
    public int currentStep = 1;

    [Header("UI")]
    public TextMeshProUGUI taskText;

    [Header("Adım 1 Objeleri")]
    public DraggableCloth silkCloth;
    public DraggableCloth woolCloth;
    public DraggableRod glassRod;
    public PendulumRod pendulumRod;

    [Header("Adım 2 Objeleri")]
    public DraggableRod plasticRod;
    public PendulumRod plasticPendulumRod;

    [Header("Adım Durumları")]
    public bool step1Completed = false;
    public bool step2Completed = false;

    [Header("Adım 3 Alt Adımları")]
    public int step3SubStep = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Invoke("DelayedStart", 0.1f);
    }

    void DelayedStart()
    {
        SetStep(1);
    }

    public void SetStep(int step)
    {
        currentStep = step;
        UpdateTaskText();
        UpdateObjectAccess();

        if (step == 2)
        {
            if (glassRod != null) glassRod.ResetToTablePublic();
            if (pendulumRod != null) pendulumRod.gameObject.SetActive(false);
            if (plasticPendulumRod != null) plasticPendulumRod.gameObject.SetActive(true);
            if (silkCloth != null) silkCloth.ResetCloth();
        }

        if (step == 3)
        {
            step3SubStep = 1;
            if (plasticRod != null) plasticRod.ResetToTablePublic();
            if (glassRod != null) glassRod.ResetToTablePublic();
            if (plasticPendulumRod != null) plasticPendulumRod.gameObject.SetActive(false);
            if (pendulumRod != null) pendulumRod.gameObject.SetActive(false);
            if (silkCloth != null) silkCloth.ResetCloth();
            if (woolCloth != null) woolCloth.ResetCloth();
        }
    }

    public void UpdateStep3Access()
    {
        if (currentStep == 3)
        {
            UpdateObjectAccess();
            if (step3SubStep == 2 && plasticRod != null)
            {
                Renderer r = plasticRod.GetComponent<Renderer>();
                if (r != null) r.material.color = plasticRod.GetOriginalColor();
            }
        }
    }

    void UpdateTaskText()
    {
        switch (currentStep)
        {
            case 1:
                taskText.text = "Adım 1: Cam çubuğu elektriklemek için\nipek kumaşı üzerine sürükleyerek sürt.";
                break;
            case 2:
                taskText.text = "Adım 2: Plastik çubuğu elektriklemek için\nyünlü kumaşı üzerine sürükleyerek sürt.";
                break;
            case 3:
                taskText.text = "Adım 3: Önce cam çubuğu yünlü kumaşla yükleyip asın!";
                break;
        }
    }

    void UpdateObjectAccess()
    {
        switch (currentStep)
        {
            case 1:
                SetClothActive(silkCloth, true);
                SetClothActive(woolCloth, false);
                SetRodDraggable(glassRod, true);
                SetRodDraggable(plasticRod, false);
                break;
            case 2:
                SetClothActive(silkCloth, false);
                SetClothActive(woolCloth, true);
                SetRodDraggable(glassRod, false);
                SetRodDraggable(plasticRod, true);
                break;
            case 3:
                if (step3SubStep == 1)
                {
                    SetClothActive(woolCloth, true);
                    SetClothActive(silkCloth, false);
                    SetRodDraggable(glassRod, true);
                    SetRodDraggable(plasticRod, false);
                }
                else if (step3SubStep == 2)
                {
                    SetClothActive(woolCloth, false);
                    SetClothActive(silkCloth, true);
                    SetRodDraggable(glassRod, false);
                    SetRodDraggable(plasticRod, true);
                }
                else
                {
                    SetClothActive(woolCloth, false);
                    SetClothActive(silkCloth, false);
                    SetRodDraggable(glassRod, false);
                    SetRodDraggable(plasticRod, false);
                }
                break;
        }
    }

    void SetClothActive(DraggableCloth cloth, bool active)
    {
        if (cloth == null) return;
        cloth.enabled = active;
        Collider col = cloth.GetComponent<Collider>();
        if (col != null) col.enabled = active;
        Renderer r = cloth.GetComponent<Renderer>();
        if (r != null) r.material.color = active ? r.material.color : new Color(0.4f, 0.4f, 0.4f);
    }

    void SetRodDraggable(DraggableRod rod, bool active)
    {
        if (rod == null) return;
        if (rod.currentState == DraggableRod.RodState.Suspended) return;

        rod.enabled = active;
        Collider col = rod.GetComponent<Collider>();
        if (col != null) col.enabled = active;
        Rigidbody rb = rod.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        // Rengi sadece orijinale döndür, griye çevirme
        Renderer r = rod.GetComponent<Renderer>();
        if (r != null) r.material.color = rod.GetOriginalColor();
    }
    public void CompleteStep(int step)
    {
        if (step == 1) step1Completed = true;
        if (step == 2) step2Completed = true;
        Debug.Log("Adım " + step + " tamamlandı!");
    }
}