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


    [SerializeField]
    private Button ShootingButton;

    [SerializeField]
    private Button StressButton;


    public Vector2 JoystickDirection = Vector2.zero;

    public static UIManager Instance { get { return _instance; } }
    private static UIManager _instance = null;


    public void Start()
    {
        if (StressButton != null)
        {
            StressButton.onClick.AddListener(() => Player.Instance.useStressSpell());
        }
    }

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

    public void setShootingButtonState(bool state)
    {
        ShootingButton.interactable = state;
    }

    public void setStressButtonState(bool used, bool cooling_down)
    {

        if (used)
        {
            Debug.Log("USING STRESS (GREEN)");
            StressButton.GetComponent<Image>().color = Color.green;
            StressButton.interactable = false;
        }
        else 
        {
            Debug.Log("COOLDOWN STRESS (RED)");
            StressButton.GetComponent<Image>().color = Color.white;
            StressButton.interactable = !cooling_down;
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
