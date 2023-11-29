using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyTankController : MonoBehaviour
{
    private TankHealth enemyTankHealth;

    private void Awake()
    {
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
               // MoveToNextLevel();
            }
        }
    }

    void MoveToNextLevel()
    {
        // Add your logic to move to the next level here
        // For example, load the next scene in the build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
