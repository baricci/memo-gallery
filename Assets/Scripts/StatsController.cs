using System.Collections;
using TMPro;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance;

    [Header("Easy")]
    public TextMeshProUGUI easyScore;
    public TextMeshProUGUI easyTime;
    public TextMeshProUGUI easyMoves;
    public TextMeshProUGUI easyGames;

    [Header("Medium")]
    public TextMeshProUGUI mediumScore;
    public TextMeshProUGUI mediumTime;
    public TextMeshProUGUI mediumMoves;
    public TextMeshProUGUI mediumGames;

    [Header("Hard")]
    public TextMeshProUGUI hardScore;
    public TextMeshProUGUI hardTime;
    public TextMeshProUGUI hardMoves;
    public TextMeshProUGUI hardGames;

    [Header("Extreme")]
    public TextMeshProUGUI extremeScore;
    public TextMeshProUGUI extremeTime;
    public TextMeshProUGUI extremeMoves;
    public TextMeshProUGUI extremeGames;

    [Header("UI References")]
    public GameObject statsPanel;
    public GameObject securityReset;
    public GameObject successResetText;

    private bool isOpen = false;
    private bool showReset = false;
    private Animator statsAnimator;
    private Animator resetAnimator;
    private Animator successAnimator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        statsAnimator = statsPanel.GetComponent<Animator>();
        resetAnimator = securityReset.GetComponent<Animator>();
        successAnimator = successResetText.GetComponent<Animator>();
    }

    public void RefreshStats()
    {
        LoadRow("Easy", easyScore, easyTime, easyMoves, easyGames);
        LoadRow("Medium", mediumScore, mediumTime, mediumMoves, mediumGames);
        LoadRow("Hard", hardScore, hardTime, hardMoves, hardGames);
        LoadRow("Extreme", extremeScore, extremeTime, extremeMoves, extremeGames);
    }

    private void LoadRow(string difficulty,
        TextMeshProUGUI score, TextMeshProUGUI time, TextMeshProUGUI moves, TextMeshProUGUI games)
    {
        score.text = PlayerPrefs.GetInt($"BestScore_{difficulty}", 0).ToString();

        float bestTime = PlayerPrefs.GetFloat($"BestTime_{difficulty}", 0);
        time.text = bestTime == 0 ? "-" : FormatTime(bestTime);

        int bestMoves = PlayerPrefs.GetInt($"BestMoves_{difficulty}", 0);
        moves.text = bestMoves == 0 ? "-" : bestMoves.ToString();

        games.text = PlayerPrefs.GetInt($"GamesPlayed_{difficulty}", 0).ToString();
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{minutes:00}:{seconds:00}";
    }

    public void ResetStats()
    {
        DeletePrefs();
        RefreshStats();

        StartCoroutine(ShowResetSuccess());
    }

    private void DeletePrefs()
    {
        PlayerPrefs.DeleteKey("BestScore_Easy");
        PlayerPrefs.DeleteKey("BestTime_Easy");
        PlayerPrefs.DeleteKey("BestMoves_Easy");
        PlayerPrefs.DeleteKey("GamesPlayed_Easy");

        PlayerPrefs.DeleteKey("BestScore_Medium");
        PlayerPrefs.DeleteKey("BestTime_Medium");
        PlayerPrefs.DeleteKey("BestMoves_Medium");
        PlayerPrefs.DeleteKey("GamesPlayed_Medium");

        PlayerPrefs.DeleteKey("BestScore_Hard");
        PlayerPrefs.DeleteKey("BestTime_Hard");
        PlayerPrefs.DeleteKey("BestMoves_Hard");
        PlayerPrefs.DeleteKey("GamesPlayed_Hard");

        PlayerPrefs.DeleteKey("BestScore_Extreme");
        PlayerPrefs.DeleteKey("BestTime_Extreme");
        PlayerPrefs.DeleteKey("BestMoves_Extreme");
        PlayerPrefs.DeleteKey("GamesPlayed_Extreme");
    }

    public void ToggleStatsPanel()
    {
        isOpen = !isOpen;
        statsAnimator.SetBool("IsOpen", isOpen);

        if (isOpen)
            RefreshStats();
    }

    public void ToggleResetContainer()
    {
        showReset = !showReset;
        resetAnimator.SetBool("ShowReset", showReset);

        if (!showReset)
            successResetText.SetActive(true);
    }

    private IEnumerator ShowResetSuccess()
    {
        successAnimator.SetTrigger("ShowSuccess");
        yield return new WaitForSeconds(1.6f);
        ToggleResetContainer();
    }
}