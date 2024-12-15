using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour
{
    public static DialogueBoxUI Instance {  get; private set; }

    private float timeBeforeDialogueDisplayStart = 1f;
    private float timeBeforeDialogueDisplayEnd = 1f;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image avatar;
    private float textDisplaySpeed = 0.04f;

    public bool displayingText = false;

    public event EventHandler OnGameEnd;

    private int i;

    [SerializeField] private float cutSceneDelay = 2f;

    List<XmlLoader.DialogueData> dialogues = new List<XmlLoader.DialogueData>();

    public bool isSubscribed = false;

    private bool typing = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StageManager.Instance.OnStageInit += StageManager_OnStageInit;
        LocatorSpawner.Instance.OnLocatorPickup += LocatorSpawner_OnLocatorPickup;
        isSubscribed = true;
        dialogueBox.gameObject.SetActive(false);
        Cursor.visible = true;
    }
    private void Update()
    {   //space-el tovább lehet nyomni a dialógust
        if (displayingText)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextText();
            }
        }
    }
    //akkor jelenik meg dialógus amikor felvesszük a pálya végén az alkatrészt
    private void LocatorSpawner_OnLocatorPickup(object sender, System.EventArgs e)
    {
        i = 0;
        StartCoroutine(WaitThenDisplayDialogues(XmlLoader.OnStage.end, timeBeforeDialogueDisplayEnd));
    }
    //akkor is jelenik meg dialógus amikor egy pálya betölt
    private void StageManager_OnStageInit(object sender, System.EventArgs e)
    {
        i = 0;
        StartCoroutine(WaitThenDisplayDialogues(XmlLoader.OnStage.start, timeBeforeDialogueDisplayStart));
    }

    private IEnumerator WaitThenDisplayDialogues(XmlLoader.OnStage onStage, float timeBeforeDialogueDisplay)
    {
        yield return new WaitForSeconds(timeBeforeDialogueDisplay); // vár egy keveset hogy ne instant legyen amikor pályát váltunk
        dialogues = XmlLoader.Instance.GetDialogueData(StageManager.Instance.currentStage, onStage); //xml loader a szükséges dialógussal feltölti a listát

        if (dialogues.Count > 0)
        {
            //ha van dialógus akkor megjelenik a ui
            displayingText = true;
            dialogueBox.gameObject.SetActive(true); //dialógus megjelenítése
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.DIALOGUE_POPUP); // hangeffekt
            
            Cursor.visible = false; //eltûnik a cursor
            PlayerMovement.Instance.DisableSight();
            PlayerMovement.Instance.canShoot = false; // a player nem tud lõni

            Time.timeScale = 0; //megállít mindent ami deltatime-ot használ (lényegében mindent)
        }
        NextText();
    }

    public void NextText()
    {
        if (i<dialogues.Count)
        {
            switch (typing)
            {
                case true: //ha még megy az írás animáció akkor befejezi
                    typing = false;
                    StopAllCoroutines();
                    dialogueText.text = dialogues[i-1].dialogueText;
                    break;
                case false: // ha nem megy az írás akkor a következõ szöveget elkezdi írni
                    string pathToImage = "Images/" + dialogues[i].name; //a beszélõ neve alapján változtatja meg az avatár képét
                    avatar.sprite = Resources.Load<Sprite>(pathToImage);

                    nameText.SetText(dialogues[i].name); //a beszélõnek a nevét átállítja
                    StartCoroutine(TypeText(dialogues[i].dialogueText)); // coroutine animálja a kiírást

                    if (i != 0) AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.NEXT_TEXT); //hangeffekt
                    i++;
                    break;
            }
        }
        else if (typing)
        {
            typing = false;
            StopAllCoroutines();
            dialogueText.text = dialogues[i - 1].dialogueText;
        }
        else
        {
            //ha elfogytak a dialógusok akkor eltûnik a ui és tovább megy a game
            dialogues.Clear();
            displayingText=false;
            Cursor.visible = true;
            PlayerMovement.Instance.EnableSight();
            dialogueBox.gameObject.SetActive(false);
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.DIALOGUE_POPUP);
            Time.timeScale=1f;
            PlayerMovement.Instance.canShoot = true;
            if (LocatorSpawner.Instance.GetCurrentLocators() == 3)
            {
                StartCoroutine(WaitThenEndScene(cutSceneDelay));
            }
        }
    }

    private IEnumerator TypeText(string text)
    {
        typing = true;
        dialogueText.text = "";
        //átalakítja a dialógust char tömbbé és egyesével kiírja delayel
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.TYPING);
            yield return new WaitForSecondsRealtime(textDisplaySpeed); //sima WaitForSeconds nem jó mert azt megállítja a timeScale = 0
        }
        typing = false;
    }

    private IEnumerator WaitThenEndScene(float delay)
    {
        SaveManager.Instance.AddScoreToLeaderboard(ScoreManager.Instance.GetPlayerScore());
        OnGameEnd?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(delay);
        GameStateManager.Instance.gameState = GameStateManager.GameState.Ending;
        Loader.LoadScene(Loader.Scene.CutScene);
    }

    private void OnDestroy()
    {
        StageManager.Instance.OnStageInit -= StageManager_OnStageInit;
        LocatorSpawner.Instance.OnLocatorPickup -= LocatorSpawner_OnLocatorPickup;
    }
}
