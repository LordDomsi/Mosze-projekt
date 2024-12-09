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
    private float textDisplaySpeed = 0.02f;

    public bool displayingText = false;

    private int i;

    [SerializeField] private float cutSceneDelay = 3f;

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
            dialogueBox.gameObject.SetActive(true);
            displayingText = true;
            PlayerMovement.Instance.canShoot = false;
            Time.timeScale = 0; //megállít mindent ami deltatime-ot használ (lényegében mindent)
        }
        NextText();
    }

    public void NextText()
    {
        if (i<dialogues.Count)
        {
            //a beszélõ neve alapján változtatja meg az avatár képét
            string pathToImage = "Images/" + dialogues[i].name;
            avatar.sprite = Resources.Load<Sprite>(pathToImage);
            nameText.SetText(dialogues[i].name); //a beszélõnek a nevét átállítja
            StopAllCoroutines();
            StartCoroutine(TypeText(dialogues[i].dialogueText)); // coroutine animálja a kiírást
            i++;
        }
        else
        {
            //ha elfogytak a dialógusok akkor eltûnik a ui és tovább megy a game
            dialogues.Clear();
            displayingText=false;
            dialogueBox.gameObject.SetActive(false);
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
        //átalakítja a dialógust char tömbbé és egyesével kiírja delayel
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textDisplaySpeed); //sima WaitForSeconds nem jó mert azt megállítja a timeScale = 0
        }
    }

    private IEnumerator WaitThenEndScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameStateManager.Instance.gameState = GameStateManager.GameState.Ending;
        Loader.LoadScene(Loader.Scene.CutScene);
    }
}
