using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BlackjackGame : MonoBehaviour
{
    public enum CardSuit { C, D, H, P }
    public enum CardValue { A = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, J = 11, Q = 12, K = 13 }

    public class Card
    {
        public CardSuit Suit { get; private set; }
        public CardValue Value { get; private set; }
        public Sprite Sprite { get; private set; }

        public Card(CardSuit suit, CardValue value, Sprite sprite)
        {
            Suit = suit;
            Value = value;
            Sprite = sprite;
        }

        public override string ToString()
        {
            return $"{GetValueString()}-{Suit}";
        }

        private string GetValueString()
        {
            switch (Value)
            {
                case CardValue.A: return "A";
                case CardValue.J: return "J";
                case CardValue.Q: return "Q";
                case CardValue.K: return "K";
                default: return ((int)Value).ToString();
            }
        }
    }

    private Dictionary<string, Sprite> cardSprites;
    public Sprite cardBackSprite;

    private List<Card> deck;
    private List<Card> playerHand;
    private List<Card> dealerHand;
    private string gameResult;
    private bool gameOver;

    private void Awake()
    {
        LoadCardSprites();
    }

        public bool StartNewGame()
    {
        InitializeGame();
        gameOver = false;
        gameResult = "";
        
        if(CalculateHandValue(playerHand) == 21 && CalculateHandValue(dealerHand) == 21){
            gameResult = "Both players have blackjack! It's a push.";
            gameOver = true;
        }
        else if(CalculateHandValue(playerHand) == 21){
            gameResult = "Player blackjack! Player wins.";
            gameOver = true;
        } else if(CalculateHandValue(dealerHand) == 21){
            gameResult = "Dealer blackjack! Dealer wins.";
            gameOver = true;
        }

        return gameOver;
    }

        private void InitializeGame()
    {
        CreateDeck();
        ShuffleDeck();
        DealInitialCards();
    }


    private void LoadCardSprites()
    {
        cardSprites = new Dictionary<string, Sprite>();
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("CardImages");
        
        foreach (Sprite sprite in loadedSprites)
        {
            cardSprites[sprite.name] = sprite;
            Debug.Log($"Loaded sprite: {sprite.name}");
        }

        if (cardSprites.Count == 0)
        {
            Debug.LogError("No sprites were loaded. Check that your images are in Assets/Resources/CardImages and are set as Sprites in Unity.");
        }

        if (!cardSprites.ContainsKey("BACK-R"))
        {
            Debug.LogError("Card back sprite not found. Ensure you have a sprite named 'BACK' in your CardImages folder.");
        }
        else
        {
            cardBackSprite = cardSprites["BACK-R"];
        }
    }

    private void CreateDeck()
    {
        deck = new List<Card>();
        foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardValue value in System.Enum.GetValues(typeof(CardValue)))
            {
                string spriteName = $"{GetSpriteValueString(value)}-{suit}";
                if (cardSprites.TryGetValue(spriteName, out Sprite sprite))
                {
                    deck.Add(new Card(suit, value, sprite));
                }
                else
                {
                    Debug.LogError($"Sprite not found for card: {spriteName}");
                }
            }
        }
    }

    private string GetSpriteValueString(CardValue value)
    {
        switch (value)
        {
            case CardValue.A: return "A";
            case CardValue.J: return "J";
            case CardValue.Q: return "Q";
            case CardValue.K: return "K";
            default: return ((int)value).ToString();
        }
    }


    private void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    private Card DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.LogError("No more cards in the deck!");
            return null;
        }

        Card drawnCard = deck[0];
        deck.RemoveAt(0);
        return drawnCard;
    }

    private void DealInitialCards()
    {
        playerHand = new List<Card>();
        dealerHand = new List<Card>();

        playerHand.Add(DrawCard());
        dealerHand.Add(DrawCard());
        playerHand.Add(DrawCard());
        dealerHand.Add(DrawCard());
    }

    public void PlayerHit()
    {
        if (gameOver) return;

        playerHand.Add(DrawCard());
        if (CalculateHandValue(playerHand) > 21)
        {
            gameResult = "Player busts! Dealer wins.";
            gameOver = true;
        }
    }

    public void PlayerStand()
    {
        if (gameOver) return;

        PlayDealerTurn();
        DetermineWinner();
        gameOver = true;
    }

    private void PlayDealerTurn()
    {
        while (CalculateHandValue(dealerHand) < 17)
        {
            dealerHand.Add(DrawCard());
        }
    }

    private void DetermineWinner()
    {
        int playerValue = CalculateHandValue(playerHand);
        int dealerValue = CalculateHandValue(dealerHand);

        if (playerValue > 21)
        {
            gameResult = "Player busts! Dealer wins.";
        }
        else if (dealerValue > 21)
        {
            gameResult = "Dealer busts! Player wins.";
        }
        else if (playerValue > dealerValue)
        {
            gameResult = "Player wins!";
        }
        else if (dealerValue > playerValue)
        {
            gameResult = "Dealer wins.";
        }
        else
        {
            gameResult = "It's a tie!";
        }
    }
    private int CalculateHandValue(List<Card> hand)
    {
        int value = 0;
        int aceCount = 0;

        foreach (Card card in hand)
        {
            if (card.Value == CardValue.A)
            {
                aceCount++;
                value += 11;
            }
            else if (card.Value >= CardValue.J)
            {
                value += 10;
            }
            else
            {
                value += (int)card.Value;
            }
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10;
            aceCount--;
        }

        return value;
    }

    public List<Sprite> GetPlayerHandSprites()
    {
        return playerHand.ConvertAll(card => card.Sprite);
    }

    public List<Sprite> GetDealerHandSprites(bool revealHidden = false)
    {
        if (!revealHidden && !gameOver)
        {
            List<Sprite> sprites = new List<Sprite> { dealerHand[0].Sprite, cardBackSprite };
            sprites.AddRange(dealerHand.GetRange(2, dealerHand.Count - 2).ConvertAll(card => card.Sprite));
            return sprites;
        }
        return dealerHand.ConvertAll(card => card.Sprite);
    }

    public string GetPlayerHandAsString()
    {
        return GetHandAsString(playerHand);
    }

    public string GetDealerHandAsString()
    {
        if (!gameOver)
        {
            return dealerHand[0].ToString() + ", Hidden";
        }
        return GetHandAsString(dealerHand);
    }

    private string GetHandAsString(List<Card> hand)
    {
        return string.Join(", ", hand);
    }   

    public string GetGameResult()
    {
        return gameResult;
    }

    public bool CanPlayerHit()
    {
        return !gameOver && CalculateHandValue(playerHand) <= 21;
    }

    public bool CanPlayerStand()
    {
        return !gameOver;
    }
}