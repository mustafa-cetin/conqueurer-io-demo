using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Castle : MonoBehaviour
{
    public int startMinGarrison, startMaxGarrison;
    public Lord lord;
    public Lord Conquerer=null;
    public int garrison;
    private SpriteRenderer spriteRenderer;
    public TextMeshProUGUI textComp;
    public bool underAttack;
    void Start()
    {
    spriteRenderer=GetComponent<SpriteRenderer>();
    GarrisonUpdate(Random.Range(startMinGarrison,startMaxGarrison));
    
    }
    // Update is called once per frame
    void Update()
    {
    if (lord!=null)
    {
        
    spriteRenderer.color=lord.lordsColor;
    }
    }
    public void GarrisonUpdate(int number){
        
        garrison=number;
        textComp.text=garrison.ToString();
    }
}
