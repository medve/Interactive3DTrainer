using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextControllerImpl : MonoBehaviour, TextController
{
    private FontStyle _textStyle;
    private Transform _transform = null;
    private Text _UIText = null;
    private string _text = ""; 

    void Start () {
        _UIText = GetComponent<Text> ();
        _transform = GetComponent<Transform> ();
        _text = _UIText.text;
    }

    public void SetText (string inputText) {
        _text = inputText;
        //textObject.Text = _text;
    }

    public void SetTextPosition (Vector3 position) {
        _transform.position = position;
        //textObject

    }

    public void SetTextStyle (FontStyle fontStyle) {
    //!!Заглушка
    //TODO:реализовать смену стиля   
        //_textStyle = fontStyle;
        //textPrefab.fontStyle = _textStyle;
    }

    public void HideText () {
        _UIText.text = "";
    }

    public void ShowText () {
        _UIText.text = _text;
    }
}

