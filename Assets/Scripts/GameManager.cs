using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current = null;
    private int m_finished = 0;
    private bool m_victory = false;

    private void Awake()
    {
        if (current == null)
        {
            current = this as GameManager;
        }
        else if (current != this)
            DestroySelf();
    }

    /// <summary>
    /// Reload the scene
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetFinished(int m_finished)
    {
        this.m_finished = m_finished;
    }

    public int GetFinished()
    {
        return this.m_finished;
    }

    public void SetVictory(bool m_victory)
    {
        this.m_victory = m_victory;
    }

    public bool GetVictory()
    {
        return this.m_victory;
    }

    /// <summary>
    /// Destroys the instance.
    /// </summary>
    private void DestroySelf()
    {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
