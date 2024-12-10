using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class CutSceneText : MonoBehaviour
{
    private float textDisplaySpeed = 0.04f;
    [SerializeField]private List<TextMeshProUGUI> dialogueBoxes = new List<TextMeshProUGUI>();
    private float timeBeforeDialogueDisplayStart = 2f;
    private List<string> dialogueTexts = new List<string>();

    public bool displayingText = false;
    private int currentTextCount = 0;
    private int i;
    private bool typingText = false;

    [SerializeField] private Texture2D defaultCursorTexture;
    private Vector2 cursorPosition;
    private void Start()
    {
        Time.timeScale = 1f;
        ResetBoxes();
        dialogueTexts.Clear();
        currentTextCount = 0;
        i = 0;
        displayingText = false;
        cursorPosition = new Vector2(0, 0);
        Cursor.SetCursor(defaultCursorTexture, cursorPosition, CursorMode.Auto);
        Debug.Log(GameStateManager.Instance.gameState.ToString());
        if(GameStateManager.Instance.gameState == GameStateManager.GameState.NewGame)
        {
            dialogueTexts = XmlLoader.Instance.GetStoryText(XmlLoader.TextID.backstory);
        }
        if (GameStateManager.Instance.gameState == GameStateManager.GameState.Ending)
        {
            dialogueTexts = XmlLoader.Instance.GetStoryText(XmlLoader.TextID.ending);
        }
        Debug.Log(dialogueTexts.Count);
        StartCoroutine(WaitThenDisplayDialogues(timeBeforeDialogueDisplayStart));
    }

    private void Update()
    {
        if (displayingText)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextText();
            }
        }
    }

    private IEnumerator WaitThenDisplayDialogues(float timeBeforeDialogueDisplay)
    {
        yield return new WaitForSeconds(timeBeforeDialogueDisplay);
        if (dialogueTexts.Count > 0)
        {
            displayingText = true;
        }
        NextText();
    }

    public void NextText()
    {
        if (i < dialogueTexts.Count)
        {
            displayingText = true;
            if (typingText) { 
                typingText = false;
                StopAllCoroutines();
                dialogueBoxes[currentTextCount-1].text = dialogueTexts[i-1];
            }
            if (currentTextCount == dialogueBoxes.Count) { ResetBoxes(); currentTextCount = 0; }
            StartCoroutine(TypeText(dialogueTexts[i], dialogueBoxes[currentTextCount]));
            currentTextCount++;
            i++;
        }
        else
        {
            displayingText = false;
            if (GameStateManager.Instance.gameState == GameStateManager.GameState.NewGame)
            {
                GameStateManager.Instance.gameState = GameStateManager.GameState.Game;
                Loader.LoadScene(Loader.Scene.GameScene);
            }
            if (GameStateManager.Instance.gameState == GameStateManager.GameState.Ending)
            {
                GameStateManager.Instance.gameState = GameStateManager.GameState.Menu;
                Loader.LoadScene(Loader.Scene.MenuScene);
            }
        }
    }

    private IEnumerator TypeText(string text, TextMeshProUGUI box)
    {
        typingText = true;
        box.text = "";
        foreach (char letter in text.ToCharArray())
        {
            box.text += letter;
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.TYPING);
            yield return new WaitForSecondsRealtime(textDisplaySpeed);
        }
        typingText = false;
    }
    public void ResetBoxes()
    {
        for(int i = 0; i < dialogueBoxes.Count; i++)
        {
            dialogueBoxes[i].text = "";
        }
    }
}
