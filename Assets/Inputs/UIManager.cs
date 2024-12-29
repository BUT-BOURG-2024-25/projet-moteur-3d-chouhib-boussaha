using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Joystick joystick = null;

    [SerializeField]
    Text waveText = null;

    [SerializeField]
    Text enemiesLeft = null;

    [SerializeField]
    Scrollbar HPbar = null;

    [SerializeField]
    private Text levelText = null;

    public Vector2 JoystickDirection = Vector2.zero;

    public static UIManager Instance { get { return _instance; } }
    private static UIManager _instance = null;

    public void Update()
    {
        JoystickDirection = new Vector2(joystick.Direction.x, joystick.Direction.y);
    }

    public void setWaveNumber(int waveNumber)
    {
        this.waveText.text = "Wave : "+waveNumber.ToString();
    }

    public void setEnemiesLeft (int enemiesLeft)
    {
        this.enemiesLeft.text = enemiesLeft.ToString();
    }

    public void setPlayerHP(int playerHP, int playermaxHP)
    {
        float coeff = playerHP / playermaxHP;

    }

    public void SetPlayerLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = level.ToString();
        }
    }

    private void Awake()

    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
