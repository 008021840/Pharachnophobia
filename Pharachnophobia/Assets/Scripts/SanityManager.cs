using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    [SerializeField] private int maxSanity = 100;
    [SerializeField] private int startingSanity = 50;

    [Header("UI")]
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private GameObject loseText;

    private int currentSanity;
    private bool hasLost = false;

    private void Start()
    {
        currentSanity = Mathf.Clamp(startingSanity, 0, maxSanity);

        if (loseText != null)
        {
            loseText.SetActive(false);
        }

        UpdateUI();
        Time.timeScale = 1f;
    }

    public void AddSanity(int amount)
    {
        if (hasLost) return;

        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        UpdateUI();
    }

    public void RemoveSanity(int amount)
    {
        if (hasLost) return;

        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);

        UpdateUI();

        if (currentSanity <= 0)
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {
        hasLost = true;

        if (loseText != null)
        {
            loseText.SetActive(true);
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

    private void UpdateUI()
    {
        if (sanitySlider != null)
        {
            sanitySlider.maxValue = maxSanity;
            sanitySlider.value = currentSanity;
        }
    }

}