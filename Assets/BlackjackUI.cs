using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlackjackUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject cardBackPanel;
    public GameObject rulesPanel;

    [Header("Main Menu")]
    public Button startGameButton;
    public Button chooseCardBackButton;
    public Button rulesButton;

    [Header("Game Panel")]
    public TextMeshProUGUI playerHandText;
    public TextMeshProUGUI dealerHandText;
    public Button hitButton;
    public Button standButton;
    public TextMeshProUGUI gameResultText;
    public Button returnToMenuButton;

    [Header("Card Back Selection")]
    public Button[] cardBackButtons;
    public Button cardBackCloseButton;

    [Header("Rules Panel")]
    public TextMeshProUGUI rulesText;
    public Button rulesCloseButton;

    private BlackjackGame blackjackGame;

    private void Start()
    {
        blackjackGame = GetComponent<BlackjackGame>();

        // Set up button listeners
        startGameButton.onClick.AddListener(StartGame);
        chooseCardBackButton.onClick.AddListener(OpenCardBackPanel);
        rulesButton.onClick.AddListener(OpenRulesPanel);
        hitButton.onClick.AddListener(OnHitButtonClicked);
        standButton.onClick.AddListener(OnStandButtonClicked);
        returnToMenuButton.onClick.AddListener(ReturnToMainMenu);
        cardBackCloseButton.onClick.AddListener(CloseCardBackPanel);
        rulesCloseButton.onClick.AddListener(CloseRulesPanel);

        // Initially show only the main menu
        ShowPanel(mainMenuPanel);
        HidePanel(gamePanel);
        HidePanel(cardBackPanel);
        HidePanel(rulesPanel);

        // Set up rules text
        rulesText.text = "Blackjack Rules:\n\n" +
            "1. The goal is to get as close to 21 points as possible without going over.\n" +
            "2. Face cards are worth 10, Aces are 1 or 11, other cards are face value.\n" +
            "3. Click 'Hit' to draw another card, or 'Stay' to keep your current hand.\n" +
            "4. If you go over 21, you 'bust' and lose.\n" +
            "5. After you stay, the dealer plays. They must hit on 16 and stay on 17.\n" +
            "6. Closest to 21 without busting wins!";
    }

    private void StartGame()
    {
        ShowPanel(gamePanel);
        HidePanel(mainMenuPanel);
        bool gameOverImmediately = blackjackGame.StartNewGame();
        UpdateGameUI();
        if (gameOverImmediately)
        {
            // Disable hit and stand buttons
            hitButton.interactable = false;
            standButton.interactable = false;
        }
    }

    private void OnHitButtonClicked()
    {
        blackjackGame.PlayerHit();
        UpdateGameUI();
    }

    private void OnStandButtonClicked()
    {
        blackjackGame.PlayerStand();
        UpdateGameUI();
    }

    private void ReturnToMainMenu()
    {
        ShowPanel(mainMenuPanel);
        HidePanel(gamePanel);
    }

    private void OpenCardBackPanel()
    {
        ShowPanel(cardBackPanel);
        HidePanel(mainMenuPanel);
    }

    private void CloseCardBackPanel()
    {
        ShowPanel(mainMenuPanel);
        HidePanel(cardBackPanel);
    }

    private void OpenRulesPanel()
    {
        ShowPanel(rulesPanel);
        HidePanel(mainMenuPanel);
    }

    private void CloseRulesPanel()
    {
        ShowPanel(mainMenuPanel);
        HidePanel(rulesPanel);
    }

    private void UpdateGameUI()
    {
        playerHandText.text = "Player Hand: " + blackjackGame.GetPlayerHandAsString();
        dealerHandText.text = "Dealer Hand: " + blackjackGame.GetDealerHandAsString();
        gameResultText.text = blackjackGame.GetGameResult();

        // Enable/disable hit and stand buttons based on game state
        hitButton.interactable = blackjackGame.CanPlayerHit();
        standButton.interactable = blackjackGame.CanPlayerStand();

        // If the game is over, show the return to menu button
        if (!hitButton.interactable && !standButton.interactable)
        {
            returnToMenuButton.gameObject.SetActive(true);
        }
        else
        {
            returnToMenuButton.gameObject.SetActive(false);
        }
    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}