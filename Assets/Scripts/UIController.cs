using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private PanelType currentPanel;
    private PanelType previousPanel;

    [Header("UI References")]
    public GameObject exitScreen;
    public GameObject splashPanel;
    public GameObject menuPanel;
    public GameObject helpPanel;
    public GameObject modePanel;

    [Header("UI Confirm Exit Pause")]
    public GameObject quitContainer;
    public GameObject restartContainer;
    public Button noQuit;
    public Button noRestart;

    private Animator exitAnimator;

    private bool wantQuit = false;
    private bool isQuitting = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        exitAnimator = exitScreen.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBack();
        }
    }

    public void HandleBack()
    {
        switch (currentPanel)
        {
            case PanelType.Game:
                OpenPausePanel(); break;
            case PanelType.Pause:
                ClosePausePanel(); break;
            case PanelType.Stats:
            case PanelType.Help:
            case PanelType.Settings:
            case PanelType.Menu:
            case PanelType.Mode:
                CloseCurrentPanel(); break;
            case PanelType.ResetConfirm:
                CloseResetPanel(); break;
            case PanelType.Splash:
            case PanelType.EndGame:
                OpenExitPanel(); break;
            case PanelType.Exit:
                CloseExitPanel(); break;
        }
    }

    private void OpenPausePanel()
    {
        GameController.Instance.PauseTimer();
    }

    private void ClosePausePanel()
    {
        GameController.Instance.ResumeGame();

        if (quitContainer.activeSelf)
            noQuit.onClick.Invoke();

        if (restartContainer.activeSelf)
            noRestart.onClick.Invoke();
    }

    private void CloseCurrentPanel()
    {
        switch (currentPanel)
        {
            case PanelType.Stats:
                StatsController.Instance.ToggleStatsPanel();
                SetCurrentPanel(PanelType.Menu); break;
            case PanelType.Help:
                helpPanel.SetActive(false);
                SetCurrentPanel(PanelType.Menu); break;
            case PanelType.Settings:
                SettingsController.Instance.ToggleSettings();
                SetCurrentPanel(previousPanel); break;
            case PanelType.Menu:
                modePanel.SetActive(true);
                menuPanel.SetActive(false);
                SetCurrentPanel(PanelType.Mode); break;
            case PanelType.Mode:
                splashPanel.SetActive(true);
                modePanel.SetActive(false);
                SetCurrentPanel(PanelType.Splash); break;
        }
    }

    private void CloseResetPanel()
    {
        StatsController.Instance.ToggleResetContainer();
        SetCurrentPanel(PanelType.Stats);
    }

    public void OpenExitPanel()
    {
        previousPanel = currentPanel;

        wantQuit = true;
        exitAnimator.SetBool("IsOpen", wantQuit);
        SetCurrentPanel(PanelType.Exit);
    }

    public void CloseExitPanel()
    {
        wantQuit = false;
        exitAnimator.SetBool("IsOpen", wantQuit);
        SetCurrentPanel(previousPanel);
    }

    public void ConfirmExit()
    {
        if (isQuitting) return;

        isQuitting = true;

        exitAnimator.SetTrigger("IsYes");
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(1.6f);
        Application.Quit();
    }

    public void SetCurrentPanel(PanelType panel)
    {
        currentPanel = panel;
    }

    public void SetPanel(int panel)
    {
        SetCurrentPanel((PanelType)panel);
    }

    public void SetPrevious()
    {
        previousPanel = currentPanel;
    }
}

public enum PanelType
{
    Splash, Menu, Game, EndGame, Pause,
    Settings, Help, Stats, ResetConfirm, Exit,
    Mode
}