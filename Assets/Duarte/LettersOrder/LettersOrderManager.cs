using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class LettersOrderManager : MonoBehaviour
{
    public delegate void WordCompleted();
    public WordCompleted wordCompleted;

    // The final words the player wants to find
    [SerializeField] List<string> finalWords = new List<string>();
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] TextMeshProUGUI finalTextOrdered;
    [SerializeField] TextMeshProUGUI finalTextShuffle;

    [SerializeField] Dictionary<int, WordDisplay> wordDisplays = new Dictionary<int, WordDisplay>();


    public void Initialize()
    {
        for (int i = 0; i < finalWords.Count; i++)
        {
            string shuffled = StringUtils.Shuffle(finalWords[i]);
            wordDisplays.Add(i + 1, new WordDisplay(shuffled, 0));
            
            finalTextShuffle.text = finalTextShuffle.text + shuffled;
            finalTextOrdered.text = finalTextOrdered.text + finalWords[i] + " ";
           
            print("Shuffled string" + shuffled);
            print("Ordered string" + finalWords[i]);
        }

        ShowRandomLetter(1);

    }


    // Displays a random character from the word corresponding to the level
    // Removes the character from the string to prevent displaying it again
    public void ShowRandomLetter(int level)
    {
        WordDisplay wordDisplay = wordDisplays[level];
        if (wordDisplay.characterIndex > (wordDisplay.word.Length - 1))
        {
            wordCompleted?.Invoke();
            return;
        }

        displayText.text = wordDisplay.word[wordDisplay.characterIndex].ToString();
        wordDisplay.characterIndex++;
    }


    public IEnumerator ShowFinalMessage()
    {
        displayText.gameObject.SetActive(false);
        finalTextShuffle.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        finalTextShuffle.gameObject.SetActive(false);
        finalTextOrdered.gameObject.SetActive(true);
        yield return StartCoroutine(ChangeTextColor());
        StartCoroutine(ChangeTextSize());
    }


    private IEnumerator ChangeTextColor()
    {
        float tick = 0f;
        ColorUtility.TryParseHtmlString("#630000", out Color endColor);
        Color startColor = finalTextOrdered.color;
        while (finalTextOrdered.color != endColor)
        {
            tick += Time.deltaTime * 0.5f;
            finalTextOrdered.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }

    private IEnumerator ChangeTextSize()
    {
        float minSize = 24f;
        float maxSize = 29f;
        float speed = 2f; // animation speed

        float time = 0f;

        while (true)
        {
            time += Time.deltaTime * speed;

            float t = Mathf.PingPong(time, 1f);
            finalTextOrdered.fontSize = Mathf.Lerp(minSize, maxSize, t);

            yield return null;
        }
    }

    public List<string> GetWordsByLevel() { return finalWords; }

    public string GetDisplayedLetter() { return displayText.text; }

    public void ResetLetter(int level)
    {
        wordDisplays[level].characterIndex = 0;
        ShowRandomLetter(level);
    }
}


class WordDisplay
{
    public string word;
    public int characterIndex;

    public WordDisplay(string word, int characterIndex)
    {
        this.word = word;
        this.characterIndex = characterIndex;
    }
}


public static class StringUtils
{
    private static System.Random rng = new System.Random();

    public static string Shuffle(string input)
    {
        char[] chars = input.ToCharArray();

        for (int i = chars.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1); // random index [0, i]

            // swap
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars);
    }
}