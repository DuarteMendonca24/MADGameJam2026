using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardManager : MonoBehaviour
{
    [SerializeField] LettersOrderManager lettersOrderManager;

    private int gameLevel;

    private string keys = "QWERTYUIOPASDFGHJKLÇZXCVBNM1234567890_.`´+?";
    private void Start()
    {
        gameLevel = GameManager.Instance.GetgameLevel();
        SetRandomKeys();
    }

    private void SetRandomKeys()
    {
        string setOfKeys = CreateSetOfKeys();
        foreach(Transform rows in transform)
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
                
            }
        }
    }

    private string CreateSetOfKeys() 
    {
        string setOfKeys = keys;   
        for (int i = setOfKeys.Length - 1; i > 0 ; i--) 
        {
            if (lettersOrderManager.GetWordsByLevel()[gameLevel - 1].Contains(setOfKeys[i]))
            {
                setOfKeys = setOfKeys.Remove(i, 1);
            }
        }

        return setOfKeys;
    }
}
