using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _upgradePanel;
    [SerializeField] private Player.PlayerMovement _player;
    [SerializeField] private Player.PlayerData _defaultPlayerData;
    [SerializeField] private MetaData _metaData;
    [SerializeField] private Button _runButton;
    [SerializeField] private Button _jumpButton;
    [SerializeField] private Button _climbButton;
    [SerializeField] private Button _rangeAttackButton;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _dashButton;
    [SerializeField] private Button _doubleJumpButton;

    [SerializeField] private TMP_Text _shardCounter;

    [SerializeField] private List<Upgrade> _runUpgrades = new();
    [SerializeField] private List<Upgrade> _jumpUpgrades = new();
    [SerializeField] private List<Upgrade> _climbUpgrades = new();
    [SerializeField] private List<Upgrade> _rangeAttackUpgrades = new();
    [SerializeField] private List<Upgrade> _attackUpgrades = new();
    [SerializeField] private List<Upgrade> _dashUpgrades = new();
    [SerializeField] private List<Upgrade> _doubleJumpUpgrades = new();

    private List<Button> _buttons = new List<Button>();

    private void Awake()
    {
        if (EventBus.Instance == null)
        {
            EventBus.Instance = GetComponent<EventBus>();
        }
        EventBus.Instance.OnSoulCollect.AddListener(OnSoulCollect);
        EventBus.Instance.ShowUpgrades.AddListener(OnShowUpgrades);
        _buttons.Add(_runButton);
        _buttons.Add(_jumpButton);
        _buttons.Add(_climbButton);
        _buttons.Add(_rangeAttackButton);
        _buttons.Add(_attackButton);
        _buttons.Add(_dashButton);
        _buttons.Add(_doubleJumpButton);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MenuEnter()
    {
        _menuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void MenuExit()
    {
        _menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void ShowUpgrades(bool state)
    {
        UpdateUpgradeButton(_runButton, _metaData.RunLevel, _runUpgrades);
        UpdateUpgradeButton(_jumpButton, _metaData.JumpLevel, _jumpUpgrades);
        UpdateUpgradeButton(_climbButton, _metaData.ClimbLevel, _climbUpgrades);
        UpdateUpgradeButton(_rangeAttackButton, _metaData.RangeAttackLevel, _rangeAttackUpgrades);
        UpdateUpgradeButton(_attackButton, _metaData.AttackLevel, _runUpgrades);
        UpdateUpgradeButton(_dashButton, _metaData.DashLevel, _runUpgrades);
        UpdateUpgradeButton(_doubleJumpButton, _metaData.BonusJumps, _runUpgrades);

        _upgradePanel.SetActive(state);
    }

    private void OnShowUpgrades()
    {
        ShowUpgrades(true);
    }

    private void OnSoulCollect(int value)
    {
        _metaData.SoulShards += 1;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _shardCounter.text = _metaData.SoulShards.ToString();
    }


    private void UpdateUpgradeButton(Button Button, int level, List<Upgrade> upgrades)
    {
        TMP_Text levelText = Button.transform.Find("Level").GetComponent<TMP_Text>();
        levelText.text = "сп:" + level.ToString();
        if (upgrades.Count == level)
        {
            Button.interactable = false;
        }
        else
        {
            TMP_Text costText = Button.transform.Find("Cost").GetComponent<TMP_Text>();
            costText.text = upgrades[level].ToString();
        }
    }

    [ContextMenu("Drop Progress")]
    public void DropProgress()
    {
        _player.Data.BonusJumpCount = _defaultPlayerData.BonusJumpCount;
        _player.Data.runMaxSpeed = _defaultPlayerData.runMaxSpeed;
        _player.Data.jumpHeight = _defaultPlayerData.jumpHeight;
        _player.Data.dashAmount = 0;
        _metaData.SoulShards = 0;
        _metaData.RelicPositions = new();
        _metaData.Relics = 0;
        _metaData.RunLevel = 1;
        _metaData.JumpLevel = 1;
        _metaData.BonusJumps = 0;
        _metaData.DashLevel = 0;
        _metaData.ClimbLevel = 0;
        _metaData.RangeAttackLevel = 0;
        _metaData.AttackLevel = 1;
    }

    [ContextMenu("Give Souls")]
    public void GiveSouls()
    {
        _metaData.SoulShards += 100;
    }
}

public struct Upgrade {
    public int Cost;
    public int Value;
}
