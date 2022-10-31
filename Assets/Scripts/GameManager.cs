using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    private const int MaxRows = 3;
    private const int MaxColumns = 4;
    private const float InitialXOffset = 10.0f;

    private static readonly Random Random = new();
    public bool isDebug;
    public GameObject initialCardModel;
    private readonly List<List<GameObject>> _cards = new();

    private readonly List<Card> _selectedCards = new();

    private void Start()
    {
        var possibleValues = GetInitialValues();

        for (var column = 0; column < MaxColumns; column++)
        {
            var cardRow = new List<GameObject>();
            for (var row = 0; row < MaxRows; row++)
            {
                var card = Instantiate(
                    initialCardModel,
                    new Vector3(InitialXOffset + column * 10.0f, initialCardModel.transform.position.y, row * 12.0f),
                    initialCardModel.transform.rotation
                );

                var cellValue = possibleValues[row * MaxColumns + column];

                var model = card.GetComponent<Card>();
                model.SetCoordinates(column, row);
                model.SetValue(cellValue);

                cardRow.Add(card);
            }

            _cards.Add(cardRow);
        }

        Destroy(initialCardModel);
    }

    public void SelectCard(Card card)
    {
        _selectedCards.Add(card);

        if (_selectedCards.Count < 2)
        {
            return;
        }

        var card1 = _selectedCards[0];
        var card2 = _selectedCards[1];

        if (card1.GetValue() == card2.GetValue())
        {
            var gameObject1 = _cards[card1.GetColumn()][card1.GetRow()];
            var gameObject2 = _cards[card2.GetColumn()][card2.GetRow()];
            DelayedCardClear();
            Destroy(gameObject1, 1.0f);
            Destroy(gameObject2, 1.0f);
        }
        else
        {
            DelayedCardClear();
        }
    }

    private void DelayedCardClear()
    {
        Invoke(nameof(ClearSelections), 1.0f);
    }

    public bool CanSelectNewCard()
    {
        return _selectedCards.Count < 2;
    }

    private void ClearSelections()
    {
        foreach (var selectedCard in _selectedCards)
        {
            selectedCard.Deselect();
        }

        _selectedCards.Clear();
    }

    private static List<int> GetInitialValues()
    {
        var output = new List<int>();
        var possibleValues = new List<int>();
        possibleValues.AddRange(Enumerable.Range(1, MaxRows * MaxColumns / 2));

        for (var i = 0; i < 2; i++)
        {
            output.AddRange(possibleValues);
        }

        ShuffleList(output);
        return output;
    }

    private static void ShuffleList<T>(IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = Random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}