using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxUI : MonoBehaviour
{
    public static DialogueBoxUI Instance {  get; private set; }

    private float timeBeforeDialogueDisplayStart = 1.5f;
    private float timeBeforeDialogueDisplayEnd = 3f;
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
    }
    private void Update()
    {   //space-el tov�bb lehet nyomni a dial�gust
        if (displayingText)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                NextText();
            }
        }
    }
    //akkor jelenik meg dial�gus amikor felvessz�k a p�lya v�g�n az alkatr�szt
    private void LocatorSpawner_OnLocatorPickup(object sender, System.EventArgs e)
    {
        i = 0;
        StartCoroutine(WaitThenDisplayDialogues(XmlLoader.OnStage.end, timeBeforeDialogueDisplayEnd));
    }
    //akkor is jelenik meg dial�gus amikor egy p�lya bet�lt
    private void StageManager_OnStageInit(object sender, System.EventArgs e)
    {
        i = 0;
        StartCoroutine(WaitThenDisplayDialogues(XmlLoader.OnStage.start, timeBeforeDialogueDisplayStart));
    }

    private IEnumerator WaitThenDisplayDialogues(XmlLoader.OnStage onStage, float timeBeforeDialogueDisplay)
    {
        yield return new WaitForSeconds(timeBeforeDialogueDisplay); // v�r egy keveset hogy ne instant legyen amikor p�ly�t v�ltunk
        dialogues = XmlLoader.Instance.GetDialogueData(StageManager.Instance.currentStage, onStage); //xml loader a sz�ks�ges dial�gussal felt�lti a list�t

        if (dialogues.Count > 0)
        {
            //ha van dial�gus akkor megjelenik a ui
            dialogueBox.gameObject.SetActive(true);
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.DIALOGUE_POPUP);
            displayingText = true;
            PlayerMovement.Instance.canShoot = false;
            Time.timeScale = 0; //meg�ll�t mindent ami deltatime-ot haszn�l (l�nyeg�ben mindent)
        }
        NextText();
    }

    public void NextText()
    {
        if (i<dialogues.Count)
        {
            //a besz�l� neve alapj�n v�ltoztatja meg az avat�r k�p�t
            string pathToImage = "Images/" + dialogues[i].name;
            avatar.sprite = Resources.Load<Sprite>(pathToImage);
            nameText.SetText(dialogues[i].name); //a besz�l�nek a nev�t �t�ll�tja
            StopAllCoroutines();
            if(i!=0)AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.NEXT_TEXT);
            StartCoroutine(TypeText(dialogues[i].dialogueText)); // coroutine anim�lja a ki�r�st
            i++;
        }
        else
        {
            //ha elfogytak a dial�gusok akkor elt�nik a ui �s tov�bb megy a game
            dialogues.Clear();
            displayingText=false;
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
        dialogueText.text = "";
        //�talak�tja a dial�gust char t�mbb� �s egyes�vel ki�rja delayel
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.TYPING);
            yield return new WaitForSecondsRealtime(textDisplaySpeed); //sima WaitForSeconds nem j� mert azt meg�ll�tja a timeScale = 0
        }
    }

    private IEnumerator WaitThenEndScene(float delay)
    {
        SaveManager.Instance.AddScoreToLeaderboard(ScoreManager.Instance.GetPlayerScore());
        OnGameEnd?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(delay);
        GameStateManager.Instance.gameState = GameStateManager.GameState.Ending;
        AudioManager.Instance.PlayMusic(AudioManager.Music_enum.ENDING_THEME);
        Loader.LoadScene(Loader.Scene.CutScene);
    }
}
