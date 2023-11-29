using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private GameObject playerObject;
    private TankHealth playerTankHealth;

    public TextMeshProUGUI gameOver;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("PLAYER");
        playerTankHealth = playerObject.GetComponent<TankHealth>();
    }

    private void Start()
    {
        // ...
    }

    void Update()
    {
        if (playerObject != null)
        {
            playerTankHealth = playerObject.GetComponent<TankHealth>();
            if (playerTankHealth != null && playerTankHealth.m_CurrentHealth > 0)
            {
                // ...
            }
            if (playerTankHealth != null && playerTankHealth.m_CurrentHealth <= 0)
            {
                StartCoroutine(GameOverCoroutine());
            }
        }
    }

    IEnumerator GameOverCoroutine()
    {
        Debug.Log("Game Over!");

        // Set the gameOver TextMeshProUGUI object active
        gameOver.gameObject.SetActive(true);

        // Wait for 3.5 seconds
        yield return new WaitForSeconds(3.5f);

        // Reset the level count (assuming EnemyHealth has a static method ResetLevelCount)
        EnemyHealth.ResetLevelCount();

        // Load the Menu scene
        SceneManager.LoadScene("Menu");
    }
}
