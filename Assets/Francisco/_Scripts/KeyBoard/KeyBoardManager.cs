using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;


public class KeyBoardManager : MonoBehaviour
{
    [SerializeField] LettersOrderManager lettersOrderManager;

    private int gameLevel;

    private string keys = "QWERTYUIOPASDFGHJKLÇZXCVBNM1234567890_.`´+?";

    private List<BaseKey> baseKeys = new List<BaseKey>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                BaseKey baseKey = grandChild.GetComponent<BaseKey>();
                if (baseKey != null)
                {
                    baseKeys.Add(baseKey);
                }
            }
        }
    }

    private void Start()
    {
        gameLevel = GameManager.Instance.GetgameLevel();
        SetRandomKeys();
    }

    private void SetRandomKeys()
    {
        string setOfKeys = CreateSetOfKeys();
        Debug.Log(setOfKeys.Length);
        foreach (Transform rows in transform)
        {
            foreach (Transform key in rows)
            {
                int chosedIndex = Random.Range(0, setOfKeys.Length);

                if (key.gameObject.GetComponent<BaseKey>().GetKeyID() == "")
                {
                    string letter = setOfKeys[chosedIndex].ToString();
                    key.gameObject.GetComponent<BaseKey>().SetKeyID(letter);
                    key.GetChild(0).Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = letter;
                    setOfKeys = setOfKeys.Remove(chosedIndex, 1);
                }

                else { key.GetChild(0).Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = key.gameObject.GetComponent<BaseKey>().GetKeyID(); }


                Debug.Log(setOfKeys);
            }
        }
    }

    private string CreateSetOfKeys()
    {
        string setOfKeys = keys;
        Debug.Log(setOfKeys);
        for (int i = setOfKeys.Length - 1; i > 0; i--)
        {
            if (lettersOrderManager.GetWordsByLevel()[gameLevel - 1].Contains(setOfKeys[i]))
            {
                Debug.Log(setOfKeys[i]);
                setOfKeys = setOfKeys.Remove(i, 1);
            }
        }

        return setOfKeys;
    }

    public void ResetKeyPositions()
    {
        foreach (BaseKey key in baseKeys)
        {
            key.ResetPosition();
        }
    }
}
