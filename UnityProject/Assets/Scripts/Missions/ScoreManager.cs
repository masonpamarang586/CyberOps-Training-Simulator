using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int startingScore = 100;

    private int currentScore;
    private int totalPenalties;
    private int hintsUsed;

    public int StartingScore => startingScore;
    public int CurrentScore => currentScore;
    public int TotalPenalties => totalPenalties;
    public int HintsUsed => hintsUsed;

    private void Awake()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        currentScore = startingScore;
        totalPenalties = 0;
        hintsUsed = 0;
    }

    public int ApplyHintPenalty(int tier)
    {
        int penalty = GetHintPenalty(tier);

        currentScore = Mathf.Max(
            0,
            currentScore - penalty
        );

        totalPenalties += penalty;
        hintsUsed++;

        return penalty;
    }

    private int GetHintPenalty(int tier)
    {
        switch (tier)
        {
            case 1:
                return 5;

            case 2:
                return 10;

            case 3:
                return 20;

            default:
                return 0;
        }
    }

    public string GetPerformanceRating()
    {
        if (currentScore >= 90)
            return "Excellent";

        if (currentScore >= 75)
            return "Proficient";

        if (currentScore >= 60)
            return "Developing";

        return "Needs Improvement";
    }

    public string GetScoreSummary()
    {
        return
            $"Score: {currentScore}/{startingScore}\n" +
            $"Hints Used: {hintsUsed}\n" +
            $"Rating: {GetPerformanceRating()}";
    }
}