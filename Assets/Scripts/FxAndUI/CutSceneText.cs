using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class CutSceneText : MonoBehaviour
{   //cutscene szöveg megjelenítéséért felelõs script
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

        if(GameStateManager.Instance.gameState == GameStateManager.GameState.NewGame) //ha newgame akkor azokat a szövegeket tölti be
        {
            dialogueTexts = XmlLoader.Instance.GetStoryText(XmlLoader.TextID.backstory);
            AudioManager.Instance.PlayMusic(AudioManager.Music_enum.INTRO_THEME);
        }
        if (GameStateManager.Instance.gameState == GameStateManager.GameState.Ending) //endingnél más szöveget tölt be
        {
            dialogueTexts = XmlLoader.Instance.GetStoryText(XmlLoader.TextID.ending);
            AudioManager.Instance.PlayMusic(AudioManager.Music_enum.ENDING_THEME);
        }
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

            switch (typingText) {
                case false: //elkezdi az írás animációt
                    if (currentTextCount == dialogueBoxes.Count)
                    {
                        ResetBoxes();
                        currentTextCount = 0;
                    }
                    StartCoroutine(TypeText(dialogueTexts[i], dialogueBoxes[currentTextCount]));
                    currentTextCount++;
                    i++;

                    break;
                case true: //ha épp megy az írás animáció akkor azt befejezi
                    typingText = false;
                    StopAllCoroutines();
                    dialogueBoxes[currentTextCount - 1].text = dialogueTexts[i - 1];
                    break;
            }

        }
        else if (typingText)
        {
            typingText = false;
            StopAllCoroutines();
            dialogueBoxes[currentTextCount - 1].text = dialogueTexts[i - 1];
        }
        else { //ha elfogytak a szövegek akkor betölti a következõ scene-t
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

    //írás animáció
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

    //lényegében törli a képernõn kiírt szöveget
    public void ResetBoxes()
    {
        for(int i = 0; i < dialogueBoxes.Count; i++)
        {
            dialogueBoxes[i].text = "";
        }
    }
}
