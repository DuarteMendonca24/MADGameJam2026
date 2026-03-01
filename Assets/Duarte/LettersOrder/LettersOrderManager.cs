using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LettersOrderManager : MonoBehaviour
{
    public delegate void WordCompleted();
    public WordCompleted wordCompleted;

    // The final words the player wants to find
    [SerializeField] List<string> finalWords = new List<string>();
    [SerializeField] TextMeshProUGUI displayText;

    // Final words mapped by each level
    Dictionary<int, string> wordsByLevel = new Dictionary<int, string>();


    public void Initialize()
    {
        print("Initialize?");
        for (int i = 0; i < finalWords.Count; i++)
        {
            wordsByLevel.Add(i + 1, finalWords[i]);
        }

        ShowRandomLetter(1);

    }


    // Displays a random character from the word corresponding to the level
    // Removes the character from the string to prevent displaying it again
    public void ShowRandomLetter(int level)
    {
        if (wordsByLevel[level] == "")
        {
            wordCompleted?.Invoke();
            return;
        }
        int randomIndex = Random.Range(0, wordsByLevel[level].Length);
        displayText.text = wordsByLevel[level][randomIndex].ToString();
        wordsByLevel[level] = wordsByLevel[level].Remove(randomIndex, 1);

        
    }

    public List<string> GetWordsByLevel() { return finalWords; }

    public string GetDisplayedLetter() { return displayText.text; }
}
