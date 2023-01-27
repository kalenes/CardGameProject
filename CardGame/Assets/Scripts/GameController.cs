using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    [SerializeField] private Text countG;
    [SerializeField] private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private string firstGuessPuzzle, secondGuessPuzzle;

    private int firstGuessIndex, secondGuessIndex;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Planets");
    }

    private void Start()
    {
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    private void GetButtons()
    {
        GameObject [] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;
        for (int i = 0; i < looper; i++)
        {
            if (index == looper/2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }
    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (!firstGuess)
        {
            firstGuess = true;
            
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
            btns[firstGuessIndex].interactable = false;
            
            
        }else if (!secondGuess)
        {
            secondGuess = true;
            
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            countGuesses++;
            countG.text = countGuesses.ToString();

            StartCoroutine(CheckIfThePuzzleMatch());
            
        }

        IEnumerator CheckIfThePuzzleMatch()
        {
            yield return new WaitForSeconds(.5f);
            if (firstGuessPuzzle == secondGuessPuzzle)
            {
                yield return new WaitForSeconds(.2f);

                btns[firstGuessIndex].interactable = false;
                btns[secondGuessIndex].interactable = false;

                btns[firstGuessIndex].image.color = new Color(0,0,0,0);
                btns[secondGuessIndex].image.color = new Color(0,0,0,0);
                    
                CheckIfTheGameIsFinished();
            }
            else
            {
                btns[firstGuessIndex].interactable = true;
                btns[firstGuessIndex].image.sprite = bgImage;
                btns[secondGuessIndex].image.sprite = bgImage;
            }
            yield return new WaitForSeconds(.2f);
            firstGuess = secondGuess = false;
        }
    }

    private void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;
        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game Finished");
            Debug.Log("It took you "+ countGuesses + "many guess(es) to finished the game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CloseGame()
    {
        Application.Quit();
        Debug.Log("Oyun Kapandı.");
    }
    
    
}
