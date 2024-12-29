using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    [SerializeField] TMP_InputField _nameField = null;
    [SerializeField] Button _confirmButton = null;
    [SerializeField] Vector2 _nameLengthRange = default;

    private const string PLAYER_NAME_KEY = "pref_player_name";

    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            GotoNextScene();
        }
        else
        {
            var _loadedName = PlayerPrefs.GetString(PLAYER_NAME_KEY, string.Empty);
            _nameField.text = _loadedName;
            HandleNameChanged();
        }
    }

    public void HandleNameChanged()
    {
        bool _isInsideLengthRange = _nameField.text.Length >= _nameLengthRange.x && _nameField.text.Length <= _nameLengthRange.y;
        _confirmButton.interactable = _isInsideLengthRange;
    }

    public void Confirm()
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, _nameField.text);
        GotoNextScene();
    }

    public void GotoNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
