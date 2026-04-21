using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [Header("Win Settings")]
    [SerializeField] private float survivalTimeToWin = 60f;

    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text timerText;

    private float timer = 0f;
    private bool hasWon = false;

    private void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (hasWon) return;

        timer += Time.deltaTime;

        float timeRemaining = survivalTimeToWin - timer;

        if (timerText != null)
        {
            timeRemaining = Mathf.Max(0f, timeRemaining);
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }

        if (timer >= survivalTimeToWin)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        hasWon = true;

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f); 
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}