using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField] GameObject fightPanel, startPanel, statsPanel, gameOverPanel, player, pastPlayer, enemy, damageText,
        trainButton, shop, autoBtn, page1, page2, ability, explosion, 
        sheild, enemySheild, enemySmoke, smoke;
    [SerializeField] SpriteRenderer sky;

    [SerializeField] Slider playerHPBar, enemyHPBar, enemyAtk, enemyDef, enemyDodge;
    [SerializeField] RectTransform attackBar, defenceBar, dodgeBar, trainBar, goldBar;

    float attackPoint = -100, defencePoint = -100, dodgePoint = -100, trainPoint = -100, goldPoint = -100;

    float time, abilityTime, tankTime, dodgeTime, enemyTankTime, enemyDodgeTime;

    public int battle = 1, battleBuff = 1, tier = 1, cycle = 0, lastCycle, enemyMode, abilityUse;
    public int aimCheck;
    public int coins;

    [SerializeField] Text levelText, cycleText, HPText, ATKText, DEFText, AGIText,
        EnemyHPText, EnemyATKText, EnemyDEFText, EnemyAGIText;
    public Text EnemyName;
    [SerializeField] Text autoCoinsText, coinsGainText, goldPriceText, coinsDropText,
        autoTrainText, autoAttackText, DefGainText, AgiGainText, DefenceFillText,
        DodgeFillText, coinText, autoText,
        critChanceText, defenceReFillText, maxDodgeText, RageModeText,
        FullGuardText, SmokeBombText;

    [SerializeField]
    public int autoCoinsPrice, coinsGainPrice, goldPrice, coinsDropPrice,
        autoTrainPrice, autoAttackPrice, DefGainPrice, AgiGainPrice,
        DefenceFillPrice, DodgeFillPrice,
        critChancePrice, defenceReFillPrice, maxDodgePrice, RageModePrice,
        FullGuardPrice, SmokeBombPrice;

    [SerializeField] Button autoCoinsBtn, coinsGainBtn, goldPriceBtn, coinsDropBtn,
        autoTrainBtn, autoAttackBtn, DefGainBtn, AgiGainBtn,
        DefenceFillBtn, DodgeFillBtn,
        critChanceBtn, defenceReFillBtn, maxDodgeBtn, RageModeBtn,
        FullGuardBtn, SmokeBombBtn,
        RageModeUseButton, FullGuardUseButton, SmokeBombUseButton;

    int reFillTime, enemyReFillTime;
    float rageModeOn, fullGuardOn, smokeBombOn,
        enemyRageMode, enemyFullGuard, enemySmokeBomb;

    public bool autoOn = true, defFill, dodgeFill, shopOn = false,
        rageMode, fullGuard, smokeBomb,
        enemyRageUse, enemyGuardUse, enemySmokeUse;

    [SerializeField] AudioSource hurt, block, dodge, statUp, buy, 
        rageModeSound, fullGuardSound, smokeBombSound;

    int trueDodge, enemyTrueDodge;

    float shake;

    public int maxBattle, maxTier, dodgeLoss = 36;

    [SerializeField] GameObject[] dialogues, enemyTiers;
    public int dialogueIndex = 0;

    public int easterEgg = 0, buySkill = 0, clickTime = 0;

    public bool easyMode = true, meetFuture, meetPast;
    [SerializeField] Text easyModeText;

    [SerializeField] GameObject appear, youFutureIcon, youPastIcon, enemyBossIcon;
    [SerializeField] Text IconText;

    [SerializeField] Text timerText;
    public float miniSecond;
    public int seconds, mins;

    [SerializeField] GameObject quitGamePanel;

    public Color32 skyColor;

    private void Start()
    {
        skyColor = sky.color;

        SpawnDialogue(dialogueIndex);
        dialogueIndex++;

        StatsController.instance.playerHP = 100;

        //coins += (int)(Random.Range(0.5f, 1.5f) * battle * 100 * tier) + 100000000;
        StatsController.instance.playerAtk = 20;
        StatsController.instance.playerDef = 50;
        StatsController.instance.playerAgi = 0;
        AGIText.text = "Agi: " + StatsController.instance.playerAgi;

        StatsController.instance.autoTrainLv = 0;
        StatsController.instance.autoAttackLv = 0;
        StatsController.instance.HpGainLv = 0;
        StatsController.instance.AtkGainLv = 0;
        StatsController.instance.DefGainLv = 0;
        StatsController.instance.AgiGainLv = 0;

        StatsController.instance.enemyHP = Random.Range(20, 41) + battle * 20;
        StatsController.instance.enemyAtk = Random.Range(10, 16) + battle * 5;
        StatsController.instance.enemyDef = Random.Range(20, 30) + battle * 3;
        StatsController.instance.enemyAgi = Random.Range(0, 2) + battle;
        StatsController.instance.enemySpeed = Random.Range(30, 51) + tier;
        for (int i = 0; i < battle * 5 * tier; i++)
        {
            int rand = Random.Range(1, 5);
            switch (rand)
            {
                case 1:
                    StatsController.instance.enemyHP += 10;
                    break;
                case 2:
                    StatsController.instance.enemyAtk += 3;
                    break;
                case 3:
                    StatsController.instance.enemyDef += 5;
                    break;
                case 4:
                    StatsController.instance.enemyAgi += 1;
                    break;
            }
        }

        EnemyHPText.text = "HP: " + StatsController.instance.enemyHP;
        EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
        EnemyDEFText.text = "Def: " + StatsController.instance.enemyDef;
        EnemyAGIText.text = "Agi: " + StatsController.instance.enemyAgi;

        playerHPBar.maxValue = StatsController.instance.playerHP;
        playerHPBar.value = StatsController.instance.playerHP;

        enemyHPBar.maxValue = StatsController.instance.enemyHP;
        enemyHPBar.value = StatsController.instance.enemyHP;
        enemyAtk.value = 0;
        enemyDef.value = 0;
        enemyDodge.value = 0;

        enemyMode = Random.Range(1, 4);

        LoadGameData();
    }

    public void SaveGameData()
    {
        SaveSystem.SaveGameData(this);
    }

    public void LoadGameData()
    {
        GameData data = SaveSystem.LoadGameData();

        if (data == null)
            return;

        dialogues[0].SetActive(false);

        StatsController.instance.playerHP = data.playerHP;
        StatsController.instance.playerAtk = data.playerAtk;
        StatsController.instance.playerDef = data.playerDef;
        StatsController.instance.playerAgi = data.playerAgi;

        StatsController.instance.autoCoinsLv = data.autoCoinsLv;
        StatsController.instance.coinsGainLv = data.coinsGainLv;
        StatsController.instance.goldPriceLv = data.goldPriceLv;
        StatsController.instance.coinsDropLv = data.coinsDropLv;

        StatsController.instance.autoTrainLv = data.autoTrainLv;
        StatsController.instance.autoAttackLv = data.autoAttackLv;
        StatsController.instance.HpGainLv = data.HpGainLv;
        StatsController.instance.AtkGainLv = data.AtkGainLv;
        StatsController.instance.DefGainLv = data.DefGainLv;
        StatsController.instance.AgiGainLv = data.AgiGainLv;
        StatsController.instance.critChance = data.critChance;
        StatsController.instance.defenceReFill = data.defenceReFill;
        StatsController.instance.maxDodge = data.maxDodge;

        StatsController.instance.enemyHP = data.enemyHP;
        StatsController.instance.enemyAtk = data.enemyAtk;
        StatsController.instance.enemyDef = data.enemyDef;
        StatsController.instance.enemyAgi = data.enemyAgi;
        StatsController.instance.enemySpeed = data.enemySpeed;

        StatsController.instance.pastPlayerHP = data.pastPlayerHP;
        StatsController.instance.pastPlayerAtk = data.pastPlayerAtk;
        StatsController.instance.pastPlayerDef = data.pastPlayerDef;
        StatsController.instance.pastPlayerAgi = data.pastPlayerAgi;
        StatsController.instance.pastPlayerAutoAttackLv = data.pastPlayerAutoAttackLv;
        StatsController.instance.pastPlayerDefFillLv = data.pastPlayerDefFillLv;
        StatsController.instance.pastPlayerDodgeFillLv = data.pastPlayerDodgeFillLv;
        StatsController.instance.pastPlayerCritChance = data.pastPlayerCritChance;
        StatsController.instance.pastPlayerDefenceReFill = data.pastPlayerDefenceReFill;
        StatsController.instance.pastPlayerMaxDodge = data.pastPlayerMaxDodge;
        StatsController.instance.pastPlayerRageModeLv = data.pastPlayerRageModeLv;
        StatsController.instance.pastPlayerFullGuardLv = data.pastPlayerFullGuardLv;
        StatsController.instance.pastPlayerSmokeBombLv = data.pastPlayerSmokeBombLv;

        battle = data.battle;
        battleBuff = data.battleBuff;
        tier = data.tier;
        cycle = data.cycle;
        lastCycle = data.lastCycle;
        enemyMode = data.enemyMode;
        abilityUse = data.abilityUse;
        coins = data.coins;

        autoCoinsPrice = data.autoCoinsPrice;
        coinsGainPrice = data.coinsGainPrice;
        goldPrice = data.goldPrice;
        coinsDropPrice = data.coinsDropPrice;

        autoTrainPrice = data.autoTrainPrice;
        autoAttackPrice = data.autoAttackPrice;
        DefGainPrice = data.DefGainPrice;

        AgiGainPrice = data.AgiGainPrice;
        DefenceFillPrice = data.DefenceFillPrice;
        DodgeFillPrice = data.DodgeFillPrice;

        critChancePrice = data.critChancePrice;
        defenceReFillPrice = data.defenceReFillPrice;
        maxDodgePrice = data.maxDodgePrice;

        RageModePrice = data.RageModePrice;
        FullGuardPrice = data.FullGuardPrice;
        SmokeBombPrice = data.SmokeBombPrice;

        autoOn = data.autoOn;
        defFill = data.defFill;
        dodgeFill = data.dodgeFill;
        shopOn = data.shopOn;

        maxBattle = data.maxBattle;
        maxTier = data.maxTier;

        dialogueIndex = data.dialogueIndex;
        easterEgg = data.easterEgg;
        buySkill = data.buySkill;

        easyMode = data.easyMode;
        meetFuture = data.meetFuture;
        meetPast = data.meetPast;
        miniSecond = data.miniSecond;
        seconds = data.seconds;
        mins = data.mins;

        skyColor.r = (byte)data.skyColor[0];
        skyColor.g = (byte)data.skyColor[1];
        skyColor.b = (byte)data.skyColor[2];
        skyColor.a = (byte)data.skyColor[3];

        rageMode = data.rageMode;
        fullGuard = data.fullGuard;
        smokeBomb = data.smokeBomb;

        EnemyName.text = data.EnemyName;

        coinText.text = "$ " + coins;

        sky.color = skyColor;

        if (StatsController.instance.autoCoinsLv == 100)
        {
            autoCoinsText.text = "autoCoinsLv: Max";
            autoCoinsBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            autoCoinsText.text = "AutoCoinsLv: " + StatsController.instance.autoCoinsLv + "\n $" + autoCoinsPrice;
        }

        if (StatsController.instance.coinsGainLv == 30)
        {
            coinsGainText.text = "CoinsGainLv: Max";
            coinsGainBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            coinsGainText.text = "CoinsGainLv: " + StatsController.instance.coinsGainLv + "\n $" + coinsGainPrice;
        }

        if (StatsController.instance.goldPriceLv == 299)
        {
            goldPriceText.text = "GoldPrice: " + (1 + StatsController.instance.goldPriceLv) + "x";
            goldPriceBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            goldPriceText.text = "GoldPrice: " + (1 + StatsController.instance.goldPriceLv) + "x\n $" + goldPrice;
        }

        if (StatsController.instance.coinsDropLv == 7)
        {
            coinsDropText.text = "CoinsDrop: " + (1 + StatsController.instance.coinsDropLv) + "x";
            coinsDropBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            coinsDropText.text = "CoinsDrop: " + (1 + StatsController.instance.coinsDropLv) + "x\n $" + coinsDropPrice;
        }

        if (StatsController.instance.autoTrainLv == 100)
        {
            autoTrainText.text = "AutoTrainLv: Max";
            autoTrainBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            autoTrainText.text = "AutoTrainLv: " + StatsController.instance.autoTrainLv + "\n $" + autoTrainPrice;
        }

        if (StatsController.instance.autoAttackLv == 80)
        {
            autoAttackText.text = "AutoAttackLv: Max";
            autoAttackBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            autoAttackText.text = "AutoAttackLv: " + StatsController.instance.autoAttackLv + "\n $" + autoAttackPrice;
        }

        if (StatsController.instance.DefGainLv == 12)
        {
            DefGainText.text = "DefGainLv: Max";
            DefGainBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            DefGainText.text = "DefGainLv: " + StatsController.instance.DefGainLv + "\n $" + DefGainPrice;
        }

        if (StatsController.instance.AgiGainLv == 5)
        {
            AgiGainText.text = "AgiGainLv: Max";
            AgiGainBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            AgiGainText.text = "AgiGainLv: " + StatsController.instance.AgiGainLv + "\n $" + AgiGainPrice;
        }

        if (defFill == true)
        {
            defencePoint = 0;
            defenceBar.offsetMax = new Vector2(0, defencePoint);
            DefenceFillText.text = "DefenceFill: Max";
            DefenceFillBtn.interactable = false;
        }
        else
        {
            DefenceFillText.text = "DefenceFill: 0\n $" + DefenceFillPrice;
        }

        if (dodgeFill == true)
        {
            dodgePoint = 0;
            dodgeBar.offsetMax = new Vector2(0, dodgePoint);
            DodgeFillText.text = "DodgeFill: Max";
            DodgeFillBtn.interactable = false;
        }
        else
        {
            DodgeFillText.text = "DodgeFill: 0 \n $" + DodgeFillPrice;
        }

        if (StatsController.instance.critChance == 100)
        {
            critChanceText.text = "CritChance: " + StatsController.instance.critChance + "%";
            critChanceBtn.interactable = false;
            coinText.text = "$ " + coins;
            return;
        }
        else
        {
            critChanceText.text = "CritChance: " + StatsController.instance.critChance + "%\n $" + critChancePrice;
        }

        if (StatsController.instance.defenceReFill == 5)
        {
            defenceReFillText.text = "DefenceReFill: " + StatsController.instance.defenceReFill;
            defenceReFillBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            defenceReFillText.text = "DefenceReFill: " + StatsController.instance.defenceReFill + "\n $" + defenceReFillPrice;
        }

        if (StatsController.instance.maxDodge == 10)
        {
            maxDodgeText.text = "MaxDodge: " + (StatsController.instance.maxDodge + 60) + "%";
            maxDodgeBtn.interactable = false;
            coinText.text = "$ " + coins;
        }
        else
        {
            maxDodgeText.text = "MaxDodge: " + (StatsController.instance.maxDodge + 60) + "%\n $" + maxDodgePrice;
        }

        if (rageMode == true)
        {
            RageModeText.text = "RageMode(Get)";
            RageModeUseButton.gameObject.SetActive(true);
            RageModeBtn.interactable = false;
        }
        else
        {
            RageModeText.text = "RageMode\n $" + RageModePrice;
        }

        if (fullGuard == true)
        {
            FullGuardText.text = "FullGuard(Get)";
            FullGuardUseButton.gameObject.SetActive(true);
            FullGuardBtn.interactable = false;
        }
        else
        {
            FullGuardText.text = "FullGuard\n $" + FullGuardPrice;
        }

        if (smokeBomb == true)
        {
            SmokeBombText.text = "SmokeBomb(Get)";
            SmokeBombUseButton.gameObject.SetActive(true);
            SmokeBombBtn.interactable = false;
        }
        else
        {
            SmokeBombText.text = "SmokeBomb\n $" + SmokeBombPrice;
        }


        if (StatsController.instance.autoAttackLv >= 1 || StatsController.instance.autoTrainLv >= 1)
            autoBtn.SetActive(true);


        levelText.text = "Battle: " + battle.ToString();
        if (battle % 100 == 0)
            levelText.text += "(BOSS)";

        StatsController.instance.playerHP += 5 * (StatsController.instance.HpGainLv + 1);
        HPText.text = "HP: " + StatsController.instance.playerHP;
        playerHPBar.maxValue = StatsController.instance.playerHP;
        playerHPBar.value = StatsController.instance.playerHP;

        EnemyHPText.text = "HP: " + StatsController.instance.enemyHP;
        EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
        EnemyDEFText.text = "Def: " + StatsController.instance.enemyDef;
        EnemyAGIText.text = "Agi: " + StatsController.instance.enemyAgi;

        enemyHPBar.maxValue = StatsController.instance.enemyHP;
        enemyHPBar.value = StatsController.instance.enemyHP;
        enemyHPBar.value = enemyHPBar.value;

        enemyAtk.value = 0;
        enemyDef.value = 0;
        enemyDodge.value = 0;

        if (EnemyName.text == "You(Future)")
        {
            StartCoroutine(DisableAnimation());
            youFutureIcon.SetActive(true);
            youPastIcon.SetActive(false);
            enemyBossIcon.SetActive(false);
            IconText.text = "You(Future) has appeared!";
        }

        if (EnemyName.text == "You(Past)")
        {
            StartCoroutine(DisableAnimation());
            youFutureIcon.SetActive(false);
            youPastIcon.SetActive(true);
            enemyBossIcon.SetActive(false);
            IconText.text = "You(Past) has appeared!";
        }

        if (battle % 100 == 0)
        {
            StartCoroutine(DisableAnimation());
            youFutureIcon.SetActive(false);
            youPastIcon.SetActive(false);
            enemyBossIcon.SetActive(true);
            IconText.text = "Enemy(Boss) has appeared!";
        }

        if (battle == 2 && dialogueIndex == 1)
        {
            dialogueIndex++;
            SpawnDialogue(1);
        }

        if (battle == 3 && meetFuture == false)
        {
            meetFuture = true;
            SpawnDialogue(2);
        }

        if (battle == 2 && meetFuture == true && meetPast == false)
        {
            meetPast = true;
            dialogueIndex = 5;
            SpawnDialogue(4);
        }

        if ((battle == 6 && dialogueIndex == 5) || (battle == 11 && dialogueIndex == 6) || (battle == 21 && dialogueIndex == 7) ||
            (battle == 31 && dialogueIndex == 8) || (battle == 41 && dialogueIndex == 9) || (battle == 51 && dialogueIndex == 10) ||
            (battle == 61 && dialogueIndex == 11) || (battle == 71 && dialogueIndex == 12) || (battle == 81 && dialogueIndex == 13) ||
            (battle == 91 && dialogueIndex == 14) || (battle == 101 && dialogueIndex == 15))
        {
            SpawnDialogue(dialogueIndex);
            dialogueIndex++;
        }

        if (dialogueIndex >= 16 && !timerText.gameObject.activeInHierarchy)
        {
            timerText.gameObject.SetActive(true);
            timerText.text = "Boss Beaten in " + mins + "minutes and " + seconds + "seconds.";
        }
        if (battle == 201 && dialogueIndex == 16)
        {
            SpawnDialogue(dialogueIndex);
            dialogueIndex++;
        }

        if (!trainButton.activeInHierarchy && dialogueIndex >= 6 || !shop.activeInHierarchy && dialogueIndex >= 6)
        {
            trainButton.SetActive(true);
            shopOn = true;
        }
        if (!page1.activeInHierarchy && dialogueIndex >= 7)
        {
            page1.SetActive(true);
        }
        if (!page2.activeInHierarchy && dialogueIndex >= 8)
        {
            page2.SetActive(true);
        }
        if (!ability.activeInHierarchy && dialogueIndex >= 11)
        {
            ability.SetActive(true);
        }

        if (shopOn)
            shop.SetActive(true);

        if (battle > maxBattle)
            maxBattle = battle;
        if (tier > maxTier)
            maxTier = tier;

        abilityUse = Random.Range(0, 8);
        abilityTime = 0;
        rageModeOn = -1;
        fullGuardOn = -1;
        smokeBombOn = -1;
        enemyRageMode = -1;
        enemyFullGuard = -1;
        enemySmokeBomb = -1;

        enemyRageUse = false;
        enemyGuardUse = false;
        enemySmokeUse = false;

        sheild.SetActive(false);
        enemySheild.SetActive(false);
        smoke.SetActive(false);
        enemySmoke.SetActive(false);

        reFillTime = 0;
        enemyReFillTime = 0;

        if (rageMode)
            RageModeUseButton.gameObject.SetActive(true);
        if (fullGuard)
            FullGuardUseButton.gameObject.SetActive(true);
        if (smokeBomb)
            SmokeBombUseButton.gameObject.SetActive(true);

        attackPoint = -100; defencePoint = -100; dodgePoint = -100;
        attackBar.offsetMax = new Vector2(0, attackPoint);

        if (defFill)
        {
            defencePoint = 0;
            if (StatsController.instance.pastPlayerDefFillLv == true && EnemyName.text == "You(Past)" ||
                EnemyName.text == "You(Future)")
                enemyDef.value = 100;
        }
        if (dodgeFill)
        {
            dodgePoint = 0;
            if (StatsController.instance.pastPlayerDefFillLv == true && EnemyName.text == "You(Past)" ||
                EnemyName.text == "You(Future)")
                enemyDodge.value = 100;
        }
        defenceBar.offsetMax = new Vector2(0, defencePoint);
        dodgeBar.offsetMax = new Vector2(0, dodgePoint);

        dodgeTime = 0; tankTime = 0; enemyDodgeTime = 0; enemyTankTime = 0;

        enemyMode = Random.Range(1, 4);

        DodgePos();
        CheckTier();

        fightPanel.SetActive(false);
        startPanel.SetActive(true);

        if (EnemyName.text == "Enemy")
        {
            pastPlayer.SetActive(false);
        }
        else
        {
            pastPlayer.SetActive(true);
        }

        HPText.text = "HP: " + StatsController.instance.playerHP;
        ATKText.text = "Atk: " + StatsController.instance.playerAtk;
        DEFText.text = "Def: " + StatsController.instance.playerDef;
        AGIText.text = "Agi: " + StatsController.instance.playerAgi;

        cycleText.text = "Cycle: " + cycle.ToString();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!quitGamePanel.activeInHierarchy)
                quitGamePanel.SetActive(true);
            else
                quitGamePanel.SetActive(false);
        }

        if (!timerText.gameObject.activeInHierarchy)
        {
            miniSecond += Time.fixedDeltaTime;
            if(miniSecond >= 1)
            {
                miniSecond = 0;
                seconds++;
            }    
            if(seconds >= 60)
            {
                seconds = 0;
                mins++;
            }
        }
        if(shake > 0)
        {
            shake -= Time.deltaTime * 3;
            if(shake < 0)
            {
                shake = 0;
            }
            Camera.main.transform.position = new Vector3(Random.Range(-shake, shake), Random.Range(-shake, shake), -10);
        }

        if (StatsController.instance.autoCoinsLv > 0 && !gameOverPanel.activeInHierarchy)
        {
            goldPoint += Time.deltaTime * (5 + 5 * StatsController.instance.autoCoinsLv);
            if (goldPoint >= 0)
            {
                goldPoint = -100;
                coins += Random.Range(10, 20) * (1 + StatsController.instance.goldPriceLv);
                coinText.text = "$ " + coins.ToString();
                if(!fightPanel.activeInHierarchy && !gameOverPanel.activeInHierarchy)
                    buy.Play();
            }

            goldBar.offsetMax = new Vector2(0, goldPoint);
        }

        if (!fightPanel.activeInHierarchy && !gameOverPanel.activeInHierarchy)
        {
            if (StatsController.instance.autoTrainLv > 0 && autoOn == true)
                AutoTrain();
            return;
        }
        if (!fightPanel.activeInHierarchy)
            return;


        if (enemySmokeBomb > 0)
        {
            trueDodge = StatsController.instance.playerAgi / 10;
            dodgeLoss = 72;
        }
        else
        {
            trueDodge = StatsController.instance.playerAgi;
            dodgeLoss = 36;
        }

        if (dodgePoint > -100)
        {
            dodgePoint -= Time.deltaTime * dodgeLoss / (1 + trueDodge / 100);
            if (dodgePoint < -100)
                dodgePoint = -100;
            dodgeBar.offsetMax = new Vector2(0, dodgePoint);
        }

        if(rageModeOn > 0)
        {
            rageModeOn -= Time.deltaTime;
            GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 2), Quaternion.identity);
            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 0, 0, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = "*";
            if (rageModeOn <= 0)
                rageModeOn = 0;
        }
        if (fullGuardOn > 0)
        {
            fullGuardOn -= Time.deltaTime;
            sheild.SetActive(true);
            if (fullGuardOn <= 0)
                fullGuardOn = 0;
        }
        else
            sheild.SetActive(false);

        if (smokeBombOn > 0)
        {
            smokeBombOn -= Time.deltaTime;
            enemySmoke.SetActive(true);
            if (smokeBombOn <= 0)
                smokeBombOn = 0;
        }
        else
            enemySmoke.SetActive(false);

        if (StatsController.instance.autoAttackLv > 0 && autoOn == true)
            AutoAttack();

        if (defencePoint >= -99)
        {
            tankTime += Time.deltaTime;
            if (tankTime >= 3)
            {
                tankTime = 0;
                StatsController.instance.playerDef += (StatsController.instance.DefGainLv + 1);
                DEFText.text = "Def: " + StatsController.instance.playerDef;
            }
        }
        if (dodgePoint >= -99)
        {
            dodgeTime += Time.deltaTime;
            if (dodgeTime >= 5)
            {
                dodgeTime = 0;
                StatsController.instance.playerAgi += (StatsController.instance.AgiGainLv + 1);
                AGIText.text = "Agi: " + StatsController.instance.playerAgi;
            }
        }
        EnemyAI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Shop(string skillName)
    {
        switch (skillName)
        {
            case "AutoCoins":
                if (StatsController.instance.autoCoinsLv == 100 || coins < autoCoinsPrice)
                    return;
                coins -= autoCoinsPrice;
                autoCoinsPrice += 50;
                if (StatsController.instance.autoCoinsLv >= 20)
                    autoCoinsPrice += 450;
                if (StatsController.instance.autoCoinsLv >= 50)
                    autoCoinsPrice += 500;
                StatsController.instance.autoCoinsLv++;
                buy.Play();
                if (StatsController.instance.autoCoinsLv == 100)
                {
                    autoCoinsText.text = "autoCoinsLv: Max";
                    autoCoinsBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                autoCoinsText.text = "AutoCoinsLv: " + StatsController.instance.autoCoinsLv + "\n $" + autoCoinsPrice;
                break;
            case "CoinsGain":
                if (StatsController.instance.coinsGainLv == 30 || coins < coinsGainPrice)
                    return;
                coins -= coinsGainPrice;
                coinsGainPrice += 250;
                if (StatsController.instance.coinsGainLv >= 10)
                    coinsGainPrice += 250;
                if (StatsController.instance.coinsGainLv >= 20)
                    coinsGainPrice += 2500;
                StatsController.instance.coinsGainLv++;
                buy.Play();
                if (StatsController.instance.coinsGainLv == 30)
                {
                    coinsGainText.text = "CoinsGainLv: Max";
                    coinsGainBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                coinsGainText.text = "CoinsGainLv: " + StatsController.instance.coinsGainLv + "\n $" + coinsGainPrice;
                break;
            case "GoldPrice":
                if (StatsController.instance.goldPriceLv == 299 || coins < goldPrice)
                    return;
                coins -= goldPrice;
                goldPrice += 100;
                if (StatsController.instance.goldPriceLv >= 50)
                    goldPrice += 100;
                if (StatsController.instance.goldPriceLv >= 100)
                    goldPrice += 100;
                if (StatsController.instance.goldPriceLv >= 200)
                    goldPrice += 200;
                StatsController.instance.goldPriceLv++;
                buy.Play();
                if (StatsController.instance.goldPriceLv == 299)
                {
                    goldPriceText.text = "GoldPrice: " + (1 + StatsController.instance.goldPriceLv) + "x";
                    goldPriceBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                goldPriceText.text = "GoldPrice: " + (1 + StatsController.instance.goldPriceLv) + "x\n $" + goldPrice;
                break;
            case "CoinsDrop":
                if (StatsController.instance.coinsDropLv == 7 || coins < coinsDropPrice)
                    return;
                coins -= coinsDropPrice;
                coinsDropPrice *= 3;
                StatsController.instance.coinsDropLv++;
                buy.Play();
                if (StatsController.instance.coinsDropLv == 7)
                {
                    coinsDropText.text = "CoinsDrop: " + (1 + StatsController.instance.coinsDropLv) + "x";
                    coinsDropBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                coinsDropText.text = "CoinsDrop: " + (1 + StatsController.instance.coinsDropLv) + "x\n $" + coinsDropPrice;
                break;

            case "AutoTrain":
                if (StatsController.instance.autoTrainLv == 100 || coins < autoTrainPrice)
                    return;
                coins -= autoTrainPrice;
                autoTrainPrice += 100;
                if (StatsController.instance.autoTrainLv >= 50)
                    autoTrainPrice += 1000;
                if (!autoBtn.activeInHierarchy)
                    autoBtn.SetActive(true);
                StatsController.instance.autoTrainLv++;
                buy.Play();
                if (StatsController.instance.autoTrainLv == 100)
                {
                    autoTrainText.text = "AutoTrainLv: Max";
                    autoTrainBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                autoTrainText.text = "AutoTrainLv: " + StatsController.instance.autoTrainLv + "\n $" + autoTrainPrice;
                break;
            case "AutoAttack":
                if (StatsController.instance.autoAttackLv == 80 || coins < autoAttackPrice)
                    return;
                coins -= autoAttackPrice;
                autoAttackPrice += 350;
                if (!autoBtn.activeInHierarchy)
                    autoBtn.SetActive(true);
                StatsController.instance.autoAttackLv++;

                if (StatsController.instance.autoAttackLv >= 10)
                    autoAttackPrice += 350;
                if (StatsController.instance.autoAttackLv >= 20)
                    autoAttackPrice += 350 * 2;
                if (StatsController.instance.autoAttackLv >= 30)
                    autoAttackPrice += 350 * 4;
                if (StatsController.instance.autoAttackLv >= 40)
                    autoAttackPrice += 350 * 8;
                if (StatsController.instance.autoAttackLv >= 50)
                    autoAttackPrice += 350 * 10;

                buy.Play();
                if (StatsController.instance.autoAttackLv == 80)
                {
                    autoAttackText.text = "AutoAttackLv: Max";
                    autoAttackBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                autoAttackText.text = "AutoAttackLv: " + StatsController.instance.autoAttackLv + "\n $" + autoAttackPrice;
                break;
            case "DefGain":
                if (StatsController.instance.DefGainLv == 12 || coins < DefGainPrice)
                    return;
                coins -= DefGainPrice;
                DefGainPrice += 1000;
                if (StatsController.instance.DefGainLv >= 2)
                    DefGainPrice += 1000;
                if (StatsController.instance.DefGainLv >= 3)
                    DefGainPrice += 1000;
                if (StatsController.instance.DefGainLv >= 4)
                    DefGainPrice += 1000;
                if (StatsController.instance.DefGainLv == 5)
                    DefGainPrice += 1000;
                if (StatsController.instance.DefGainLv >= 5)
                    DefGainPrice += 5000;
                StatsController.instance.DefGainLv++;
                buy.Play();
                if (StatsController.instance.DefGainLv == 12)
                {
                    DefGainText.text = "DefGainLv: Max";
                    DefGainBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                DefGainText.text = "DefGainLv: " + StatsController.instance.DefGainLv + "\n $" + DefGainPrice;
                break;
            case "AgiGain":
                if (StatsController.instance.AgiGainLv == 5 || coins < AgiGainPrice)
                    return;
                coins -= AgiGainPrice;
                AgiGainPrice *= 2;
                StatsController.instance.AgiGainLv++;
                buy.Play();
                if (StatsController.instance.AgiGainLv == 5)
                {
                    AgiGainText.text = "AgiGainLv: Max";
                    AgiGainBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                AgiGainText.text = "AgiGainLv: " + StatsController.instance.AgiGainLv + "\n $" + AgiGainPrice;
                break;
            case "DefenceFill":
                if (defFill == true || coins < DefenceFillPrice)
                    return;
                coins -= DefenceFillPrice;

                defFill = true;
                defencePoint = 0;
                buy.Play();
                defenceBar.offsetMax = new Vector2(0, defencePoint);

                DefenceFillText.text = "DefenceFill: Max";
                DefenceFillBtn.interactable = false;
                break;
            case "DodgeFill":
                if (dodgeFill == true || coins < DodgeFillPrice)
                    return;
                coins -= DodgeFillPrice;

                dodgeFill = true;
                dodgePoint = 0;
                buy.Play();
                dodgeBar.offsetMax = new Vector2(0, dodgePoint);
                DodgeFillText.text = "DodgeFill: Max";
                DodgeFillBtn.interactable = false;
                break;
            case "CritChance":
                if (StatsController.instance.critChance == 100 || coins < critChancePrice)
                    return;
                coins -= critChancePrice;
                critChancePrice += 5000;
                StatsController.instance.critChance++;
                buy.Play();
                if (StatsController.instance.critChance == 100)
                {
                    critChanceText.text = "CritChance: " + StatsController.instance.critChance + "%";
                    critChanceBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                critChanceText.text = "CritChance: " + StatsController.instance.critChance + "%\n $" + critChancePrice;
                break;
            case "DefenceReFill":
                if (StatsController.instance.defenceReFill == 5 || coins < defenceReFillPrice)
                    return;
                coins -= defenceReFillPrice;
                coinText.text = "$ " + coins;
                defenceReFillPrice *= 3;
                StatsController.instance.defenceReFill++;
                buy.Play();
                if (StatsController.instance.defenceReFill == 5)
                {
                    defenceReFillText.text = "DefenceReFill: " + StatsController.instance.defenceReFill;
                    defenceReFillBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                defenceReFillText.text = "DefenceReFill: " + StatsController.instance.defenceReFill + "\n $" + defenceReFillPrice;
                break;
            case "MaxDodge":
                if (StatsController.instance.maxDodge == 10 || coins < maxDodgePrice)
                    return;
                coins -= maxDodgePrice;
                maxDodgePrice += 300000;
                StatsController.instance.maxDodge++;
                buy.Play();
                if (StatsController.instance.maxDodge == 10)
                {
                    maxDodgeText.text = "MaxDodge: " + (StatsController.instance.maxDodge + 60) + "%";
                    maxDodgeBtn.interactable = false;
                    coinText.text = "$ " + coins;
                    return;
                }
                maxDodgeText.text = "MaxDodge: " + (StatsController.instance.maxDodge + 60) + "%\n $" + maxDodgePrice;
                break;
            case "RageMode":
                if (rageMode == true || coins < RageModePrice)
                    return;
                coins -= RageModePrice;
                if(buySkill == 0)
                {
                    buySkill++;
                    SpawnDialogue(16);
                }

                if(fullGuard != true)
                {
                    FullGuardPrice *= 2;
                    FullGuardText.text = "FullGuard\n $" + FullGuardPrice;
                }
                if (smokeBomb != true)
                {
                    SmokeBombPrice *= 2;
                    SmokeBombText.text = "SmokeBomb\n $" + SmokeBombPrice;
                }

                rageMode = true;
                RageModeUseButton.gameObject.SetActive(true);
                buy.Play();
                RageModeText.text = "RageMode(Get)";
                RageModeBtn.interactable = false;
                StatsController.instance.pastPlayerRageModeLv = true;
                break;
            case "FullGuard":
                if (fullGuard == true || coins < FullGuardPrice)
                    return;
                coins -= FullGuardPrice;
                if (buySkill == 0)
                {
                    buySkill++;
                    SpawnDialogue(16);
                }

                if (rageMode != true)
                {
                    RageModePrice *= 2;
                    RageModeText.text = "RageMode\n $" + RageModePrice;
                }
                if (smokeBomb != true)
                {
                    SmokeBombPrice *= 2;
                    SmokeBombText.text = "SmokeBomb\n $" + SmokeBombPrice;
                }

                fullGuard = true;
                FullGuardUseButton.gameObject.SetActive(true);
                buy.Play();
                FullGuardText.text = "FullGuard(Get)";
                FullGuardBtn.interactable = false;
                StatsController.instance.pastPlayerFullGuardLv = true;
                break;
            case "SmokeBomb":
                if (smokeBomb == true || coins < SmokeBombPrice)
                    return;
                coins -= SmokeBombPrice;
                if (buySkill == 0)
                {
                    buySkill++;
                    SpawnDialogue(16);
                }

                if (rageMode != true)
                {
                    RageModePrice *= 2;
                    RageModeText.text = "RageMode\n $" + RageModePrice;
                }
                if (fullGuard != true)
                {
                    FullGuardPrice *= 2;
                    FullGuardText.text = "FullGuard\n $" + FullGuardPrice;
                }

                smokeBomb = true;
                SmokeBombUseButton.gameObject.SetActive(true);
                buy.Play();
                SmokeBombText.text = "SmokeBomb(Get)";
                SmokeBombBtn.interactable = false;
                StatsController.instance.pastPlayerSmokeBombLv = true;
                break;
        }
        coinText.text = "$ " + coins;
    }

    public void AutoOnOff()
    {
        if(autoOn == true)
        {
            autoText.text = "Auto: Off";
            autoOn = false;
        }
        else
        {
            autoText.text = "Auto: On";
            autoOn = true;
        }
    }

    void AutoTrain()
    {
        trainPoint += Time.deltaTime * (5 + 5 * StatsController.instance.autoTrainLv);
        if (trainPoint >= 0)
        {
            trainPoint = -100;
            StatsController.instance.playerHP += 3 * (StatsController.instance.HpGainLv + 1);
            HPText.text = "HP: " + StatsController.instance.playerHP;
            StatsController.instance.playerAtk += (StatsController.instance.AtkGainLv + 1);
            ATKText.text = "Atk: " + StatsController.instance.playerAtk;
            statUp.Play();
        }

        GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(-6.5f, -3.3f, 2) +
                new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-0.5f, 0.5f), -1), Quaternion.identity);
        damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 0, 0, 255);
        damageTextPrefabs.GetComponent<TextMesh>().text = "+";

        trainBar.offsetMax = new Vector2(0, trainPoint);
    }

    public void RageMode()
    {
        rageModeOn = 3;
        if ((enemyRageUse == false && EnemyName.text == "You(Past)" &&
          StatsController.instance.pastPlayerRageModeLv == true) ||
          (enemyRageUse == false && EnemyName.text == "You(Future)" && rageMode == true))
        {
            rageModeSound.Play();
            enemyRageUse = true;
            enemyRageMode = 3;
        }
        if ((enemyGuardUse == false && EnemyName.text == "You(Past)" &&
          StatsController.instance.pastPlayerFullGuardLv == true) ||
          (enemyGuardUse == false && EnemyName.text == "You(Future)" && fullGuard == true))
        {
            fullGuardSound.Play();
            enemyGuardUse = true;
            enemyFullGuard = 3;
            return;
        }
    }
    public void FullGuard()
    {
        fullGuardOn = 3;
    }
    public void SmokeBomb()
    {
        smokeBombOn = 3;
        enemyDodge.value /= 2;
        if ((enemyGuardUse == false && EnemyName.text == "You(Past)" &&
          StatsController.instance.pastPlayerFullGuardLv == true) ||
          (enemyGuardUse == false && EnemyName.text == "You(Future)" && fullGuard == true))
        {
            fullGuardSound.Play();
            enemyGuardUse = true;
            enemyFullGuard = 3;
            abilityTime = 0;
            abilityUse = Random.Range(1, 8);
        }
        if ((enemySmokeUse == false && EnemyName.text == "You(Past)" &&
          StatsController.instance.pastPlayerSmokeBombLv == true) ||
          (enemySmokeUse == false && EnemyName.text == "You(Future)" && smokeBomb == true))
        {
            smokeBombSound.Play();
            enemySmokeUse = true;
            enemySmokeBomb = 3;
            dodgePoint /= 2;
            dodgeBar.offsetMax = new Vector2(0, dodgePoint);
            abilityTime = 0;
            abilityUse = Random.Range(1, 8);
        }
    }

    void AutoAttack()
    {
        attackPoint += Time.deltaTime * (15 + 5 * StatsController.instance.autoAttackLv);

        if (attackPoint >= 0)
        {
            attackPoint = -100;
            PlayerDamage((int)(StatsController.instance.playerAtk * Random.Range(0.9f, 1.1f)));
        }

        GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(-5.3f, -3.3f, 2) +
                new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-0.5f, 0.5f), -1), Quaternion.identity);
        damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 0, 0, 255);
        damageTextPrefabs.GetComponent<TextMesh>().text = "+";

        attackBar.offsetMax = new Vector2(0, attackPoint);
    }

    public void Attack()
    {
        attackPoint += 12;
        if (easyMode)
            attackPoint += 12;

        if (rageModeOn > 0)
        {
            attackPoint += 50;
            if (easyMode)
                attackPoint += 50;
        }

        if (attackPoint >= 0)
        {
            attackPoint = -100;
            PlayerDamage((int)(StatsController.instance.playerAtk * Random.Range(0.9f, 1.1f)));
        }

        GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(-5.3f, -3.3f, 2) +
                new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-0.5f, 0.5f), -1), Quaternion.identity);
        damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 0, 0, 255);
        damageTextPrefabs.GetComponent<TextMesh>().text = "+";

        attackBar.offsetMax = new Vector2(0,attackPoint);
    }

    void PlayerDamage(int damage)
    {
        aimCheck = Random.Range(0, 100);
        
        float checkAim = 1;
        if (EnemyName.text == "You(Past)")
            checkAim = (60f + (StatsController.instance.pastPlayerMaxDodge / 2)) / 100f;
        else if (EnemyName.text == "You(Future)")
            checkAim = (60f + (StatsController.instance.maxDodge / 2)) / 100f;
        else if (EnemyName.text == "Enemy")
            checkAim = 0.6f;

        if (enemySmokeBomb > 0)
            checkAim /= 2;
        if (aimCheck < enemyDodge.value * checkAim)
        {
            dodge.Play();
            GameObject damageTextPrefabs = Instantiate(damageText, enemy.transform.position +
                new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.3f, 0.3f), -1), Quaternion.identity);
            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(0, 180, 80, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = "Miss";
            return;
        }
        shake = 0.3f;
        if (aimCheck <= StatsController.instance.critChance - 1)
            damage *= 2;

        StatsController.instance.playerAtk += (StatsController.instance.AtkGainLv + 1);
        ATKText.text = "Atk: " + StatsController.instance.playerAtk;
        if (damage > StatsController.instance.enemyDef * enemyDef.value / 100 && enemyFullGuard <= 0)
        {
            enemyHPBar.value -= damage - StatsController.instance.enemyDef * enemyDef.value / 100;

            GameObject damageTextPrefabs = Instantiate(damageText, enemy.transform.position + 
                new Vector3(Random.Range(-0.6f, 0.6f),Random.Range(-0.3f, 0.3f), -1), Quaternion.identity);

            if (aimCheck <= StatsController.instance.critChance - 1)
                damageTextPrefabs.GetComponent<TextMesh>().fontSize *= 2;
            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 15, 0, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = (-(int)(damage - StatsController.instance.enemyDef * enemyDef.value / 100)).ToString();
            hurt.Play();

            enemyDef.value = ((StatsController.instance.enemyDef * enemyDef.value / 100) - damage) /
            StatsController.instance.enemyDef * 100;
            enemyDef.value = Mathf.Clamp(enemyDef.value, 0, 100);

            if ((enemyReFillTime < StatsController.instance.pastPlayerDefenceReFill && EnemyName.text == "You(Past)") ||
                (enemyReFillTime < StatsController.instance.defenceReFill && EnemyName.text == "You(Future)"))
            {
                enemyReFillTime++;
                enemyDef.value = 100;
            }
        }
        else
        {
            GameObject damageTextPrefabs = Instantiate(damageText, enemy.transform.position + 
                new Vector3(Random.Range(-0.6f, 0.6f),Random.Range(-0.3f, 0.3f), -1), Quaternion.identity);

            if (aimCheck <= StatsController.instance.critChance - 1)
                damageTextPrefabs.GetComponent<TextMesh>().fontSize *= 2;
            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(0, 80, 180, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = "0";

            if(enemyFullGuard <= 0)
            {
                enemyDef.value = ((StatsController.instance.enemyDef * enemyDef.value / 100) - damage) /
                StatsController.instance.enemyDef * 100;
                enemyDef.value = Mathf.Clamp(enemyDef.value, 0, 100);
            }
        }
        if (enemyFullGuard > 0 && fullGuardOn <= 0)
        {
            EnemyDamage(damage);
        }
        if (enemyDef.value > 0)
        {
            block.Play();
            StatsController.instance.enemyDef++;
        }
        EnemyDEFText.text = "Def: " + StatsController.instance.enemyDef;

        if (enemyHPBar.value <= 0)
        {
            enemyHPBar.value = 0;
            Instantiate(explosion, new Vector3(6, -1.35f, -1), Quaternion.identity);
            NextBattle();
            return;
        }
    }

    public void Defence()
    {
        if (defencePoint < 0)
        {
            defencePoint += 5;
            if (easyMode)
                defencePoint += 5;
        }
        if (defencePoint > 0)
            defencePoint = 0;

        GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(0, -3.3f, 2) +
                new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-0.5f, 0.5f), -1), Quaternion.identity);
        damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(0, 0, 210, 255);
        damageTextPrefabs.GetComponent<TextMesh>().text = "+";

        defenceBar.offsetMax = new Vector2(0, defencePoint);
    }

    public void Dodge()
    {
        if (dodgePoint < 0)
        {
            dodgePoint += 10;
            if(easyMode)
                dodgePoint += 10;
        }


        if (dodgePoint > 0)
            dodgePoint = 0;

        GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(5.3f, -3.3f, 2) +
                new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-0.5f, 0.5f), -1), Quaternion.identity);
        damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(0, 160, 0, 255);
        damageTextPrefabs.GetComponent<TextMesh>().text = "+";

        dodgeBar.offsetMax = new Vector2(0, dodgePoint);
    }

    public void Train()
    {
        trainPoint += 6;
        if (easyMode)
            trainPoint += 6;
        if (trainPoint >= 0)
        {
            trainPoint = -100;
            StatsController.instance.playerHP += 3 * (StatsController.instance.HpGainLv + 1);
            HPText.text = "HP: " + StatsController.instance.playerHP;
            StatsController.instance.playerAtk += (StatsController.instance.AtkGainLv + 1);
            ATKText.text = "Atk: " + StatsController.instance.playerAtk;
            statUp.Play();
        }

        GameObject damageTextPrefabs = Instantiate(damageText, new Vector3(-6.5f, -3.3f, 2) +
                new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-0.5f, 0.5f), -1), Quaternion.identity);
        damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 0, 0, 255);
        damageTextPrefabs.GetComponent<TextMesh>().text = "+";

        trainBar.offsetMax = new Vector2(0, trainPoint);
    }

    public void Gold()
    {
        goldPoint += 5 + (1 * StatsController.instance.coinsGainLv);
        if(easyMode)
            goldPoint += 5 + (1 * StatsController.instance.coinsGainLv);
        if (goldPoint >= 0)
        {
            goldPoint = -100;
            coins += Random.Range(10, 20) * (1 + StatsController.instance.goldPriceLv);
            coinText.text = "$ " + coins.ToString();
            buy.Play();
        }

        goldBar.offsetMax = new Vector2(0, goldPoint);
    }

    void NextBattle()
    {
        if (battle == lastCycle)
        {
            StatsController.instance.playerHP += (int)(StatsController.instance.pastPlayerHP / 5);
            StatsController.instance.playerAtk += (int)(StatsController.instance.pastPlayerAtk / 10);
            StatsController.instance.playerDef += (int)(StatsController.instance.pastPlayerDef / 5);
            StatsController.instance.playerAgi += (int)(StatsController.instance.pastPlayerAgi / 20);
            ATKText.text = "ATK: " + StatsController.instance.playerAtk;
            DEFText.text = "Def: " + StatsController.instance.playerDef;
            AGIText.text = "Agi: " + StatsController.instance.playerAgi;
        }
        else if (EnemyName.text == "You(Future)")
        {
            StatsController.instance.playerHP += (int)(StatsController.instance.pastPlayerHP / 5);
            StatsController.instance.playerAtk += (int)(StatsController.instance.pastPlayerAtk / 10);
            StatsController.instance.playerDef += (int)(StatsController.instance.pastPlayerDef / 5);
            StatsController.instance.playerAgi += (int)(StatsController.instance.pastPlayerAgi / 20);
            ATKText.text = "ATK: " + StatsController.instance.playerAtk;
            DEFText.text = "Def: " + StatsController.instance.playerDef;
            AGIText.text = "Agi: " + StatsController.instance.playerAgi;
        }
        else
        {
            coins += (int)(Random.Range(0.5f, 1.5f) * battle * 50 * (1 + StatsController.instance.coinsDropLv));
            if(tier >= 6)
                coins += (int)(Random.Range(0.5f, 1.5f) * battle * 50 * (tier - 5));
            coinText.text = "$ " + coins.ToString();
        }

        battle++;
        battleBuff++;
        if(battleBuff >= 11)
        {
            battleBuff = 1;
            tier++;
        }

        levelText.text = "Battle: " + battle.ToString();
        if(battle % 100 == 0)
            levelText.text += "(BOSS)";

        StatsController.instance.playerHP += 5 * (StatsController.instance.HpGainLv + 1);
        HPText.text = "HP: " + StatsController.instance.playerHP;
        playerHPBar.maxValue = StatsController.instance.playerHP;
        playerHPBar.value = StatsController.instance.playerHP;

        NextEnemy();

        EnemyHPText.text = "HP: " + StatsController.instance.enemyHP;
        EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
        EnemyDEFText.text = "Def: " + StatsController.instance.enemyDef;
        EnemyAGIText.text = "Agi: " + StatsController.instance.enemyAgi;

        enemyHPBar.maxValue = StatsController.instance.enemyHP;
        enemyHPBar.value = StatsController.instance.enemyHP;
        enemyHPBar.value = enemyHPBar.value;

        enemyAtk.value = 0;
        enemyDef.value = 0;
        enemyDodge.value = 0;

        if (EnemyName.text == "You(Future)")
        {
            StartCoroutine(DisableAnimation());
            youFutureIcon.SetActive(true);
            youPastIcon.SetActive(false);
            enemyBossIcon.SetActive(false);
            IconText.text = "You(Future) has appeared!";
        }
        if (EnemyName.text == "You(Past)")
        {
            StartCoroutine(DisableAnimation());
            youFutureIcon.SetActive(false);
            youPastIcon.SetActive(true);
            enemyBossIcon.SetActive(false);
            IconText.text = "You(Past) has appeared!";
        }
        if (battle % 100 == 0)
        {
            StartCoroutine(DisableAnimation());
            youFutureIcon.SetActive(false);
            youPastIcon.SetActive(false);
            enemyBossIcon.SetActive(true);
            IconText.text = "Enemy(Boss) has appeared!";
        }

        if (battle == 2 && dialogueIndex == 1)
        {
            dialogueIndex++;
            SpawnDialogue(1);
        }
        if(battle == 3 && meetFuture == false)
        {
            meetFuture = true;
            SpawnDialogue(2);
        }
        if (battle == 2 && meetFuture == true && meetPast == false)
        {
            meetPast = true;
            dialogueIndex = 5;
            SpawnDialogue(4);
        }

        if ( (battle == 6 && dialogueIndex == 5) || (battle == 11 && dialogueIndex == 6) || (battle == 21 && dialogueIndex == 7) || 
            (battle == 31 && dialogueIndex == 8) || (battle == 41 && dialogueIndex == 9) || (battle == 51 && dialogueIndex == 10) || 
            (battle == 61 && dialogueIndex == 11) || (battle == 71 && dialogueIndex == 12) || (battle == 81 && dialogueIndex == 13) || 
            (battle == 91 && dialogueIndex == 14) || (battle == 101 && dialogueIndex == 15))
        {
            SpawnDialogue(dialogueIndex);
            dialogueIndex++;
            if (dialogueIndex >= 16 && !timerText.gameObject.activeInHierarchy)
            {
                timerText.gameObject.SetActive(true);
                timerText.text = "Boss Beaten in " + mins + "minutes and " + seconds + "seconds.";
            }
        }
        if(battle == 201 && dialogueIndex == 16)
        {
            SpawnDialogue(dialogues.Length - 1);
            dialogueIndex++;
        }

        if (!trainButton.activeInHierarchy && dialogueIndex >= 6 || !shop.activeInHierarchy && dialogueIndex >= 6)
        {
            trainButton.SetActive(true);
            shopOn = true;
        }
        if (!page1.activeInHierarchy && dialogueIndex >= 7)
        {
            page1.SetActive(true);
        }
        if (!page2.activeInHierarchy && dialogueIndex >= 9)
        {
            page2.SetActive(true);
        }
        if (!ability.activeInHierarchy && dialogueIndex >= 11)
        {
            ability.SetActive(true);
        }

        if (shopOn)
            shop.SetActive(true);

        if (battle > maxBattle)
            maxBattle = battle;
        if (tier > maxTier)
            maxTier = tier;

        abilityUse = Random.Range(0, 8);
        abilityTime = 0;
        rageModeOn = -1;
        fullGuardOn = -1;
        smokeBombOn = -1;
        enemyRageMode = -1;
        enemyFullGuard = -1;
        enemySmokeBomb = -1;

        enemyRageUse = false;
        enemyGuardUse = false;
        enemySmokeUse = false;

        sheild.SetActive(false);
        enemySheild.SetActive(false);
        smoke.SetActive(false);
        enemySmoke.SetActive(false);

        reFillTime = 0;
        enemyReFillTime = 0;

        if (rageMode)
            RageModeUseButton.gameObject.SetActive(true);
        if (fullGuard)
            FullGuardUseButton.gameObject.SetActive(true);
        if (smokeBomb)
            SmokeBombUseButton.gameObject.SetActive(true);

        attackPoint = -100; defencePoint = -100; dodgePoint = -100;
        attackBar.offsetMax = new Vector2(0, attackPoint);

        if (defFill)
        {
            defencePoint = 0;
            if (StatsController.instance.pastPlayerDefFillLv == true && EnemyName.text == "You(Past)" ||
                EnemyName.text == "You(Future)")
                enemyDef.value = 100;
        }
        if (dodgeFill)
        {
            dodgePoint = 0;
            if (StatsController.instance.pastPlayerDefFillLv == true && EnemyName.text == "You(Past)" ||
                EnemyName.text == "You(Future)")
                enemyDodge.value = 100;
        }
        defenceBar.offsetMax = new Vector2(0, defencePoint);
        dodgeBar.offsetMax = new Vector2(0, dodgePoint);

        dodgeTime = 0; tankTime = 0; enemyDodgeTime = 0; enemyTankTime = 0;

        enemyMode = Random.Range(1, 4);
        DodgePos();
        CheckTier();

        fightPanel.SetActive(false);
        startPanel.SetActive(true);

        SaveGameData();
    }

    void DodgePos()
    {
        if (tier >= 9)
        {
            time = 0;
            enemyMode = 3;
        }
    }

    void CheckTier()
    {
        for (int i = 0; i < enemyTiers.Length; i++)
        {
            if (battle % 100 == 0)
                enemyTiers[i].SetActive(true);
            else if (i <= tier - 2 && i != enemyTiers.Length - 1 && 
                EnemyName.text != "You(Future)" && EnemyName.text != "You(Past)")
                enemyTiers[i].SetActive(true);
            else
                enemyTiers[i].SetActive(false);
        }
    }

    public void GetCoins()
    {
        if(shopOn == true && !fightPanel.activeInHierarchy)
        {
            clickTime++;
            coins++;
            coinText.text = "$ " + coins;
            buy.Play();
        }
        if (easterEgg == 0 && clickTime == 5)
        {
            easterEgg++;
            SpawnDialogue(17);
        }
    }

    void NextEnemy()
    {
        int rand = Random.Range(1, 101);

        if (battle == lastCycle)
        {
            StatsController.instance.PastPlayerStats();
            EnemyName.text = "You(Past)";
            pastPlayer.SetActive(true);
            StatsController.instance.enemySpeed = Random.Range(60, 71);
            return;
        }

        if (battle == 3 && meetFuture == false)
        {
            EnemyName.text = "You(Future)";
            pastPlayer.SetActive(true);
            StatsController.instance.enemyHP = StatsController.instance.playerHP;
            StatsController.instance.enemyAtk = StatsController.instance.playerAtk;
            StatsController.instance.enemyDef = StatsController.instance.playerDef;
            StatsController.instance.enemyAgi = StatsController.instance.playerAgi;
            StatsController.instance.enemySpeed = Random.Range(70, 91);

            for (int i = 0; i < 321; i++)
            {
                rand = Random.Range(1, 5);
                switch (rand)
                {
                    case 1:
                        StatsController.instance.enemyHP += 20;
                        break;
                    case 2:
                        StatsController.instance.enemyAtk += 3;
                        break;
                    case 3:
                        StatsController.instance.enemyDef += 5;
                        break;
                    case 4:
                        StatsController.instance.enemyAgi += 1;
                        break;
                }
            }
            return;
        }

        if (battle > 3 && cycle != 0 && rand <= 2 && battle != 100)
        {
            EnemyName.text = "You(Future)";
            pastPlayer.SetActive(true);
            StatsController.instance.enemyHP = StatsController.instance.playerHP;
            StatsController.instance.enemyAtk = StatsController.instance.playerAtk;
            StatsController.instance.enemyDef = StatsController.instance.playerDef;
            StatsController.instance.enemyAgi = StatsController.instance.playerAgi;
            StatsController.instance.enemySpeed = Random.Range(70, 91);

            for (int i = 0; i < maxBattle * maxTier; i++)
            {
                rand = Random.Range(1, 5);
                switch (rand)
                {
                    case 1:
                        StatsController.instance.enemyHP += 20;
                        break;
                    case 2:
                        StatsController.instance.enemyAtk += 3;
                        break;
                    case 3:
                        StatsController.instance.enemyDef += 5;
                        break;
                    case 4:
                        StatsController.instance.enemyAgi += 1;
                        break;
                }
            }
            return;
        }

        EnemyName.text = "Enemy";
        pastPlayer.SetActive(false);
        StatsController.instance.enemyHP = Random.Range(20, 41) + (battle + tier) * 10;
        StatsController.instance.enemyAtk = Random.Range(10, 16) + (battle + tier) * 3;
        StatsController.instance.enemyDef = Random.Range(20, 30) + (battle+ tier) * 5;
        StatsController.instance.enemyAgi = Random.Range(0, 2) + battle;
        StatsController.instance.enemySpeed = Random.Range(30, 51) + battle;
        if (tier >= 6)
            StatsController.instance.enemySpeed += battle;
        if (tier >= 7)
            StatsController.instance.enemySpeed += battle;
        if (tier >= 8)
            StatsController.instance.enemySpeed += battle;
        if (tier >= 9)
            StatsController.instance.enemySpeed += battle;
        if (tier >= 10)
            StatsController.instance.enemySpeed += battle * 2;

        for (int i = 0; i < battle * (5 + ((tier - 1) * 3)); i++)
        {
            rand = Random.Range(1, 5);
            switch (rand)
            {
                case 1:
                    StatsController.instance.enemyHP += 20;
                    if (tier >= 6)
                        StatsController.instance.enemyHP += 10 + tier;
                    if (tier >= 10)
                        StatsController.instance.enemyHP += 10 + tier;
                    break;
                case 2:
                    StatsController.instance.enemyAtk += 3;
                    if (tier >= 6)
                        StatsController.instance.enemyAtk += 2;
                    if (tier >= 7)
                        StatsController.instance.enemyAtk += 1;
                    if (tier >= 10)
                        StatsController.instance.enemyAtk += 2;
                    break;
                case 3:
                    StatsController.instance.enemyDef += 5;
                    if (tier >= 8)
                        StatsController.instance.enemyDef += 1;
                    break;
                case 4:
                    rand = Random.Range(1, 5);
                    if (rand == 1)
                        StatsController.instance.enemyAgi += 1;
                    break;
            }
        }

        if (battle != 100)
        {
            return;
        }

        StatsController.instance.enemyHP *= 3;
        StatsController.instance.enemyAtk += (int)((float)StatsController.instance.enemyAtk / 3);
    }

    void EnemyAI()
    {
        if (smokeBombOn > 0)
        {
            enemyTrueDodge = StatsController.instance.enemyAgi / 10;
            dodgeLoss = 72;
        }
        else
        {
            enemyTrueDodge = StatsController.instance.enemyAgi;
            dodgeLoss = 36;
        }

        if (enemyDodge.value > 0)
        {
            enemyDodge.value -= Time.deltaTime * dodgeLoss / (1 + enemyTrueDodge / 100);
            if (enemyDodge.value < 0)
                enemyDodge.value = 0;
        }

        time += Time.deltaTime;
        abilityTime += Time.deltaTime;
        if (time > 2 / (1 + StatsController.instance.enemySpeed / 1000))
        {
            time = 0;
            enemyMode = Random.Range(1, 4);
        }

        if (((enemyGuardUse == false && EnemyName.text == "You(Past)" &&
          StatsController.instance.pastPlayerFullGuardLv == true) ||
          (enemyGuardUse == false && EnemyName.text == "You(Future)" && fullGuard == true)) &&
          enemyHPBar.value <= enemyHPBar.maxValue / 3)
        {
            fullGuardSound.Play();
            enemyGuardUse = true;
            enemyFullGuard = 3;
        }

        if (abilityTime >= abilityUse)
        {
            switch (enemyMode)
            {
                case 1:
                    if ((enemyRageUse == false && EnemyName.text == "You(Past)" &&
                        StatsController.instance.pastPlayerRageModeLv == true) ||
                        (enemyRageUse == false && EnemyName.text == "You(Future)" && rageMode == true))
                    {
                        rageModeSound.Play();
                        enemyRageUse = true;
                        enemyRageMode = 3;
                        abilityTime = 0;
                        abilityUse = Random.Range(1, 8);
                    }
                    break;
                case 2:
                    if ((enemyGuardUse == false && EnemyName.text == "You(Past)" &&
                       StatsController.instance.pastPlayerFullGuardLv == true) ||
                       (enemyGuardUse == false && EnemyName.text == "You(Future)" && fullGuard == true))
                    {
                        fullGuardSound.Play();
                        enemyGuardUse = true;
                        enemyFullGuard = 3;
                        abilityTime = 0;
                        abilityUse = Random.Range(1, 8);
                    }
                    break;
                case 3:
                    if ((enemySmokeUse == false && EnemyName.text == "You(Past)" &&
                       StatsController.instance.pastPlayerSmokeBombLv == true) ||
                       (enemySmokeUse == false && EnemyName.text == "You(Future)" && smokeBomb == true))
                    {
                        smokeBombSound.Play();
                        enemySmokeUse = true;
                        dodgePoint /= 2;
                        dodgeBar.offsetMax = new Vector2(0, dodgePoint);
                        enemySmokeBomb = 3;
                        abilityTime = 0;
                        abilityUse = Random.Range(1, 8);
                    }
                    break;
            }
        }

        if(EnemyName.text != "Enemy" && shopOn == true)
        {
            if(EnemyName.text == "You(Future)")
            {
                if(StatsController.instance.autoAttackLv < 50)
                {
                    enemyAtk.value += Time.deltaTime * (15 + 5 * (StatsController.instance.autoAttackLv + Random.Range(0, 2)));
                }
                else
                    enemyAtk.value += Time.deltaTime * (15 + 5 * (StatsController.instance.autoAttackLv));
            }
            else
                enemyAtk.value += Time.deltaTime * (15 + 5 * StatsController.instance.pastPlayerAutoAttackLv);
            EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
            if (enemyAtk.value >= 100)
            {
                enemyAtk.value = 0;
                EnemyDamage((int)(StatsController.instance.enemyAtk * Random.Range(0.9f, 1.1f)));
            }
        }
        if (enemyRageMode > 0)
        {
            enemyMode = 1;
            enemyAtk.value += Time.deltaTime * StatsController.instance.enemySpeed * 3;
            enemyRageMode -= Time.deltaTime;
        }
        if (enemyFullGuard > 0)
        {
            enemySheild.SetActive(true);
            enemyFullGuard -= Time.deltaTime;
        }
        else
            enemySheild.SetActive(false);
        if (enemySmokeBomb > 0)
        {
            smoke.SetActive(true);
            enemySmokeBomb -= Time.deltaTime;
        }
        else
            smoke.SetActive(false);

        switch (enemyMode)
        {
            case 1:
                enemyAtk.value += Time.deltaTime * StatsController.instance.enemySpeed;
                EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
                if (enemyAtk.value >= 100)
                {
                    enemyAtk.value = 0;
                    if(tier >= 9)
                    {
                        enemyMode = Random.Range(1, 3);
                    }
                    EnemyDamage((int)(StatsController.instance.enemyAtk * Random.Range(0.9f, 1.1f)));
                }
                break;
            case 2:
                if (enemyDef.value == 100)
                {
                    enemyMode = Random.Range(1, 3);
                    if (enemyMode == 2)
                        enemyMode = 3;
                    return;
                }
                enemyDef.value += Time.deltaTime * StatsController.instance.enemySpeed / 2;
                if (enemyDef.value >= 100)
                {
                    enemyDef.value = 100;
                }
                break;
            case 3:
                if (StatsController.instance.enemySpeed - 5 < dodgeLoss)
                {
                    enemyMode = Random.Range(1, 3);
                    return;
                }
                enemyDodge.value += Time.deltaTime * StatsController.instance.enemySpeed;
                if (enemyDodge.value >= 100)
                {
                    enemyDodge.value = 100;
                }
                if (enemyDodge.value == 100)
                {
                    enemyMode = Random.Range(1, 3);
                }
                break;
        }

        if (enemyDef.value >= 1)
        {
            enemyTankTime += Time.deltaTime;
            if (enemyTankTime >= 3)
            {
                enemyTankTime = 0;
                StatsController.instance.enemyDef++;
                EnemyDEFText.text = "Def: " + StatsController.instance.enemyDef;
            }
        }
        if (enemyDodge.value >= 1)
        {
            enemyDodgeTime += Time.deltaTime;
            if (enemyDodgeTime >= 5)
            {
                enemyDodgeTime = 0;
                StatsController.instance.enemyAgi++;
                EnemyAGIText.text = "Agi: " + StatsController.instance.enemyAgi;
            }
        }
    }

    void EnemyDamage(int damage)
    {
        aimCheck = Random.Range(0, 100);

        float checkAim = (100f + dodgePoint) * (60f + (StatsController.instance.maxDodge / 2)) / 100f;
        if (smokeBombOn > 0)
            checkAim /= 2;
        if (aimCheck < checkAim)
        {
            dodge.Play();

            GameObject damageTextPrefabs = Instantiate(damageText, player.transform.position + 
                new Vector3(Random.Range(-0.6f, 0.6f),Random.Range(-0.3f, 0.3f), -1), Quaternion.identity);
            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(0, 180, 80, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = "Miss";
            return;
        }

        shake = 0.3f;

        if ((aimCheck <= StatsController.instance.pastPlayerCritChance - 1 && EnemyName.text == "You(Past)") ||
                (aimCheck <= StatsController.instance.critChance - Random.Range(-1, 3) && EnemyName.text != "You(Future)"))
            damage *= 2;
        StatsController.instance.enemyAtk++;
        if (battle % 100 == 0)
            StatsController.instance.enemyAtk += 29;
        EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
        float defence = 100 + defencePoint;
        if (damage > StatsController.instance.playerDef * defence / 100 && fullGuardOn <= 0)
        {
            hurt.Play();

            GameObject damageTextPrefabs = Instantiate(damageText, player.transform.position + 
                new Vector3(Random.Range(-0.6f, 0.6f),Random.Range(-0.3f, 0.3f), -1), Quaternion.identity);

            if ((aimCheck <= StatsController.instance.pastPlayerCritChance - 1 && EnemyName.text == "You(Past)") ||
                (aimCheck <= StatsController.instance.critChance - Random.Range(-1, 3) && EnemyName.text != "You(Future)"))
                damageTextPrefabs.GetComponent<TextMesh>().fontSize *= 2;

            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(180, 15, 0, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = (-(int)(damage - (StatsController.instance.playerDef * defence / 100))).ToString();
            playerHPBar.value -= damage - (StatsController.instance.playerDef * defence / 100);

            defencePoint = -100 + (StatsController.instance.playerDef * defence / 100 - damage) /
            StatsController.instance.playerDef * 100;
            defencePoint = Mathf.Clamp(defencePoint, -100, 0);

            if (reFillTime < StatsController.instance.defenceReFill)
            {
                reFillTime++;
                defencePoint = 0;
            }
        }
        else
        {
            GameObject damageTextPrefabs = Instantiate(damageText, player.transform.position + 
                new Vector3(Random.Range(-0.6f, 0.6f),Random.Range(-0.3f, 0.3f), -1), Quaternion.identity);

            if ((aimCheck <= StatsController.instance.pastPlayerCritChance - 1 && EnemyName.text == "You(Past)") ||
                (aimCheck <= StatsController.instance.critChance - Random.Range(-1, 3) && EnemyName.text != "You(Future)"))
                damageTextPrefabs.GetComponent<TextMesh>().fontSize *= 2;

            damageTextPrefabs.GetComponent<TextMesh>().color = new Color32(0, 80, 180, 255);
            damageTextPrefabs.GetComponent<TextMesh>().text = "0";

            if(fullGuardOn <= 0)
            {
                defencePoint = -100 + (StatsController.instance.playerDef * defence / 100 - damage) /
                StatsController.instance.playerDef * 100;
                defencePoint = Mathf.Clamp(defencePoint, -100, 0);
            }
        }
        if (fullGuardOn > 0 && enemyFullGuard <= 0)
        {
            PlayerDamage(damage);
        }

        if (defence > 0)
        {
            block.Play();
            StatsController.instance.playerDef += (StatsController.instance.DefGainLv + 1);
            DEFText.text = "Def: " + StatsController.instance.playerDef;
        }

        StatsController.instance.playerHP += 5 * (StatsController.instance.HpGainLv + 1);
        HPText.text = "HP: " + StatsController.instance.playerHP;
        
        defenceBar.offsetMax = new Vector2(0, defencePoint);

        if (playerHPBar.value <= 0)
        {
            playerHPBar.value = 0;
            if (battle > 2)
            {
                lastCycle = battle - 1;
                StatsController.instance.LastTimeline();

                StatsController.instance.pastPlayerDefFillLv = defFill;
                StatsController.instance.pastPlayerDodgeFillLv = dodgeFill;
            }

            fightPanel.SetActive(false);
            player.SetActive(false);

            gameOverPanel.SetActive(true);

            if (battle == 3 && meetFuture == false)
            {
                meetFuture = true;
            }
            if(meetPast)
            {
                if (easyMode)
                    SpawnDialogue(Random.Range(18, 28));
                else
                    SpawnDialogue(Random.Range(28, 35));
            }

            return;
        }
    }

    public void NewCycle()
    {
        battle = 1;
        battleBuff = 1;
        tier = 1;
        cycle++;
        if(cycle == 1)
        {
            SpawnDialogue(3);
        }

        sky.color = new Color32((byte)Random.Range(60, 150), (byte)Random.Range(60, 150), (byte)Random.Range(60, 150), 255);
        skyColor = sky.color;

        if (rageMode)
            RageModeUseButton.gameObject.SetActive(true);
        if (fullGuard)
            FullGuardUseButton.gameObject.SetActive(true);
        if (smokeBomb)
            SmokeBombUseButton.gameObject.SetActive(true);

        if (shopOn)
            shop.SetActive(true);

        reFillTime = 0;
        enemyReFillTime = 0;

        abilityUse = Random.Range(0, 10);
        abilityTime = 0;
        rageModeOn = -1;
        fullGuardOn = -1;
        smokeBombOn = -1;
        enemyRageMode = -1;
        enemyFullGuard = -1;
        enemySmokeBomb = -1;
        
        enemyRageUse = false;
        enemyGuardUse = false;
        enemySmokeUse = false;

        sheild.SetActive(false);
        enemySheild.SetActive(false);
        smoke.SetActive(false);
        enemySmoke.SetActive(false);

        EnemyName.text = "Enemy";
        pastPlayer.SetActive(false);
        
        levelText.text = "Battle: " + battle.ToString();
        cycleText.text = "Cycle: " + cycle.ToString();

        StatsController.instance.enemyHP = Random.Range(20, 41) + battle * 20;
        StatsController.instance.enemyAtk = Random.Range(10, 16) + battle * 5;
        StatsController.instance.enemyDef = Random.Range(20, 30) + battle * 3;
        StatsController.instance.enemyAgi = Random.Range(0, 2) + battle;
        StatsController.instance.enemySpeed = Random.Range(30, 51) + tier;

        for (int i = 0; i < battle * 5 * tier; i++)
        {
            int rand = Random.Range(1, 5);
            switch (rand)
            {
                case 1:
                    StatsController.instance.enemyHP += 10;
                    break;
                case 2:
                    StatsController.instance.enemyAtk += 3;
                    break;
                case 3:
                    StatsController.instance.enemyDef += 5;
                    break;
                case 4:
                    StatsController.instance.enemyAgi += 1;
                    break;
            }
        }

        EnemyHPText.text = "HP: " + StatsController.instance.enemyHP;
        EnemyATKText.text = "Atk: " + StatsController.instance.enemyAtk;
        EnemyDEFText.text = "Def: " + StatsController.instance.enemyDef;
        EnemyAGIText.text = "Agi: " + StatsController.instance.enemyAgi;

        playerHPBar.maxValue = StatsController.instance.playerHP;
        playerHPBar.value = StatsController.instance.playerHP;

        enemyHPBar.maxValue = StatsController.instance.enemyHP;
        enemyHPBar.value = StatsController.instance.enemyHP;

        enemyAtk.value = 0;
        enemyDef.value = 0;
        enemyDodge.value = 0;

        attackPoint = -100; defencePoint = -100; dodgePoint = -100;
        attackBar.offsetMax = new Vector2(0, attackPoint);

        if (defFill)
        {
            defencePoint = 0;
            StatsController.instance.pastPlayerDefFillLv = true;
        }
        if (dodgeFill)
        {
            dodgePoint = 0;
            StatsController.instance.pastPlayerDefFillLv = true;
        }

        if (rageMode)
            StatsController.instance.pastPlayerRageModeLv = true;
        if (fullGuard)
            StatsController.instance.pastPlayerFullGuardLv = true;
        if (smokeBomb)
            StatsController.instance.pastPlayerSmokeBombLv = true;

        defenceBar.offsetMax = new Vector2(0, defencePoint);
        dodgeBar.offsetMax = new Vector2(0, dodgePoint);

        CheckTier();

        player.SetActive(true);

        dodgeTime = 0; tankTime = 0; enemyDodgeTime = 0; enemyTankTime = 0;


        SaveGameData();
    }

    void SpawnDialogue(int dialogue)
    {
        if (appear.activeInHierarchy)
        {
            StartCoroutine(DelaySpawnDIalogue(dialogue));
            return;
        }
        dialogues[dialogue].SetActive(true);
    }

    IEnumerator DelaySpawnDIalogue(int dialogue)
    {
        yield return new WaitForSeconds(2);
        dialogues[dialogue].SetActive(true);
    }

    IEnumerator DisableAnimation()
    {
        appear.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        appear.SetActive(false);
    }

    public void EasyMode()
    {
        if (easyMode == false)
        {
            easyMode = true;
            easyModeText.text = "HardMode: Off";
        }
        else
        {
            easyMode = false;
            easyModeText.text = "HardMode: On";
        }
    }
}
