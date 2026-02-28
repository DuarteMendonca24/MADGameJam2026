using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LettersOrderManager : MonoBehaviour
{   
    [SerializeField]
    // The final words the player wants to find
    List<string> finalWords = new List<string>();

    [SerializeField]
    TextMeshProUGUI displayText;

    // Final words mapped by each level
    Dictionary<int, string> wordsByLevel = new Dictionary<int, string>();


    void Start()
    {
        for (int i = 0; i < finalWords.Count; i++)
        {
            wordsByLevel.Add(i + 1, finalWords[i]);
        }

        ShowRandomLetter(1);
    }

    // Displays a random character from the word corresponding to the level
    // Removes the character from the string to prevent displaying it again
    void ShowRandomLetter(int level)
    {
        int randomIndex = Random.Range(0, wordsByLevel[level].Length);
        displayText.text = wordsByLevel[level][randomIndex].ToString();
        wordsByLevel[level] = wordsByLevel[level].Remove(randomIndex);

        if (wordsByLevel[level] == "")
        {
            // TODO: Apply logic or event or variable to notify
            print("Word completed");
        }
    }
}
