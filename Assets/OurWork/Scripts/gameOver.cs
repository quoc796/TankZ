using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public TextMeshProUGUI gOverTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI eLefttxt;
    currentLvState lvState;
    public int level;
    public static gameOver i;
    public GameObject enemyPre;

    public void Awake()
    {
        if (i == null) i = this;
        else
        {
            Destroy(this.gameObject);return;
        }

        level = PlayerPrefs.GetInt("CurrentLevel",2);
        StartCoroutine(spawnEnemy());
        
    }
    public void enemyKilled()
    {
        lvState.enemyDied();
        eLefttxt.text = "Enemy Left: " + lvState.getRemain().ToString(); 
        if (lvState.toNextLv()) ToNextLevel();
    }


    IEnumerator spawnEnemy()
    {
        levelTxt.gameObject.SetActive(true);
        levelTxt.text = "Level " + level;
        yield return new WaitForSeconds(3.5f);
        levelTxt.gameObject.SetActive(false);
        lvState = new currentLvState(level);
        eLefttxt.text = "Enemy Left: " + lvState.getRemain().ToString();
        GameObject eSpawnPoint = null;
        while (eSpawnPoint == null)
        {
            eSpawnPoint = GameObject.Find("EnemySpawnPoint");
            yield return null;
        }
        Transform spw = eSpawnPoint.transform;
        int pointIndex = 0;
        for (int i = 0; i < level; i++)
        {
            GameObject enemy = Instantiate(enemyPre, eSpawnPoint.transform);
            enemy.transform.position = spw.GetChild(pointIndex).position;
            if (pointIndex == spw.childCount - 1)
            {
                yield return new WaitForSeconds(15f);
                pointIndex = 0;
            }
            pointIndex++;
        }
        yield return null;
    }

    public void setGameOver()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        StartCoroutine(GameOverCoroutine());
    }
    IEnumerator GameOverCoroutine()
    {
        // Set the gameOver TextMeshProUGUI object active
        gOverTxt.gameObject.SetActive(true);

        // Wait for 3.5 seconds
        yield return new WaitForSeconds(3.5f);
        // Reset the level count (assuming EnemyHealth has a static method ResetLevelCount)
        // Load the Menu scene
        SceneManager.LoadScene("Menu");
    }
    void ToNextLevel()
    {
        level++;
        PlayerPrefs.SetInt("CurrentLevel", level);
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

public class currentLvState
{
    int maxEnemy;
    int diedEnemy;
    public currentLvState(int max)
    {
        maxEnemy = max;
        diedEnemy = 0;
    }
    public void enemyDied()
    {
        diedEnemy++;
    }
    public bool toNextLv()
    {
        return diedEnemy >= maxEnemy;
    }
    public int getRemain()
    {
        return maxEnemy - diedEnemy;
    }
}
