using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;


public class ExperienceManager : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    int currentLevel, totalExperience;
    int previousLevelsExperience, nextLevelsExperience;

    [Header("Interface")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceFill;

    void Start()
    {
        UpdateLevel();
    }

    void Update() 
    {
        // var ts = Touchscreen.current;
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Debug.Log("Click detected");
            // AddExperience(5);
        }
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        // UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if(totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();

            // Start level up sequence... Possibly vfx?
        }
    }

    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        // UpdateInterface();
    }

    void UpdateInterface()
    {
        // 🔍 Debug: Cek apakah objek aktif
        if (levelText == null)
        {
            Debug.LogError("levelText is null!");
            return;
        }

        if (!levelText.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("levelText is disabled! GameObject is inactive.");
            return;
        }

        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;
        float fill = end > 0 ? (float)start / (float)end : 0f;

        levelText.text = (currentLevel + 1).ToString();
        experienceText.text = start + " exp";
        experienceFill.fillAmount = fill;
    }
}
