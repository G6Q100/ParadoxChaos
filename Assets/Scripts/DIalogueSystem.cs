using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DIalogueSystem : MonoBehaviour
{
    public Text dialogue;
    public string[] lines;
    public float textspeed;
    private int index;
    [SerializeField] GameObject dialoguePanel;
    private float delay = 1;
    RectTransform you, You_;
    Vector3 youScale, You_Scale;

    [SerializeField] AudioSource click;
    private void OnEnable()
    {
        delay = 0.8f;
        foreach (Transform child in dialoguePanel.transform)
        {
            if (child.name == "You")
            {
                you = child.GetComponent<RectTransform>();
            }
            else if (child.name == "You(?)")
            {
                You_ = child.GetComponent<RectTransform>();
            }
        }

        dialogue.text = string.Empty;
        youScale = you.localScale;
        You_Scale = You_.localScale;
        StartDialogue();
    }

    private void Update()
    {
        if (delay > 0)
            delay -= Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && delay <= 0)
        {
            if(dialogue.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogue.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    public void Skip()
    {
        dialoguePanel.SetActive(false);
    }

    void NextLine()
    {
        click.Play();
        if (index < lines.Length - 1)
        {
            index++;
            dialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }

    IEnumerator TypeLine()
    {
        if (lines[index].Contains("You(?)") || lines[index].Contains("???"))
        {
            you.localScale = Vector3.zero;
            You_.localScale = You_Scale;
        }
        else
        {
            you.localScale = youScale;
            You_.localScale = Vector3.zero;
        }

        foreach (char c in lines[index].ToCharArray())
        {
            dialogue.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }
}
