using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform cardhand;
    public Transform table;
    public List<Card> CardHandList = new List<Card>();
    public List<Card> cardGroup;
    public List<Card> deck = new List<Card>();
    public List<Card> discard = new List<Card>();
    public Text deckSizeText;
    public Text discardSizeText;
    public Text ManaText;
    public Card currentcard;
    public Text playerHpText;
    public Text playerProtection;
    public Text enemyHpText;
    public Text enemyProtection;

    public Player player;
    public Enemy enemy;

    public Transform playerHpFill;
    public Transform enemyHpFill;

    private GameObject lackManaUI;
    private bool isDisplayingLackMana = false;
    // public Text LackMana;
    // Start is called before the first frame update
    void Start()
    {
        deck = cardGroup;
        for(int i = 1; i <= 4; i++)
        {
            DrawCard();
        }
        lackManaUI = GameObject.Find("LackMana");
    }

    // Update is called once per frame
    void Update()
    {
        int n = cardhand.childCount;
        cardhand.GetComponent<RectTransform>().anchoredPosition = new Vector2(-n * 40+100, -150);
        deckSizeText.text = deck.Count.ToString();
        discardSizeText.text = discard.Count.ToString();
        while (table.childCount > 0)
        {
            Transform card = table.GetChild(0);
            if (card.GetComponent<Card>().ManaCost > player.Mana)
            {
                if (!isDisplayingLackMana)
                {
                    StartCoroutine(DisplayLackManaUI());
                }
                card.transform.SetParent(cardhand);
                break;
            }
            CardApply(card.GetComponent<Card>(), player, enemy);
            card.SetParent(null);
            discard.Add(card.GetComponent<Card>());
        }
        ManaText.text = player.Mana.ToString() + "/" + player.ManaUpperBound.ToString();
        UpdatePlayerAndEnemy();
    }

    public void DrawCard()
    {
        if(deck.Count >= 1)
        {
            Card randCard = deck[UnityEngine.Random.Range(0, deck.Count)];
            currentcard = randCard;
            if(cardhand.childCount < 8)
            {
                CardHandList.Add(randCard);
                randCard.transform.SetParent(GameObject.Find("cardhand").transform);
                deck.Remove(randCard);
                return;
            }
        }
        else if(deck.Count == 0)
        {
            ShuffleCard();
            DrawCard();
        }
    }
    public void ShuffleCard()
    {
        deck = discard;
        discard = new List<Card>();
    }

    public void RoundEnd()
    {
        while (cardhand.childCount > 0)
        {
            Transform card = cardhand.GetChild(0);
            card.SetParent(null);
            CardHandList.Remove(card.GetComponent<Card>());
            discard.Add(card.GetComponent<Card>());
        }

        if(player.statusVal > 0)
        {
            player.statusVal--;
        }
        if(player.statusVal < 0)
        {
            player.statusVal++;
        }

        // TODO: Enemy action

        for (int i = 1; i <= 4; i++)
        {
            DrawCard();
        }

        player.Mana = player.ManaUpperBound + player.ManaChangeNext;
        player.ManaChangeNext = 0;

        if(!player.MemoryFlag)
        {
            player.protection = 0;
        }
    }

    public void CardApply(Card card, Player player, Enemy enemy)
    {
        player.Mana -= card.ManaCost;
        int c = player.multiple;
        float status = 1;
        if(player.statusVal > 0)
        {
            status = 1.5f;
        }
        else if(player.statusVal < 0)
        {
            status = 0.5f;
        }
        for(int i = 1; i <= c; i++)
        {
            enemy.HP -= (int)(card.DamageToEnemy * status);
            player.protection += card.ProtectToPlayer;
            player.Mana += card.ManaChangeNow;
            player.ManaChangeNext += card.ManaChangeNext;

            if (player.statusVal * card.StatusChange > 0)
            {
                player.statusVal += card.StatusChange;
            }
            else if(player.statusVal * card.StatusChange <= 0 && card.StatusChange != 0)
            {
                player.statusVal = card.StatusChange;
            }

            for (int j = 1; j <= card.DrawCardNum; j++)
            {
                DrawCard();
            }

            if (card.MemoryFlag)
            {
                player.MemoryFlag = true;
            }

            if (card.TripleFlag)
            {
                player.multiple = 3;
            }

            if (card.RomoveTired && player.statusVal < 0)
            {
                player.statusVal = 0;
            }

            if (card.RandStatus)
            {
                int change = 4 * UnityEngine.Random.Range(0, 2) - 2;
                if (player.statusVal * change > 0)
                {
                    player.statusVal += change;
                }
                else if(player.statusVal * change <= 0 && change != 0)
                {
                    player.statusVal = change;
                }
            }
        }

        if(c == 3 && card.TripleFlag == false)
        {
            player.multiple = 1;
        }
        

    }

    public void UpdatePlayerAndEnemy()
    {
        // 血量数字更新
        playerHpText.text = player.HP.ToString() + "/" + player.HPUpperBound.ToString();
        playerProtection.text = player.protection.ToString();

        enemyHpText.text = enemy.HP.ToString() + "/" + enemy.HPUpperBound.ToString();

        // 血条更新
        playerHpFill.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(577.0 * player.HP / player.HPUpperBound), playerHpFill.GetComponent<RectTransform>().sizeDelta.y);
        enemyHpFill.GetComponent<RectTransform>().sizeDelta = new Vector2((float)(570.0 * enemy.HP / enemy.HPUpperBound), enemyHpFill.GetComponent<RectTransform>().sizeDelta.y);


    }

    // 显示MANA不足
    IEnumerator DisplayLackManaUI()
    {
        isDisplayingLackMana = true;

        lackManaUI.transform.SetParent(GameObject.Find("FightUI").transform);
        lackManaUI.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 200.0f);
        yield return new WaitForSeconds(1f);
        lackManaUI.transform.SetParent(null);

        isDisplayingLackMana = false;
    }
}
