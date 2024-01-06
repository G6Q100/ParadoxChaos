using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
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

    public static StatsController instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void LastTimeline()
    {
        pastPlayerHP = playerHP;
        pastPlayerAtk = playerAtk;
        pastPlayerDef = playerDef;
        pastPlayerAgi = playerAgi;
        pastPlayerAutoAttackLv = autoAttackLv;
        pastPlayerCritChance = critChance;
        pastPlayerDefenceReFill = defenceReFill;
        pastPlayerMaxDodge = maxDodge;
    }

    public void PastPlayerStats()
    {
        enemyHP = pastPlayerHP;
        enemyAtk = pastPlayerAtk;
        enemyDef = pastPlayerDef;
        enemyAgi = pastPlayerAgi;
    }
}