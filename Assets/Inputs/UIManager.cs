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

    [SerializeField]
    private Button DodgeButton;

    [SerializeField]
    private Button AOESlashButton;


    public Vector2 JoystickDirection = Vector2.zero;

    public static UIManager Instance { get { return _instance; } }
    private static UIManager _instance = null;


    public void Start()
    {
        if (StressButton != null)
        {
            StressButton.onClick.AddListener(() => Player.Instance.useStressSpell());
        }
        if (DodgeButton != null)
        {
            DodgeButton.onClick.AddListener(() => Player.Instance.useDodgeSpell());
        }
        if (AOESlashButton != null)
        {
            AOESlashButton.onClick.AddListener(() => Player.Instance.useAOESpell());
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

    public void setDodgeButtonState(bool used, bool cooling_down)
    {

        if (used)
        {
            DodgeButton.GetComponent<Image>().color = Color.green;
            DodgeButton.interactable = false;
        }
        else
        {
            DodgeButton.GetComponent<Image>().color = Color.white;
            DodgeButton.interactable = !cooling_down;
        }
    }

    public void setAOEButtonState(bool used, bool cooling_down)
    {

        if (used)
        {
            AOESlashButton.GetComponent<Image>().color = Color.green;
            AOESlashButton.interactable = false;
        }
        else
        {
            AOESlashButton.GetComponent<Image>().color = Color.white;
            AOESlashButton.interactable = !cooling_down;
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
