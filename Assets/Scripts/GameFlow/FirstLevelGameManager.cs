using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FirstLevelGameManager : MonoBehaviour
{
    public static FirstLevelGameManager Instance { get; private set; }
    [SerializeField] GameObject introControls;
    [SerializeField] GameObject introStory;
    [SerializeField] GameObject endingStory;
    [SerializeField] float introTime = 5f;

    [SerializeField] int score = 0;
    [SerializeField] int pointsForRightAnswer = 10;
    [SerializeField] int pointsForWrongAnswer = 5;
    public bool wasFirstQuestSolved = false;
    [SerializeField] DoorController doorController;
    [SerializeField] int collectedTrash = 0;
    [SerializeField] int numberOfTrashToSpawn = 1;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameStateHandler.Instance.GameState = GameState.introduction;
        introControls.SetActive(true);
        endingStory.SetActive(false);
        introStory.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameStateHandler.Instance.GameState == GameState.introduction)
        {
            introControls.SetActive(false);
            StartCoroutine(ShowIntroStory());
        }


    }

    IEnumerator ShowIntroStory()
    {
        introStory.SetActive(true);
        yield return new WaitForSeconds(introTime);
        GameStateHandler.Instance.GameState = GameState.game;
        introStory.SetActive(false);
    }

    public void UpdateScrore(bool isRight)
    {
        if (isRight)
        {
            score += pointsForRightAnswer;          
        }
        else
        {
            score -= pointsForWrongAnswer;
        }
        ScoreManager.Instance.UpdateUIPoints(score, isRight);


    }

    public void OnSolvedFirstQuest()
    {
        wasFirstQuestSolved = true;
        if (doorController == null) return;
        doorController.OpenDoor();
    }

    public void EndFirstLevel()
    {
        endingStory.SetActive(true);
        // show tree as achievment 
        GameStateHandler.Instance.GameState = GameState.endOfFirstQuest;
        StartCoroutine(ShowEndScene());
    }

    IEnumerator ShowEndScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        yield return new WaitForSeconds(introTime);
        SceneManager.LoadScene(nextSceneIndex);

    }

    public void UpdateCollectedTrash()
    {
        collectedTrash++;
        if (collectedTrash >= numberOfTrashToSpawn)
        {     
            OnSolvedFirstQuest();
        }
    }

    public int  GetNumberOfTrashToSpawn()
    {
        return numberOfTrashToSpawn;
    }
}
