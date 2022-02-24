using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int finish;

    [Header("End Game UI")]
    [SerializeField]
    private GameObject m_VictoryText;
    [SerializeField]
    private GameObject m_LoseText;
    [SerializeField]
    private GameObject m_CreditText;
    [SerializeField]
    private GameObject m_RestartButton;
    [SerializeField]
    private GameObject m_MainMenuButton;

    private void Start()
    {
        finish = 0;
    }

    private void Update()
    {
        // if the end is near...
        if (GameManager.current.GetFinished() == 1 && finish == 0)
        {
            finish = GameManager.current.GetFinished();
            FinishedGame(GameManager.current.GetVictory());
        }
        else if(GameManager.current.GetFinished() == 0 && finish == 1)
        {
            finish = GameManager.current.GetFinished();
            ReloadLevel();
        }
    }

    /// <summary>
    /// activate the end game ui
    /// </summary>
    /// <param name="victory"> has the player won the game </param>
    public void FinishedGame(bool victory)
    {
        if (victory)
            m_VictoryText.SetActive(true);
        else
            m_LoseText.SetActive(true);
        m_RestartButton.SetActive(true);
        m_MainMenuButton.SetActive(true);
        m_CreditText.SetActive(true);
    }
    
    /// <summary>
    /// Reload/load the level
    /// </summary>
    public void ReloadLevel()
    {
        m_VictoryText.SetActive(false);
        m_LoseText.SetActive(false);
        m_RestartButton.SetActive(false);
        m_MainMenuButton.SetActive(false);
        m_CreditText.SetActive(false);
    }
}
