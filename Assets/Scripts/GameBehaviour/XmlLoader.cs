using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XmlLoader : MonoBehaviour
{
    public static XmlLoader Instance { get; private set; }

    XmlDocument storyData;

    public class DialogueData
    {
        public string name;
        public string dialogueText;
    }

    public enum OnStage
    {
        start,
        end
    }

    public enum TextID
    {
        backstory,
        ending
    }

    private void Awake()
    {
        Instance = this;

        //xml file bet�lt�se
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/story");
        storyData = new XmlDocument();
        storyData.LoadXml(xmlTextAsset.text);
    }

    //az xml fileb�l kiszedi a t�rt�netet att�l f�gg�en hogy h�tt�rt�rt�net vagy pedig ending
    public List<string> GetStoryText(TextID textID)
    {
        List<string> list = new List<string>();

        XmlNodeList sections = storyData.SelectNodes("/story/"+ textID.ToString() +"/section");

        foreach (XmlNode section in sections)
        {
            list.Add(section.InnerText);
        }
        return list;
    }

    //az xml fileb�l kiszedi a dial�gus adatait att�l f�gg�en hogy melyik stage dial�gusa �s a stage elej�n vagy v�g�n j�tsz�dik
    public List<DialogueData> GetDialogueData(int stage, OnStage startend)
    {
        List<DialogueData> dialogueDatas = new List<DialogueData>();

        XmlNodeList dialogues = storyData.SelectNodes("/story/stages/stage[@id='" + stage.ToString() + "']/" + startend.ToString() + "/dialogue");

        int i = 0;
        foreach (XmlNode dialogue in dialogues)
        {
            DialogueData dialogueData = new DialogueData();
            i++;
            dialogueData.name = dialogue["name"].InnerText;
            dialogueData.dialogueText = dialogue["text"].InnerText;
            dialogueDatas.Add(dialogueData);
        }
        return dialogueDatas;
    }
}
