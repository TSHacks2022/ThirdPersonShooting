using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SwitchMaterials : MonoBehaviour
{
    public static SwitchMaterials sm;
    public GameObject MaterialsPanel;
    public GameObject UsingMaterialsPanel;
    public GameObject M1;
    public GameObject M2;
    public GameObject M3;
    public GameObject UM1;
    public GameObject UM2;
    public GameObject UM3;
    public TextMeshProUGUI M1text;
    public TextMeshProUGUI M2text;
    public TextMeshProUGUI M3text;
    public TextMeshProUGUI UM1text;
    public TextMeshProUGUI UM2text;
    public TextMeshProUGUI UM3text;
    public TextMeshProUGUI ATK;
    public TextMeshProUGUI ACC;
    public TextMeshProUGUI FR;
    public GameObject Done;
    public TextMeshProUGUI AATK;
    public TextMeshProUGUI AACC;
    public TextMeshProUGUI AFR;
    public GameObject ResultPanel;
    public TextMeshProUGUI BATK;
    public TextMeshProUGUI BACC;
    public TextMeshProUGUI BFR;
    public TextMeshProUGUI RATK;
    public TextMeshProUGUI RACC;
    public TextMeshProUGUI RFR;
    public TextMeshProUGUI UCATK;
    public TextMeshProUGUI UCACC;
    public TextMeshProUGUI UCFR;
    
    //public GameObject 
    public  int M1stack = 10,M2stack = 10,M3stack = 10;
    public static int M1plus = 5, M2plus = 5, M3plus = 5;
    public int UM1stack = 0,UM2stack = 0, UM3stack = 0;
    public int attack = 10, accuracy = 50, firerate = 16;

    void Start(){
        M1text.text = "M1   x" + M1stack.ToString();
        M2text.text = "M2   x" + M2stack.ToString();
        M3text.text = "M3   x" + M3stack.ToString();
        if(M1stack != 0) M1.SetActive(true);
        if(M2stack != 0) M2.SetActive(true);
        if(M3stack != 0) M3.SetActive(true);
        ATK.text = "ATK: "+attack.ToString()+"-> ";
        ACC.text = "ACC: "+accuracy.ToString()+"-> ";
        FR.text = "FR:  "+firerate.ToString()+"-> ";
        AATK.text = attack.ToString();
        AACC.text = accuracy.ToString();
        AFR.text = firerate.ToString();

    }

    public void SwitchM1(){
        if(EventSystem.current.currentSelectedGameObject == M1){
            M1stack--; UM1stack++;
            if(M1stack == 0){
                M1.SetActive(false);
            }
            if(UM1stack == 1){
                UM1.SetActive(true);
            }
        }else{
            M1stack++; UM1stack--;
            if(M1stack == 1){
                M1.SetActive(true);
            }
            if(UM1stack == 0){
                UM1.SetActive(false);
            }
        }
        M1text.text = "M1   x" + M1stack.ToString();
        UM1text.text = "M1   x" + UM1stack.ToString();
        ATK.text = "ATK: "+attack.ToString()+"-> ";
        AATK.text = (attack+M1plus*UM1stack).ToString();
    }

    public void SwitchM2(){
        if(EventSystem.current.currentSelectedGameObject == M2){
            M2stack--; UM2stack++;
            if(M2stack == 0){
                M2.SetActive(false);
            }
            if(UM2stack == 1){
                UM2.SetActive(true);
            }
        }else{
            M2stack++; UM2stack--;
            if(M2stack == 1){
                M2.SetActive(true);
            }
            if(UM2stack == 0){
                UM2.SetActive(false);
            }
        }
        M2text.text = "M2   x" + M2stack.ToString();
        UM2text.text = "M2   x" + UM2stack.ToString();
        ACC.text = "ACC: "+accuracy.ToString()+"-> ";
        AACC.text = (accuracy+M2plus*UM2stack).ToString();
    }

    public void SwitchM3(){
        if(EventSystem.current.currentSelectedGameObject == M3){
            M3stack--; UM3stack++;
            if(M3stack == 0){
                M3.SetActive(false);
            }
            if(UM3stack == 1){
                UM3.SetActive(true);
            }
        }else{
            M3stack++; UM3stack--;
            if(M3stack == 1){
                M3.SetActive(true);
            }
            if(UM3stack == 0){
                UM3.SetActive(false);
            }
        }
        M3text.text = "M3   x" + M3stack.ToString();
        UM3text.text = "M3   x" + UM3stack.ToString();
        FR.text = "FR:  "+firerate.ToString()+"-> ";
        AFR.text = (firerate+M3plus*UM3stack).ToString();
    }
    
    public void EnhancementDone(){
        if(UM1stack+UM2stack+UM3stack > 0){
            result();
        }
        attack += M1plus*UM1stack;
        accuracy += M2plus*UM2stack;
        firerate += M3plus*UM3stack;
        UM1stack = 0; UM2stack = 0; UM3stack = 0;
        UM1.SetActive(false); UM2.SetActive(false); UM3.SetActive(false);
        ATK.text = "ATK: "+attack.ToString()+"-> ";
        ACC.text = "ACC: "+accuracy.ToString()+"-> ";
        FR.text = "FR:  "+firerate.ToString()+"-> ";
        AATK.text = attack.ToString();
        AACC.text = accuracy.ToString();
        AFR.text = firerate.ToString();
    }
    
    public void result(){
        ResultPanel.SetActive(true);
        BATK.text = "ATK: "+attack.ToString()+"-> ";
        BACC.text = "ACC: "+accuracy.ToString()+"-> ";
        BFR.text = "FR:  "+firerate.ToString()+"-> ";
        RATK.text = (attack+M1plus*UM1stack).ToString();
        RACC.text = (accuracy+M2plus*UM2stack).ToString();
        RFR.text = (firerate+M3plus*UM3stack).ToString();
    }
}
