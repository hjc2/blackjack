using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlackjackUI : MonoBehaviour
{
    [Header("Main Menu")]
    public Canvas mainMenuCanvas;
    public Button startGameButton;
    public Button chooseCardBackButton;
    public Button rulesButton;

    [Header("Card Back Selection")]
    public GameObject cardBackPanel;
    public Button[] cardBackButtons;
    public Button cardBackCloseButton;

    [Header("Rules Panel")]
    public GameObject rulesPanel;
    public Button rulesCloseButton;
    public TextMeshProUGUI rulesText;

    [Header("Button Text")]
    public TextMeshProUGUI startGameButtonText;
    public TextMeshProUGUI chooseCardBackButtonText;
    public TextMeshProUGUI rulesButtonText;
    public TextMeshProUGUI cardBackCloseButtonText;
    public TextMeshProUGUI rulesCloseButtonText;

    private void Start()
    {
        // Set up button listeners
        startGameButton.onClick.AddListener(StartGame);
        chooseCardBackButton.onClick.AddListener(OpenCardBackPanel);
        rulesButton.onClick.AddListener(OpenRulesPanel);
        cardBackCloseButton.onClick.AddListener(CloseCardBackPanel);
        rulesCloseButton.onClick.AddListener(CloseRulesPanel);

        // Initially hide panels
        cardBackPanel.SetActive(false);
        rulesPanel.SetActive(false);

        // Set up button text
        startGameButtonText.text = "Start Game";
        chooseCardBackButtonText.text = "Choose Card Back";
        rulesButtonText.text = "Rules";
        cardBackCloseButtonText.text = "Close";
        rulesCloseButtonText.text = "Close";

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
        // TODO: Implement game start logic
        Debug.Log("Starting the game");
    }

    private void OpenCardBackPanel()
    {
        cardBackPanel.SetActive(true);
    }

    private void CloseCardBackPanel()
    {
        cardBackPanel.SetActive(false);
    }

    private void OpenRulesPanel()
    {
        rulesPanel.SetActive(true);
    }

    private void CloseRulesPanel()
    {
        rulesPanel.SetActive(false);
    }

    // TODO: Implement method to handle card back selection
}