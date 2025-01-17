using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN1, PLAYERTURN2, ENEMYTURN1, ENEMYTURN2, ENDOFQUEUE, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    #region "Start"
    #region "Variables"
    public Renderer playerGO1Renderer;
    public Renderer playerGO2Renderer;
    public Renderer enemyGO1Renderer;
    public Renderer enemyGO2Renderer;

    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;

    public Transform playerBattleStation1;
    public Transform playerBattleStation2;
    public Transform enemyBattleStation1;
    public Transform enemyBattleStation2;

    Unit playerUnit1;
    Unit playerUnit2;
    Unit enemyUnit1;
    Unit enemyUnit2;

    public Text dialogueText;

    public BattleHUD playerHUD1;
    public BattleHUD playerHUD2;
    public BattleHUD enemyHUD1;
    public BattleHUD enemyHUD2;

    public BattleState state;

    Queue<BattleState> TurnOrder;

    public GameObject StartPanel;
    public GameObject AttackPanel;
    public GameObject choicePanel;
    public Text Choice1Text;
    public Text Choice2Text;

    public GameObject UI;

    public PlayerManager PlayerManager;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;

        GameObject playerGO1 = Instantiate(playerPrefab1, playerBattleStation1);
        playerUnit1 = playerGO1.GetComponent<Unit>();
        playerGO1Renderer = playerGO1.GetComponent<Renderer>();

        GameObject playerGO2 = Instantiate(playerPrefab2, playerBattleStation2);
        playerUnit2 = playerGO2.GetComponent<Unit>();
        playerGO2Renderer = playerGO2.GetComponent<Renderer>();

        GameObject enemyGO1 = Instantiate(enemyPrefab2, enemyBattleStation1);
        enemyUnit1 = enemyGO1.GetComponent<Unit>();
        enemyGO1Renderer = enemyGO1.GetComponent<Renderer>();

        GameObject enemyGO2 = Instantiate(enemyPrefab2, enemyBattleStation2);
        enemyUnit2 = enemyGO2.GetComponent<Unit>();
        enemyGO2Renderer = enemyGO2.GetComponent<Renderer>();

        playerHUD1.SetHUD(playerUnit1);
        playerHUD2.SetHUD(playerUnit2);
        enemyHUD1.SetHUD(enemyUnit1);
        enemyHUD2.SetHUD(enemyUnit2);
    }

    IEnumerator SetupBattle()
    {
        UI.SetActive(true);

        string temp = "";

        try
        {
             temp = "A wild " + enemyUnit1.SO_StatsBlock.unitName + " approaches...";
        }
        catch (NullReferenceException)
        {
            temp = "An unknown Approaches  approaches...";
        }

        dialogueText.text = temp;

        yield return new WaitForSeconds(2f);

        TurnOrder = new Queue<BattleState>();
        GetNewTurns();
        StateMachineHandling();
    }

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    #endregion

    #region "Turn Handling"

    void StateMachineHandling()
    {
        if ((state == BattleState.WON) | (state == BattleState.LOST))
        {
            EndBattle();
        }
        else
        {

            state = TurnOrder.Dequeue();

            if (state == BattleState.ENEMYTURN1)
            {
                StartCoroutine(EnemyTurn());
            }
            else if (state == BattleState.ENEMYTURN2)
            {
                StartCoroutine(EnemyTurn());
            }
            else if (state == BattleState.PLAYERTURN1)
            {
                PlayerTurn();
            }
            else if (state == BattleState.PLAYERTURN2)
            {
                PlayerTurn();
            }
            else if ((state == BattleState.WON) | (state == BattleState.LOST))
            {
                EndBattle();
            }
            else if (state == BattleState.ENDOFQUEUE)
            {
                GetNewTurns();
                StateMachineHandling();
            }
        }

    }

    void GetNewTurns()
    {
        TurnOrder.Clear();

        if ((!playerUnit1.isAlive) & (!playerUnit2.isAlive))
        {
            TurnOrder.Enqueue(BattleState.LOST);
        }

        if ((!enemyUnit1.isAlive) & (!enemyUnit2.isAlive))
        {
            TurnOrder.Enqueue(BattleState.WON);
        }

        if (playerUnit1.isAlive)
        {
            TurnOrder.Enqueue(BattleState.PLAYERTURN1);
        }

        if (playerUnit2.isAlive)
        {
            TurnOrder.Enqueue(BattleState.PLAYERTURN2);
        }

        if (enemyUnit1.isAlive)
        {
            TurnOrder.Enqueue(BattleState.ENEMYTURN1);
        }
        if (enemyUnit2.isAlive)
        {
            TurnOrder.Enqueue(BattleState.ENEMYTURN2);
        }

        TurnOrder.Enqueue(BattleState.ENDOFQUEUE);
    }

    void RemoveDefeated(BattleState defeated)
    {
        TurnOrder = new Queue<BattleState>(TurnOrder.Where(x => x != defeated));
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }

        // wait a little or ask for prompt, then end.
        UI.SetActive(false);
        PlayerManager.EndBattle();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    #endregion

    #region "Enemy Actions"

    IEnumerator EnemyTurn()
    {

        dialogueText.text = enemyUnit1.SO_StatsBlock.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isAlive = UnitAttack(enemyUnit1, playerUnit1, playerHUD1);

        StartCoroutine(Flasher(playerUnit1, playerGO1Renderer, Color.gray));

        if (!isAlive)
        {
            state = BattleState.LOST;
        }
        StateMachineHandling();
    }

    #endregion

    #region "Player Actions"
    public void PlayerAttack()
    {

        if (enemyUnit1.isAlive)
        {
            bool isAlive = UnitAttack(playerUnit1, enemyUnit1, enemyHUD1);

            StartCoroutine(Flasher(enemyUnit1, enemyGO1Renderer, Color.gray));

            if (!isAlive)
            {
                RemoveDefeated(BattleState.ENEMYTURN1);
            }

        }
        else if (enemyUnit2.isAlive)
        {
            bool isAlive = UnitAttack(playerUnit1, enemyUnit2, enemyHUD2);

            StartCoroutine(Flasher(enemyUnit2, enemyGO2Renderer, Color.gray));

            if (!isAlive)
            {
                RemoveDefeated(BattleState.ENEMYTURN2);
            }
        }

        StateMachineHandling();
    }

    public void PlayerAttack(int unitNumber)
    {

        if (unitNumber == 1)
        {
            bool isAlive = UnitAttack(playerUnit1, enemyUnit1, enemyHUD1);

            StartCoroutine(Flasher(enemyUnit1, enemyGO1Renderer, Color.gray));

            if (!isAlive)
            {
                RemoveDefeated(BattleState.ENEMYTURN1);
            }

        }
        else if (unitNumber == 2)
        {
            bool isAlive = UnitAttack(playerUnit1, enemyUnit2, enemyHUD2);

            StartCoroutine(Flasher(enemyUnit2, enemyGO2Renderer, Color.gray));

            if (!isAlive)
            {
                RemoveDefeated(BattleState.ENEMYTURN2);
            }
        }

        StateMachineHandling();
    }

    private bool UnitAttack(Unit Attacker, Unit Defender, BattleHUD DefenderHUD)
    {
        bool isAlive = Defender.TakeDamage(Attacker.SO_StatsBlock.baseDamage);

        DefenderHUD.SetHP(Defender.currentHP);
        dialogueText.text = Attacker.SO_StatsBlock.unitName + " attack is successful!";

        return isAlive;
    }

    public void PlayerHeal()
    {
        HealAction(playerUnit1, playerHUD1);

        StartCoroutine(Flasher(enemyUnit1, enemyGO1Renderer, Color.green));

        StateMachineHandling();
    }

    private void HealAction(Unit triggeringUnit, BattleHUD unitHUD)
    {
        triggeringUnit.Heal(5);

        unitHUD.SetHP(triggeringUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";
    }

    #endregion

    #region "UI"

    IEnumerator Flasher(Unit damagedUnit, Renderer renderer, Color flashColor)
    {
        for (int i = 0; i < 2; i++)
        {
            renderer.material.color = flashColor;
            yield return new WaitForSeconds(.1f);
            renderer.material.color = damagedUnit.unitColor;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void OnFightButton()
    {
        StartPanel.SetActive(false);
        AttackPanel.SetActive(true);
    }

    public void OnAttackButton()
    {
        if ((state != BattleState.PLAYERTURN1) & (state != BattleState.PLAYERTURN2))
            return;

        if ((enemyUnit1.isAlive) & (enemyUnit2.isAlive))
        {
            // activate choice buttons and fill in with defenders.

            // deactivate old 
            AttackPanel.SetActive(false);

            // set choice text
            dialogueText.text = "Choose unit to attack:";
            Choice1Text.text = enemyUnit1.SO_StatsBlock.unitName;
            Choice2Text.text = enemyUnit2.SO_StatsBlock.unitName;

            // activate choices
            choicePanel.SetActive(true);
        }
        else if (enemyUnit1.isAlive)
        {
            PlayerAttack(1);
            AttackPanel.SetActive(false);
            StartPanel.SetActive(true);
        }
        else if (enemyUnit2.isAlive)
        {
            PlayerAttack(2);
            AttackPanel.SetActive(false);
            StartPanel.SetActive(true);
        }
    }

    public void OnHealButton()
    {
        if ((state != BattleState.PLAYERTURN1) & (state != BattleState.PLAYERTURN2))
            return;

        PlayerHeal();
    }

    public void OnChoice1Button()
    {
        if ((state != BattleState.PLAYERTURN1) & (state != BattleState.PLAYERTURN2))
            return;

        PlayerAttack(1);
        choicePanel.SetActive(false);
        StartPanel.SetActive(true);
    }

    public void OnChoice2Button()
    {
        if ((state != BattleState.PLAYERTURN1) & (state != BattleState.PLAYERTURN2))
            return;

        PlayerAttack(2);
        choicePanel.SetActive(false);
        StartPanel.SetActive(true);
    }

    #endregion
}