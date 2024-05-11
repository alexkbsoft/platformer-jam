using System;
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
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private List<GameObject> _relics;
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
        EventBus.Instance.OnRelicCollect.AddListener(OnRelicCollect);
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
        _introPanel.SetActive(true);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRelicCollect()
    {
        _metaData.Relics++;
        for(int i=0; i< _metaData.Relics; i++)
        {
            _relics[i].SetActive(true);
        }
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
        UpdateUpgradeButton(_attackButton, _metaData.AttackLevel, _attackUpgrades);
        UpdateUpgradeButton(_dashButton, _metaData.DashLevel, _dashUpgrades);
        UpdateUpgradeButton(_doubleJumpButton, _metaData.BonusJumps, _doubleJumpUpgrades);

        Health.HealthProcessor HP = _player.GetComponent<Health.HealthProcessor>();
        HP.TakeHeal(100);

        if (_metaData.Relics >= 3)
        {
            _victoryPanel.SetActive(true);
        }
        else
        {
            _upgradePanel.SetActive(state);
        }
    }

    public void Upgrade(int index)
    {
        switch (index) {
            case 0:
                if (_runUpgrades[_metaData.RunLevel].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _runUpgrades[_metaData.RunLevel].Cost;
                    _player.Data.runMaxSpeed += 0.2f * 20 * _runUpgrades[_metaData.RunLevel].Value;
                    _metaData.RunLevel++;
                }
                break;
            case 1:
                if (_jumpUpgrades[_metaData.JumpLevel].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _jumpUpgrades[_metaData.JumpLevel].Cost;
                    _player.Data.jumpHeight += 0.2f * 10 * _jumpUpgrades[_metaData.JumpLevel].Value;
                    _metaData.JumpLevel++;
                }
                break;
            case 2:
                if (_climbUpgrades[_metaData.ClimbLevel].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _climbUpgrades[_metaData.ClimbLevel].Cost;
                    _player.Data.wallClimbSpeed += 0.2f * 8 * _climbUpgrades[_metaData.ClimbLevel].Value;
                    _metaData.ClimbLevel++;
                }
                break;
            case 3: 
                if (_rangeAttackUpgrades[_metaData.RangeAttackLevel].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _rangeAttackUpgrades[_metaData.RangeAttackLevel].Cost;

                    _metaData.RangeAttack += Mathf.RoundToInt(0.2f * 20 * _rangeAttackUpgrades[_metaData.RangeAttackLevel].Value);
                    _metaData.RangeAttackLevel++;
                    SetStats();
                }
                break;
            case 4: 
                if (_attackUpgrades[_metaData.AttackLevel].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _attackUpgrades[_metaData.AttackLevel].Cost;

                    _metaData.Attack += Mathf.RoundToInt(0.2f * 100 * _attackUpgrades[_metaData.AttackLevel].Value);
                    _metaData.AttackLevel++;
                    SetStats();
                }
                break;
            case 5:
                if (_dashUpgrades[_metaData.DashLevel].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _dashUpgrades[_metaData.DashLevel].Cost;

                    _player.Data.dashAmount = 1;
                    _player.Data.dashSpeed += Mathf.RoundToInt(0.2f * 55 * _dashUpgrades[_metaData.DashLevel].Value);
                    _metaData.DashLevel++;
                    SetStats();
                }
                break;
            case 6:
                if (_doubleJumpUpgrades[_metaData.BonusJumps].Cost <= _metaData.SoulShards)
                {
                    _metaData.SoulShards -= _doubleJumpUpgrades[_metaData.BonusJumps].Cost;
                    _metaData.BonusJumps = 1;
                    _player.Data.BonusJumpCount = 1;
                    SetStats();
                }
                break;
            default: break;
        }

        ShowUpgrades(true);
    }

    private void SetStats()
    {
        GameObject gunGO = GameObject.Find("Gun");
        Shoot.Gun gun = gunGO.GetComponent<Shoot.Gun>();
        gun.Damage = _metaData.RangeAttack;

        GameObject SwordGO = GameObject.Find("Sword");
        Melee.Sword sword = SwordGO.GetComponent<Melee.Sword>();
        sword.Damage = _metaData.Attack;
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
        levelText.text = "сп: " + level.ToString();
        if (upgrades.Count == level)
        {
            Button.interactable = false;
        }
        else
        {
            TMP_Text costText = Button.transform.Find("Cost").GetComponent<TMP_Text>();
            costText.text = upgrades[level].Cost.ToString();
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
        _metaData.Attack = 100;
        _metaData.RangeAttack = 20;
    }

    [ContextMenu("Give Souls")]
    public void GiveSouls()
    {
        _metaData.SoulShards += 100;
    }
}

[Serializable]
public struct Upgrade {
    public int Cost;
    public int Value;
}
