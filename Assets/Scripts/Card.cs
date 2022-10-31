using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

internal enum AnimationDirection
{
    Forward,
    Backward
}

public class Card : MonoBehaviour, IPointerClickHandler
{
    public iTween.EaseType easeType;
    public float rotationAnimationTime;
    private int _col;
    private GameManager _gameManager;
    private bool _hasBeenSelected;
    private int _row;
    private TMP_Text _text;
    private int _value = -1;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _text = GetComponentInChildren<TMP_Text>();

        _text.text = _gameManager.isDebug ? _value + "" : "";
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!_hasBeenSelected && _gameManager.CanSelectNewCard())
        {
            FlipCard(AnimationDirection.Forward);

            _text.text = _value + " ";
            _text.text = _gameManager.isDebug ? _value + "" : "";

            _gameManager.SelectCard(this);
            _hasBeenSelected = true;
        }
    }

    private void FlipCard(AnimationDirection direction)
    {
        if (direction == AnimationDirection.Forward)
        {
            iTween.MoveBy(
                gameObject,
                iTween.Hash(
                    "space",
                    Space.World,
                    "y",
                    3.0f,
                    "time",
                    rotationAnimationTime / 3,
                    "easeType",
                    easeType,
                    "axis",
                    "y"
                )
            );

            iTween.RotateTo(
                gameObject,
                iTween.Hash(
                    "z",
                    0.0f,
                    "time",
                    rotationAnimationTime,
                    "easetype",
                    easeType
                )
            );
        }
        else
        {
            iTween.RotateTo(
                gameObject,
                iTween.Hash(
                    "z",
                    180.0f,
                    "time",
                    rotationAnimationTime / 2,
                    "easetype",
                    easeType
                )
            );
            
            iTween.MoveBy(
                gameObject,
                iTween.Hash(
                    "space",
                    Space.World,
                    "y",
                    -3.0f,
                    "time",
                    rotationAnimationTime,
                    "easeType",
                    easeType,
                    "axis",
                    "z"
                )
            );
        }
    }

    public void SetValue(int value)
    {
        _value = value;
    }

    public int GetValue()
    {
        return _value;
    }

    public void SetCoordinates(int col, int row)
    {
        _col = col;
        _row = row;
    }

    public int GetRow()
    {
        return _row;
    }

    public int GetColumn()
    {
        return _col;
    }

    public void Deselect()
    {
        _hasBeenSelected = false;
        FlipCard(AnimationDirection.Backward);
        _text.text = _gameManager.isDebug ? _value + "" : "";
    }
}