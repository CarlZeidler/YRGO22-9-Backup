using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TextWriter : MonoBehaviour
{
    public TextMeshProUGUI textdisplay;
    public string[] sentances;
    public string randomTextSymbols;
    private char[] randomSymbols;
    public UnityEvent OnFinish;

    private AudioSource src;

    private int index;
    [SerializeField]private float typingSpeed = 0.03f;
    private bool canContinue;
    private bool lastSentance = false;

    private void Start()
    {
        randomSymbols = randomTextSymbols.ToCharArray();
        src = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    private void Update()
    {
        if (textdisplay.text == sentances[index])
            canContinue = true;

        if (Input.anyKey && canContinue && !lastSentance)
            NextSentance();
        else if (Input.anyKey && canContinue && lastSentance)
        {
            OnFinish.Invoke();
        }

        if (sentances.Length - 1 == index)
            lastSentance = true;
    }

    IEnumerator Type()
    {
        int i = 0;
        foreach(char letter in sentances[index].ToCharArray())
        {
            char randomChar = randomSymbols[Random.Range(0, randomSymbols.Length)];
            textdisplay.text += randomChar;
            //src.Play();
            yield return new WaitForSeconds(typingSpeed);
            char[] newchars = textdisplay.text.ToCharArray();
            newchars[i] = letter;
            textdisplay.text = new string (newchars);

            i++;
        }
        canContinue = true;
    }

    private void NextSentance()
    {
        canContinue = false;

        if (index<sentances.Length-1)
        {
            index++;
            textdisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textdisplay.text = "";
        }
    }
}
