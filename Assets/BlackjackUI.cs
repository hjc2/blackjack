using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [Header("Card Layout")]
    public float cardSpacing = 40f;
    public float dealerCardOffset = 20f;

    [Header("Game Panel")]
    public Transform playerHandArea;
    public Transform dealerHandArea;
    public Button hitButton;
    public Button standButton;
    public Button restartButton;
    public TextMeshProUGUI gameResultText;
    public Button returnToMenuButton;

    [Header("Card Back Selection")]
    public Sprite[] cardBackSprites;

    public Button[] cardBackButtons;
    public Button cardBackCloseButton;
    private int selectedCardBackIndex = 0;

    [Header("Rules Panel")]
    public TextMeshProUGUI rulesText;
    public Button rulesCloseButton;

    [Header("Card Prefab")]
    public GameObject cardPrefab;

    private BlackjackGame blackjackGame;
    private List<GameObject> playerCardObjects = new List<GameObject>();
    private List<GameObject> dealerCardObjects = new List<GameObject>();

    [Header("Animation")]
    public float animationDuration = 1f;
    public float stackOffset = 5f;
    public Vector2 playerOffScreenPosition = new Vector2(400, 20);
    public Vector2 dealerOffScreenPosition = new Vector2(400, -20);

    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);  // Add this line

    private void Start()
    {
        blackjackGame = GetComponent<BlackjackGame>();

        SetupButtonListeners();
        ShowMainMenu();
        SetupRulesText();
        SelectCardBack(1);
    }

    private void SetupButtonListeners()
    {
        startGameButton.onClick.AddListener(StartGame);
        chooseCardBackButton.onClick.AddListener(OpenCardBackPanel);
        rulesButton.onClick.AddListener(OpenRulesPanel);
        hitButton.onClick.AddListener(OnHitButtonClicked);
        standButton.onClick.AddListener(OnStandButtonClicked);
        returnToMenuButton.onClick.AddListener(ReturnToMainMenu);
        cardBackCloseButton.onClick.AddListener(CloseCardBackPanel);
        rulesCloseButton.onClick.AddListener(CloseRulesPanel);
    }

    public void SelectCardBack(int index)
    {
        selectedCardBackIndex = index;
        Debug.Log("Selected card back: " + selectedCardBackIndex);
        UpdateCardBackSelection();
        blackjackGame.SetCardBackSprite(cardBackSprites[selectedCardBackIndex]);
        
    }

    public void TestButtonClick(int index){
        Debug.Log("Button clicked: " + index);
    }

    private void UpdateCardBackSelection()
    {
        for (int i = 0; i < cardBackButtons.Length; i++)
        {
            cardBackButtons[i].interactable = i != selectedCardBackIndex;
        }
    }

    private void ShowMainMenu()
    {
        ShowPanel(mainMenuPanel);
        HidePanel(gamePanel);
        HidePanel(cardBackPanel);
        HidePanel(rulesPanel);
    }

    private void SetupRulesText()
    {
        rulesText.text = "Blackjack Rules:\n\n" +
            "1. The goal is to get as close to 21 points as possible without going over.\n" +
            "2. Face cards are worth 10, Aces are 1 or 11, other cards are face value.\n" +
            "3. Click 'Hit' to draw another card, or 'Stay' to keep your current hand.\n" +
            "4. If you go over 21, you 'bust' and lose.\n" +
            "5. After you stay, the dealer plays. They must hit on 16 and stay on 17.\n" +
            "6. Closest to 21 without busting wins!";
    }

    public void RestartGame()
    {
        StartCoroutine(AnimateAllCardsOffScreen());


    }
    private void StartGame()
    {
        ShowPanel(gamePanel);
        HidePanel(mainMenuPanel);
        ClearCards();
        restartButton.interactable = false;
        bool gameOverImmediately = blackjackGame.StartNewGame();
        UpdateGameUI();
        if (gameOverImmediately)
        {
            EndGame();
        }
    }

    private void OnHitButtonClicked()
    {
        blackjackGame.PlayerHit();
        UpdateGameUI();
        if (!blackjackGame.CanPlayerHit() && !blackjackGame.CanPlayerStand())
        {
            EndGame();
        }
    }

    private void OnStandButtonClicked()
    {
        blackjackGame.PlayerStand();
        UpdateGameUI();
        EndGame();
    }

    private void ReturnToMainMenu()
    {
        ShowMainMenu();
        ClearCards();
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
        UpdatePlayerHand();
        UpdateDealerHand();
        gameResultText.text = blackjackGame.GetGameResult();

        hitButton.interactable = blackjackGame.CanPlayerHit();
        standButton.interactable = blackjackGame.CanPlayerStand();
    }

    private void UpdatePlayerHand()
    {
        List<Sprite> playerSprites = blackjackGame.GetPlayerHandSprites();
        UpdateHandDisplay(playerHandArea, playerCardObjects, playerSprites);
    }

    private void UpdateDealerHand()
    {
        bool revealHidden = !blackjackGame.CanPlayerHit() && !blackjackGame.CanPlayerStand();
        List<Sprite> dealerSprites = blackjackGame.GetDealerHandSprites(revealHidden);
        UpdateHandDisplay(dealerHandArea, dealerCardObjects, dealerSprites);
    }

    private void UpdateHandDisplay(Transform area, List<GameObject> cardObjects, List<Sprite> sprites)
    {
        // Remove excess card objects
        while (cardObjects.Count > sprites.Count)
        {
            GameObject cardToRemove = cardObjects[cardObjects.Count - 1];
            cardObjects.RemoveAt(cardObjects.Count - 1);
            Destroy(cardToRemove);
        }

        // Update existing card objects and add new ones if needed
        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject cardObject;
            if (i < cardObjects.Count)
            {
                cardObject = cardObjects[i];
            }
            else
            {
                cardObject = Instantiate(cardPrefab, area);
                cardObjects.Add(cardObject);
            }

            // Update sprite
            cardObject.GetComponent<Image>().sprite = sprites[i];

            // Position the card
            RectTransform rectTransform = cardObject.GetComponent<RectTransform>();
            float xPos = i * cardSpacing;
            float yPos = 0;

            // Apply offset to dealer's hidden card
            if (area == dealerHandArea && i == 1 && !blackjackGame.CanPlayerHit())
            {
                yPos = dealerCardOffset;
            }

            rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }
    }


    private void ClearCards()
    {
        ClearCardObjects(playerCardObjects);
        ClearCardObjects(dealerCardObjects);
    }

    private void ClearCardObjects(List<GameObject> cardObjects)
    {
        foreach (GameObject card in cardObjects)
        {
            Destroy(card);
        }
        cardObjects.Clear();
    }

    private void EndGame()
    {
        hitButton.interactable = false;
        standButton.interactable = false;
        restartButton.interactable = true;
        returnToMenuButton.gameObject.SetActive(true);
    }


    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    private IEnumerator<Coroutine> AnimateAllCardsOffScreen()
    {   

        yield return StartCoroutine(AnimateCardsOffScreen(playerCardObjects, playerOffScreenPosition));
        
        Debug.Log("Player animation completed");

        yield return StartCoroutine(AnimateCardsOffScreen(dealerCardObjects, dealerOffScreenPosition));
        Debug.Log("Dealer animation completed");

        ClearCards();

        restartButton.interactable = false;
        
        ClearCards();
        bool gameOverImmediately = blackjackGame.StartNewGame();
        UpdateGameUI();
        if (gameOverImmediately)
        {
            EndGame();
        }
    }


    private IEnumerator<Coroutine> AnimateCardsOffScreen(List<GameObject> cardObjects, Vector2 offScreenPosition)
    {
        List<RectTransform> cards = GetCardRectTransforms(cardObjects);
        Vector2 stackPosition = offScreenPosition;
        float elapsedTime = 0f;

        // Store initial positions
        Vector2[] startPositions = new Vector2[cards.Count];
        Vector2[] endPositions = new Vector2[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            startPositions[i] = cards[i].anchoredPosition;
            endPositions[i] = stackPosition + new Vector2(i * stackOffset, 0f);
        }

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            float easedT = easeCurve.Evaluate(t);  // Use the ease curve

            for (int i = 0; i < cards.Count; i++)
            {
                RectTransform card = cards[i];
                card.anchoredPosition = Vector2.Lerp(startPositions[i], endPositions[i], easedT);
            }
            yield return null;
        }

        // Ensure all cards are at their final positions
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].anchoredPosition = endPositions[i];
        }

        yield return null;
    }


    private List<RectTransform> GetCardRectTransforms(List<GameObject> cardObjects)
    {
        return cardObjects.ConvertAll(card => card.GetComponent<RectTransform>());
    }
}