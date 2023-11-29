using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    private GameObject playerObject;
    private TankHealth playerTankHealth;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("PLAYER");

        playerTankHealth = playerObject.GetComponent<TankHealth>();
    }

    private void Start()
    {
     //   Debug.Log("hi");
     //  Debug.Log($"player object is: {playerObject}");
     //   Debug.Log($"player component is: {playerTankHealth}");
     //   Debug.Log("HEALTHHHH " + playerTankHealth.m_CurrentHealth);
    }

    void Update()
    {
        if (playerObject != null)
        {
            playerTankHealth = playerObject.GetComponent<TankHealth>();
            if (playerTankHealth != null && playerTankHealth.m_CurrentHealth > 0)
            {
                Debug.Log(playerTankHealth.m_CurrentHealth);
            }
            if (playerTankHealth != null && playerTankHealth.m_CurrentHealth <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("Menu");
    }
}
