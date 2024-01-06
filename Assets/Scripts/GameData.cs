using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerHP;
    public int playerAtk;
    public int playerDef;
    public int playerAgi;

    public int autoCoinsLv;
    public int coinsGainLv;
    public int goldPriceLv;
    public int coinsDropLv;

    public int autoTrainLv;
    public int autoAttackLv;
    public int HpGainLv;
    public int AtkGainLv;
    public int DefGainLv;
    public int AgiGainLv;
    public int critChance;
    public int defenceReFill;
    public int maxDodge;

    public int enemyHP;
    public int enemyAtk;
    public int enemyDef;
    public int enemyAgi;
    public int enemySpeed;

    public int pastPlayerHP;
    public int pastPlayerAtk;
    public int pastPlayerDef;
    public int pastPlayerAgi;
    public int pastPlayerAutoAttackLv;
    public bool pastPlayerDefFillLv;
    public bool pastPlayerDodgeFillLv;
    public int pastPlayerCritChance;
    public int pastPlayerDefenceReFill;
    public int pastPlayerMaxDodge;
    public bool pastPlayerRageModeLv;
    public bool pastPlayerFullGuardLv;
    public bool pastPlayerSmokeBombLv;

    public int battle = 1, battleBuff = 1, tier = 1, cycle = 0, lastCycle, enemyMode, abilityUse;
    public int coins;

    public int autoCoinsPrice, coinsGainPrice, goldPrice, coinsDropPrice,
        autoTrainPrice, autoAttackPrice, DefGainPrice, AgiGainPrice,
        DefenceFillPrice, DodgeFillPrice,
        critChancePrice, defenceReFillPrice, maxDodgePrice, RageModePrice,
        FullGuardPrice, SmokeBombPrice;

    public bool autoOn = true, defFill, dodgeFill, shopOn = false,
        rageMode, fullGuard, smokeBomb;

    public int maxBattle, maxTier;

    public int dialogueIndex = 0;
    public int easterEgg = 0, buySkill = 0;

    public bool easyMode = true, meetFuture, meetPast;
    public float miniSecond;
    public int seconds, mins;

    public int[] skyColor;

    public string EnemyName;

    public GameData(ActionController gameData)
    {
        playerHP = StatsController.instance.playerHP;
        playerAtk = StatsController.instance.playerAtk;
        playerDef = StatsController.instance.playerDef;
        playerAgi = StatsController.instance.playerAgi;

        autoCoinsLv = StatsController.instance.autoCoinsLv;
        coinsGainLv = StatsController.instance.coinsGainLv;
        goldPriceLv = StatsController.instance.goldPriceLv;
        coinsDropLv = StatsController.instance.coinsDropLv;

        autoTrainLv = StatsController.instance.autoTrainLv;
        autoAttackLv = StatsController.instance.autoAttackLv;
        HpGainLv = StatsController.instance.HpGainLv;
        AtkGainLv = StatsController.instance.AtkGainLv;
        DefGainLv = StatsController.instance.DefGainLv;
        AgiGainLv = StatsController.instance.AgiGainLv;
        critChance = StatsController.instance.critChance;
        defenceReFill = StatsController.instance.defenceReFill;
        maxDodge = StatsController.instance.maxDodge;

        enemyHP = StatsController.instance.enemyHP;
        enemyAtk = StatsController.instance.enemyAtk;
        enemyDef = StatsController.instance.enemyDef;
        enemyAgi = StatsController.instance.enemyAgi;
        enemySpeed = StatsController.instance.enemySpeed;

        pastPlayerHP = StatsController.instance.pastPlayerHP;
        pastPlayerAtk = StatsController.instance.pastPlayerAtk;
        pastPlayerDef = StatsController.instance.pastPlayerDef;
        pastPlayerAgi = StatsController.instance.pastPlayerAgi;
        pastPlayerAutoAttackLv = StatsController.instance.pastPlayerAutoAttackLv;
        pastPlayerDefFillLv = StatsController.instance.pastPlayerDefFillLv;
        pastPlayerDodgeFillLv = StatsController.instance.pastPlayerDodgeFillLv;
        pastPlayerCritChance = StatsController.instance.pastPlayerCritChance;
        pastPlayerDefenceReFill = StatsController.instance.pastPlayerDefenceReFill;
        pastPlayerMaxDodge = StatsController.instance.pastPlayerMaxDodge;
        pastPlayerRageModeLv = StatsController.instance.pastPlayerRageModeLv;
        pastPlayerFullGuardLv = StatsController.instance.pastPlayerFullGuardLv;
        pastPlayerSmokeBombLv = StatsController.instance.pastPlayerSmokeBombLv;

        battle = gameData.battle;
        battleBuff = gameData.battleBuff;
        tier = gameData.tier;
        cycle = gameData.cycle;
        lastCycle = gameData.lastCycle;
        enemyMode = gameData.enemyMode;
        abilityUse = gameData.abilityUse;
        coins = gameData.coins;

        autoCoinsPrice = gameData.autoCoinsPrice;
        coinsGainPrice = gameData.coinsGainPrice;
        goldPrice = gameData.goldPrice;
        coinsDropPrice = gameData.coinsDropPrice;

        autoTrainPrice = gameData.autoTrainPrice;
        autoAttackPrice = gameData.autoAttackPrice;
        DefGainPrice = gameData.DefGainPrice;

        AgiGainPrice = gameData.AgiGainPrice;
        DefenceFillPrice = gameData.DefenceFillPrice;
        DodgeFillPrice = gameData.DodgeFillPrice;

        critChancePrice = gameData.critChancePrice;
        defenceReFillPrice = gameData.defenceReFillPrice;
        maxDodgePrice = gameData.maxDodgePrice;

        RageModePrice = gameData.RageModePrice;
        FullGuardPrice = gameData.FullGuardPrice;
        SmokeBombPrice = gameData.SmokeBombPrice;

        autoOn = gameData.autoOn;
        defFill = gameData.defFill;
        dodgeFill = gameData.dodgeFill;
        shopOn = gameData.shopOn;

        maxBattle = gameData.maxBattle;
        maxTier = gameData.maxTier;

        dialogueIndex = gameData.dialogueIndex;
        easterEgg = gameData.easterEgg;
        buySkill = gameData.buySkill;

        easyMode = gameData.easyMode;
        meetFuture = gameData.meetFuture;
        meetPast = gameData.meetPast;
        miniSecond = gameData.miniSecond;
        seconds = gameData.seconds;
        mins = gameData.mins;

        skyColor = new int[4];
        skyColor[0] = gameData.skyColor.r;
        skyColor[1] = gameData.skyColor.g;
        skyColor[2] = gameData.skyColor.b;
        skyColor[3] = gameData.skyColor.a;


        rageMode = gameData.rageMode;
        fullGuard = gameData.fullGuard;
        smokeBomb = gameData.smokeBomb;

        EnemyName = gameData.EnemyName.text;
    }
}
