using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlackjackUIManager : MonoBehaviour
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
        SetupButton(startGameButton, StartGame, "Start Game Button");
        SetupButton(chooseCardBackButton, OpenCardBackPanel, "Choose Card Back Button");
        SetupButton(rulesButton, OpenRulesPanel, "Rules Button");
        SetupButton(cardBackCloseButton, CloseCardBackPanel, "Card Back Close Button");
        SetupButton(rulesCloseButton, CloseRulesPanel, "Rules Close Button");

        // Initially hide panels
        SetActiveWithNull(cardBackPanel, false, "Card Back Panel");
        SetActiveWithNull(rulesPanel, false, "Rules Panel");

        // Set up button text
        SetTextWithNull(startGameButtonText, "Start Game", "Start Game Button Text");
        SetTextWithNull(chooseCardBackButtonText, "Choose Card Back", "Choose Card Back Button Text");
        SetTextWithNull(rulesButtonText, "Rules", "Rules Button Text");
        SetTextWithNull(cardBackCloseButtonText, "Close", "Card Back Close Button Text");
        SetTextWithNull(rulesCloseButtonText, "Close", "Rules Close Button Text");

        // Set up rules text
        SetTextWithNull(rulesText, "Blackjack Rules:\n\n" +
            "1. The goal is to get as close to 21 points as possible without going over.\n" +
            "2. Face cards are worth 10, Aces are 1 or 11, other cards are face value.\n" +
            "3. Click 'Hit' to draw another card, or 'Stay' to keep your current hand.\n" +
            "4. If you go over 21, you 'bust' and lose.\n" +
            "5. After you stay, the dealer plays. They must hit on 16 and stay on 17.\n" +
            "6. Closest to 21 without busting wins!", "Rules Text");
    }

    private void SetupButton(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
        else
        {
            Debug.LogError($"{buttonName} is not assigned in the Inspector.");
        }
    }

    private void SetActiveWithNull(GameObject obj, bool active, string objectName)
    {
        if (obj != null)
        {
            obj.SetActive(active);
        }
        else
        {
            Debug.LogError($"{objectName} is not assigned in the Inspector.");
        }
    }

    private void SetTextWithNull(TextMeshProUGUI textComponent, string text, string componentName)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
        }
        else
        {
            Debug.LogError($"{componentName} is not assigned in the Inspector.");
        }
    }

    private void StartGame()
    {
        Debug.Log("Starting the game");
    }

    private void OpenCardBackPanel()
    {
        SetActiveWithNull(cardBackPanel, true, "Card Back Panel");
    }

    private void CloseCardBackPanel()
    {
        SetActiveWithNull(cardBackPanel, false, "Card Back Panel");
    }

    private void OpenRulesPanel()
    {
        SetActiveWithNull(rulesPanel, true, "Rules Panel");
    }

    private void CloseRulesPanel()
    {
        SetActiveWithNull(rulesPanel, false, "Rules Panel");
    }

    // TODO: Implement method to handle card back selection
}