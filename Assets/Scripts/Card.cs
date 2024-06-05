using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading;
public class Card : MonoBehaviour
{
    private Vector3 startPos;

    public int DamageToEnemy;
    public int ProtectToPlayer;
    public int ManaCost;
    public int ManaChangeNow;
    public int ManaChangeNext;
    public int StatusChange;

    public int DrawCardNum;
    public bool TripleFlag;
    public bool MemoryFlag;

    public bool RomoveTired;
    public bool RandStatus;
    // Start is called before the first frame update
    void Start()
    {
        // startPos = new Vector3(0.0f, 0.0f, 0.0f);
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Êó±ê½øÈë·¶Î§£¬¿¨ÅÆ±ä´ó
    public void Enter()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    // Êó±êÀë¿ª·µ»Ø£¬¿¨ÅÆ»Ö¸´
    public void Exit()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // ÍÏ×§ÒÆ¶¯
    public void Drag()
    {
        transform.position = Input.mousePosition;
        transform.SetParent(GameObject.Find("templecard").transform);
    }

    // ¿¨ÅÆ¸´Î»
    public void Up()
    {
        if(Vector3.Distance(transform.position, GameObject.Find("TableCenter").transform.position) < 200)
        {
            transform.position = GameObject.Find("TableCenter").transform.position;
            transform.SetParent(GameObject.Find("TableCenter").transform);
        }
        else
        {
            transform.position = startPos;
            transform.SetParent(GameObject.Find("cardhand").transform);
        }
    }

    public void Click()
    {

    }
}
