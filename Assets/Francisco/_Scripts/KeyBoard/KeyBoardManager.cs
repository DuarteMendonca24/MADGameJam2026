using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;


public class KeyBoardManager : MonoBehaviour
{
    [SerializeField] LettersOrderManager lettersOrderManager;

    private int gameLevel;

    private string keys = "QWERTYUIOPASDFGHJKL�ZXCVBNM1234567890_.`�+?";

    private List<BaseKey> baseKeys = new List<BaseKey>();

    private List<BaseKey> teleportKeys = new List<BaseKey>();
    private List<BaseKey> noneKeys = new List<BaseKey>();

    public Vector2 playerSpawnPosition = new Vector2();

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

                    switch (baseKey.GetKeyType())
                    {
                        case KeyType.Spawner:
                            playerSpawnPosition = grandChild.position;
                            break;
                        case KeyType.Teleport:
                            teleportKeys.Add(baseKey);
                            break;
                        case KeyType.None:
                            noneKeys.Add(baseKey);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        foreach (BaseKey teleportKey in teleportKeys)
        {
            int randomIndex = Random.Range(0, noneKeys.Count);
            Vector2 randomNonePosition = noneKeys[randomIndex].transform.position;
            teleportKey.SetTeleportPosition(randomNonePosition);
        }
    }

    private void Start()
    {
        gameLevel = GameManager.Instance.GetGameLevel();
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
