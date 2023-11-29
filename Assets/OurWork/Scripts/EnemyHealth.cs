using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    private TankHealth enemyTankHealth;
    private int level;

    public TextMeshProUGUI levelText;

    private void Awake()
    {
        // Initialize the level from PlayerPrefs
        level = PlayerPrefs.GetInt("CurrentLevel", 2);

        // Find the enemy tank GameObject using a tag
        GameObject enemyTankObject = GameObject.FindGameObjectWithTag("ENEMY");

        // Check if the enemy tank object is found
        if (enemyTankObject != null)
        {
            // Get the TankHealth component from the enemy tank object
            enemyTankHealth = enemyTankObject.GetComponent<TankHealth>();

            // Check if TankHealth is found
            if (enemyTankHealth == null)
            {
                Debug.LogError("TankHealth component not found on the enemy tank object.");
            }
        }
        else
        {
            Debug.LogError("Enemy tank object not found in the scene. Make sure it has the 'EnemyTank' tag.");
        }
    }

    void Update()
    {
        // Check if enemyTankHealth is not null before accessing its properties
        if (enemyTankHealth != null)
        {
            if (enemyTankHealth.m_CurrentHealth <= 0)
            {
                Debug.Log("Enemy tank health is zero. Moving to the next level!");
                StartCoroutine(ShowMessageAndRestart());
            }
        }
    }

    IEnumerator ShowMessageAndRestart()
    {
        Debug.Log("Showing message for 5 seconds...");

        levelText.text = "Level " + level;

        yield return new WaitForSeconds(3.5f);

        level++;

        // Save the updated level to PlayerPrefs
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.Save();

        MoveToNextLevel(level);
    }

    void MoveToNextLevel(int lev)
    {
        Debug.Log("Moving to the next level");
        SceneManager.LoadScene("Main");
    }

    public static void ResetLevelCount()
    {
        // Reset the level count to 1
        PlayerPrefs.SetInt("CurrentLevel", 2);
        PlayerPrefs.Save();
    }
}
