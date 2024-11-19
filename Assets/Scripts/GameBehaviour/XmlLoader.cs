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

        //xml file betöltése
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/story");
        storyData = new XmlDocument();
        storyData.LoadXml(xmlTextAsset.text);
    }

    //az xml fileból kiszedi a történetet attól függõen hogy háttértörténet vagy pedig ending
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

    //az xml fileból kiszedi a dialógus adatait attól függõen hogy melyik stage dialógusa és a stage elején vagy végén játszódik
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
