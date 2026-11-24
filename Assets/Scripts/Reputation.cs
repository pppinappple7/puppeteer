using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reputation : MonoBehaviour
{
    [SerializeField] Sprite[] medals;
    [SerializeField] int[] milestones;
    [Header("References")]
    [SerializeField] Image medalImage;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] Slider pointBar;

    int points;
    int level;

    public int Points => points;

    void Start()
    {
        RefreshProgress();
    }

    public int GetLevel()
    {
        return level;
    }

    public void AddReputation(int amount)
    {
        long updatedPoints = (long)points + amount;

        if (updatedPoints < 0)
            updatedPoints = 0;
        else if (updatedPoints > int.MaxValue)
            updatedPoints = int.MaxValue;

        points = (int)updatedPoints;
        RefreshProgress();
    }

    public bool TrySetPoints(int value, out string error)
    {
        error = null;

        if (value < 0)
        {
            error = "Reputation points cannot be negative.";
            return false;
        }

        if (!TryCalculateProgress(value, out int calculatedLevel, out bool isMax, out int terminalThresholdIndex, out error))
            return false;

        points = value;
        level = calculatedLevel;
        RefreshUI(isMax, terminalThresholdIndex);
        return true;
    }

    void RefreshProgress()
    {
        if (!TryCalculateProgress(points, out int calculatedLevel, out bool isMax, out int terminalThresholdIndex, out _))
            return;

        level = calculatedLevel;
        RefreshUI(isMax, terminalThresholdIndex);
    }

    bool TryCalculateProgress(
        int value,
        out int calculatedLevel,
        out bool isMax,
        out int terminalThresholdIndex,
        out string error)
    {
        calculatedLevel = 0;
        isMax = false;
        terminalThresholdIndex = 0;
        error = null;

        if (medals == null || medals.Length == 0 || milestones == null)
        {
            error = "Reputation milestones or medals are not assigned.";
            return false;
        }

        if (milestones.Length != medals.Length && milestones.Length != medals.Length + 1)
        {
            error = "Milestones must have the same length as medals or one extra entry.";
            return false;
        }

        if (milestones.Length == 0)
        {
            error = "Reputation milestones are empty.";
            return false;
        }

        for (int i = 0; i < milestones.Length; i++)
        {
            if (milestones[i] < 0)
            {
                error = "Reputation milestones cannot be negative.";
                return false;
            }

            if (i > 1 && milestones[i] <= milestones[i - 1])
            {
                error = "Reputation milestones must be strictly increasing from index 1.";
                return false;
            }
        }

        int maxLevel = medals.Length - 1;
        terminalThresholdIndex = milestones.Length == medals.Length + 1
            ? medals.Length
            : maxLevel;

        for (int i = 1; i <= maxLevel; i++)
        {
            if (value >= milestones[i])
                calculatedLevel = i;
        }

        isMax = value >= milestones[terminalThresholdIndex];
        return true;
    }

    void RefreshUI(bool isMax, int terminalThresholdIndex)
    {
        if (levelText != null)
        {
            levelText.gameObject.SetActive(!isMax);
            levelText.text = level.ToString();
        }

        if (pointText != null)
            pointText.text = isMax ? "MAX" : points.ToString();

        if (medalImage != null && medals != null && level >= 0 && level < medals.Length)
            medalImage.sprite = medals[level];

        if (pointBar != null && milestones != null && milestones.Length > 0)
        {
            int thresholdIndex = isMax ? terminalThresholdIndex : Mathf.Min(level + 1, milestones.Length - 1);
            float maxValue = milestones[thresholdIndex];

            if (maxValue <= pointBar.minValue)
                maxValue = pointBar.minValue + 1f;

            pointBar.maxValue = maxValue;
            pointBar.value = Mathf.Clamp(points, pointBar.minValue, pointBar.maxValue);
        }
    }
}
