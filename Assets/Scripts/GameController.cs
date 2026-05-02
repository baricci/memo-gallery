using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Card Setup")]
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public Sprite cardBack;

    [Header("UI References")]
    public GameObject winScreen;
    public GameObject pauseScreen;

    [SerializeField] private Transform cardGrid;

    private List<Sprite> deck = new();
    private Card firstCard, secondCard;
    private int pairsFound = 0;
    private int totalPairs;
    private bool isInputLocked = false;
    private bool hasGameEnded = false;
    private Difficulty currentDifficulty;

    [Header("Stats UI")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI timeStatsValue;
    public TextMeshProUGUI movesStatsValue;
    public TextMeshProUGUI scoreStatsValue;
    public GameObject newRecordLabel;

    private float elapsedTime;
    private bool timerRunning;
    private int moves;
    private int score;
    private bool isNewRecord = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (!timerRunning) return;

        elapsedTime += Time.deltaTime;
        timeText.text = FormatTime(elapsedTime);
    }

    public void SelectDifficulty(int pairs)
    {
        totalPairs = pairs;

        currentDifficulty = pairs switch
        {
            4 => Difficulty.Easy,
            8 => Difficulty.Medium,
            12 => Difficulty.Hard,
            16 => Difficulty.Extreme,
            _ => Difficulty.Easy
        };

        CleanGrid();
        deck = GenerateDeck(pairs);
        StartCoroutine(InitGrid());
    }

    public void ReplayGame()
    {
        ChangeDifficulty();
        SelectDifficulty(totalPairs);

        UIController.Instance.SetCurrentPanel(PanelType.Game);
    }

    public void PauseTimer()
    {
        timerRunning = false;
        SFXManager.Instance.PlayButton();
        pauseScreen.GetComponent<Animator>().SetBool("inPause", true);

        UIController.Instance.SetCurrentPanel(PanelType.Pause);
    }

    public void ResumeGame()
    {
        SFXManager.Instance.PlayButton();

        pauseScreen.GetComponent<Animator>().SetBool("inPause", false);
        timerRunning = true;

        UIController.Instance.SetCurrentPanel(PanelType.Game);
    }

    public void QuitGame()
    {
        SFXManager.Instance.PlayButton();

        ChangeDifficulty();
        pauseScreen.GetComponent<Animator>().SetBool("inPause", false);

        UIController.Instance.SetCurrentPanel(PanelType.Menu);
    }

    private IEnumerator InitGrid()
    {
        yield return new WaitForEndOfFrame();

        BuildResponsiveGrid();
        GenerateCards();
        LayoutRebuilder.ForceRebuildLayoutImmediate(cardGrid.GetComponent<RectTransform>());

        elapsedTime = 0f;
        timerRunning = true;
    }

    private List<(int id, Sprite icon)> GenerateCardData(int pairsNeeded)
    {
        List<(int id, Sprite icon)> cards = new();
        List<Sprite> icons = GetRandomIcons(pairsNeeded);

        for (int i = 0; i < icons.Count; i++)
        {
            cards.Add((i, icons[i]));
            cards.Add((i, icons[i]));
        }
        cards = cards.OrderBy(x => Random.value).ToList();

        return cards;
    }

    private void GenerateCards()
    {
        var cardData = GenerateCardData(totalPairs);

        foreach (var (id, icon) in cardData)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardGrid);
            Card card = cardObj.GetComponent<Card>();
            card.Setup(id, icon, cardBack, this);
        }
    }

    private List<Sprite> GetRandomIcons(int pairsNeeded)
    {
        List<Sprite> availableIcons = new(cardFaces);

        availableIcons = availableIcons.OrderBy(x => Random.value).ToList();
        List<Sprite> selectedIcons = availableIcons.Take(pairsNeeded).ToList();

        return selectedIcons;
    }

    private List<Sprite> GenerateDeck(int pairsNeeded)
    {
        List<Sprite> deck = new();
        List<Sprite> selectedIcons = GetRandomIcons(pairsNeeded);

        foreach (Sprite icon in selectedIcons)
        {
            deck.Add(icon);
            deck.Add(icon);
        }
        deck = deck.OrderBy(x => Random.value).ToList();

        return deck;
    }

    private void CleanGrid()
    {
        foreach (Transform child in cardGrid)
        {
            Destroy(child.gameObject);
        }

        deck.Clear();
        pairsFound = 0;
        moves = 0;
        movesText.text = moves.ToString();
        newRecordLabel.SetActive(false);
    }

    public void CardRevealed(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            isInputLocked = true;
            moves++;
            movesText.text = moves.ToString();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (firstCard.id == secondCard.id)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (SettingsController.Instance.IsVibrationEnabled())
                Handheld.Vibrate();
#endif

            firstCard.animator.SetBool("IsMatched", true);
            secondCard.animator.SetBool("IsMatched", true);
            SFXManager.Instance.PlayMatch();

            pairsFound++;

            if (pairsFound >= totalPairs && !hasGameEnded)
            {
                timerRunning = false;
                score = CalculateScore();
                CheckBestScore();
                SaveStats();
                UpdateStatsUI();

                hasGameEnded = true;
                isInputLocked = true;
                yield return new WaitForSeconds(0.5f);
                winScreen.GetComponent<Animator>().SetInteger("WinState", isNewRecord ? 2 : 1);

                SFXManager.Instance.PlayVictory();
                UIController.Instance.SetCurrentPanel(PanelType.EndGame);
            }
        }
        else
        {
            SFXManager.Instance.PlayMismatch();
            firstCard.Hide();
            secondCard.Hide();
        }

        firstCard = null;
        secondCard = null;
        isInputLocked = false;
    }

    private void CheckBestScore()
    {
        string key = GetKey("BestScore");

        int bestScore = PlayerPrefs.GetInt(key, 0);

        if (score > bestScore)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
            isNewRecord = true;
        }
        else
            isNewRecord = false;
    }

    private void BuildResponsiveGrid()
    {
        GridLayoutGroup grid = cardGrid.GetComponent<GridLayoutGroup>();

        Vector2 space = GetAvailableSpace();

        int totalCards = totalPairs * 2;

        int bestColumns = CalculateBestColumns(space, totalCards);
        int rows = Mathf.CeilToInt((float)totalCards / bestColumns);

        float spacing = CalculateSpacing(space);
        grid.spacing = new Vector2(spacing, spacing);

        float padding = spacing * 0.5f;
        grid.padding = new RectOffset((int)padding, (int)padding, (int)padding, (int)padding);

        float availableWidth = space.x - spacing * (bestColumns - 1) - padding * 2;
        float availableHeight = space.y - spacing * (rows - 1) - padding * 2;

        float cellWidth = availableWidth / bestColumns;
        float cellHeight = availableHeight / rows;

        float size = Mathf.Min(cellWidth, cellHeight);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = bestColumns;
        grid.cellSize = new Vector2(size, size);
    }

    private Vector2 GetAvailableSpace()
    {
        RectTransform rect = cardGrid.GetComponent<RectTransform>();
        rect.ForceUpdateRectTransforms();

        float width = rect.rect.width;
        float height = rect.rect.height;

        return new Vector2(width, height);
    }

    private int CalculateBestColumns(Vector2 space, int totalCards)
    {
        int minColumns = GetMinColumns();
        int maxColumns = GetMaxColumns();

        int bestColumns = minColumns;
        float bestSize = 0;

        for (int cols = minColumns; cols <= maxColumns; cols++)
        {
            int rows = Mathf.CeilToInt((float)totalCards / cols);

            float cellWidth = space.x / cols;
            float cellHeight = space.y / rows;

            float size = Mathf.Min(cellWidth, cellHeight);

            if (size > bestSize)
            {
                bestSize = size;
                bestColumns = cols;
            }
        }

        return bestColumns;
    }

    private int GetMinColumns()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 2,
            Difficulty.Medium => 3,
            Difficulty.Hard => 4,
            Difficulty.Extreme => 4,
            _ => 3
        };
    }

    private int GetMaxColumns()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 4,
            Difficulty.Medium => 5,
            Difficulty.Hard => 6,
            Difficulty.Extreme => 8,
            _ => 6
        };
    }

    private float CalculateSpacing(Vector2 space)
    {
        float minSide = Mathf.Min(space.x, space.y);

        return Mathf.Clamp(minSide * 0.02f, 4f, 20f);
    }

    public void ChangeDifficulty()
    {
        hasGameEnded = false;
        isInputLocked = false;

        winScreen.GetComponent<Animator>().SetInteger("WinState", 0);
        winScreen.GetComponent<Animator>().SetTrigger("Restart");
    }

    public bool IsInputLocked()
    {
        return isInputLocked;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    private int CalculateScore()
    {
        int idealMoves = GetIdealMoves();
        float idealTime = GetIdealTime();
        float difficultyMultiplier = GetDifficultyMultiplier();

        float moveScore = (float)idealMoves / moves;
        moveScore = Mathf.Clamp01(moveScore);

        float timeScore = idealTime / elapsedTime;
        timeScore = Mathf.Clamp01(timeScore);

        float performance = (moveScore * 0.6f) + (timeScore * 0.4f);

        int baseScore = totalPairs * 100;

        int finalScore = Mathf.RoundToInt(baseScore * performance * difficultyMultiplier);
        return Mathf.Max(0, finalScore);
    }

    private void UpdateStatsUI()
    {
        timeStatsValue.text = FormatTime(elapsedTime);
        scoreStatsValue.text = score.ToString();
        movesStatsValue.text = movesText.text;

        newRecordLabel.SetActive(isNewRecord);
    }


    private int GetIdealMoves()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 5,
            Difficulty.Medium => 12,
            Difficulty.Hard => 21,
            Difficulty.Extreme => 32,
            _ => 20
        };
    }

    private float GetIdealTime()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 12f,
            Difficulty.Medium => 30f,
            Difficulty.Hard => 50f,
            Difficulty.Extreme => 75f,
            _ => 60f
        };
    }

    private float GetDifficultyMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => 1.0f,
            Difficulty.Medium => 1.5f,
            Difficulty.Hard => 2f,
            Difficulty.Extreme => 2.5f,
            _ => 1.0f
        };
    }

    private string GetKey(string stat)
    {
        return $"{stat}_{currentDifficulty}";
    }

    private void SaveStats()
    {
        SaveBestScore();
        SaveBestTime();
        SaveBestMoves();
        IncreseGamesPlayed();
    }

    private void SaveBestScore()
    {
        string key = GetKey("BestScore");

        int best = PlayerPrefs.GetInt(key, 0);

        if (score > best)
        {
            PlayerPrefs.SetInt(key, score);
            isNewRecord = true;
        }
    }

    private void SaveBestTime()
    {
        string key = GetKey("BestTime");

        float best = PlayerPrefs.GetFloat(key, float.MaxValue);

        if (elapsedTime < best)
        {
            PlayerPrefs.SetFloat(key, elapsedTime);
        }
    }

    private void SaveBestMoves()
    {
        string key = GetKey("BestMoves");

        int best = PlayerPrefs.GetInt(key, int.MaxValue);

        if (moves < best)
        {
            PlayerPrefs.SetInt(key, moves);
        }
    }

    private void IncreseGamesPlayed()
    {
        string key = GetKey("GamesPlayed");

        int played = PlayerPrefs.GetInt(key, 0);

        PlayerPrefs.SetInt(key, played + 1);
    }
}

public enum Difficulty
{
    Easy, Medium, Hard, Extreme
}