using System.Collections.Generic;
using UnityEngine;

public class BlackjackGame : MonoBehaviour
{
    public enum CardSuit { Hearts, Diamonds, Clubs, Spades }
    public enum CardValue { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    public class Card
    {
        public CardSuit Suit { get; private set; }
        public CardValue Value { get; private set; }

        public Card(CardSuit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value} of {Suit}";
        }
    }

    private List<Card> deck;
    private List<Card> playerHand;
    private List<Card> dealerHand;
    private string gameResult;
    private bool gameOver;

    public bool StartNewGame()
    {
        InitializeGame();
        gameOver = false;
        gameResult = "";
        
        // Check for initial blackjack
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

    private void CreateDeck()
    {
        deck = new List<Card>();
        foreach (CardSuit suit in System.Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardValue value in System.Enum.GetValues(typeof(CardValue)))
            {
                deck.Add(new Card(suit, value));
            }
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

        playerHand.Add(new Card(CardSuit.Hearts, CardValue.Ace));
        playerHand.Add(new Card(CardSuit.Hearts, CardValue.Ten));

        Debug.Log(CalculateHandValue(playerHand));


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
            if (card.Value == CardValue.Ace)
            {
                aceCount++;
                value += 11;
            }
            else if (card.Value >= CardValue.Jack)
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