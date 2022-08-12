using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//item static data
using Unity.FPS.Game;
using Unity.FPS.Gameplay;
public class SwitchMaterials : MonoBehaviour
{
    public static SwitchMaterials sm;
    public GameObject MaterialsPanel;
    public GameObject UsingMaterialsPanel;
    public GameObject M1;
    public GameObject M2;
    public GameObject M3;
    public GameObject M4;
    public GameObject UM1;
    public GameObject UM2;
    public GameObject UM3;
    public GameObject UM4;
    public TextMeshProUGUI M1text;
    public TextMeshProUGUI M2text;
    public TextMeshProUGUI M3text;
    public TextMeshProUGUI M4text;
    public TextMeshProUGUI UM1text;
    public TextMeshProUGUI UM2text;
    public TextMeshProUGUI UM3text;
    public TextMeshProUGUI UM4text;
    public TextMeshProUGUI ATK;
    public TextMeshProUGUI SPD;
    public TextMeshProUGUI RPD;
    public TextMeshProUGUI HP;
    public GameObject Done;
    public TextMeshProUGUI AATK;
    public TextMeshProUGUI ASPD;
    public TextMeshProUGUI ARPD;
    public TextMeshProUGUI AHP;
    public GameObject ResultPanel;
    public TextMeshProUGUI BATK;
    public TextMeshProUGUI BSPD;
    public TextMeshProUGUI BRPD;
    public TextMeshProUGUI BHP;
    public TextMeshProUGUI RATK;
    public TextMeshProUGUI RSPD;
    public TextMeshProUGUI RRPD;
    public TextMeshProUGUI RHP;
    public TextMeshProUGUI UCATK;
    public TextMeshProUGUI UCSPD;
    public TextMeshProUGUI UCRPD;
    public TextMeshProUGUI UCHP;
    
    //public GameObject 
    public  int M1stack,M2stack,M3stack, M4stack;
    public static int M1plus = 5, M2plus = 5, M3plus = 5,M4plus = 5;
    public int UM1stack = 0,UM2stack = 0, UM3stack = 0, UM4stack = 0;
    public static int  attack = 0,speed = 0, health = 0;
    public static int rapid = 0;

    void Start(){
        M1stack = ItemStaticData.itemDictionary["AttackPart"];
        M2stack = ItemStaticData.itemDictionary["SpeedPart"];
        M3stack = ItemStaticData.itemDictionary["RapidPart"];
        M4stack = ItemStaticData.itemDictionary["HitPointPart"];
        M1text.text = "Attack   x" + M1stack.ToString();
        M2text.text = "Speed   x" + M2stack.ToString();
        M3text.text = "Rapid   x" + M3stack.ToString();
        M4text.text = "HP   x" + M4stack.ToString();
        if(M1stack != 0) M1.SetActive(true);
        if(M2stack != 0) M2.SetActive(true);
        if(M3stack != 0) M3.SetActive(true);
        if(M4stack != 0) M4.SetActive(true);
        ATK.text = "ATK: +"+attack.ToString()+" -> ";
        SPD.text = "SPD: +"+speed.ToString()+" -> ";
        RPD.text = "RPD: +"+rapid.ToString()+" -> ";
        HP.text  = "HP:  +"+health.ToString()+" -> ";
        AATK.text = attack.ToString();
        ASPD.text = speed.ToString();
        ARPD.text = rapid.ToString();
        AHP.text = health.ToString();
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
        M1text.text = "Attack   x" + M1stack.ToString();
        UM1text.text = "Attack   x" + UM1stack.ToString();
        ATK.text = "ATK: +"+attack.ToString()+" -> ";
        AATK.text = (attack+UM1stack).ToString();
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
        M2text.text = "Speed   x" + M2stack.ToString();
        UM2text.text = "Speed   x" + UM2stack.ToString();
        SPD.text = "SPD: +"+speed.ToString()+" -> ";
        ASPD.text = (speed+UM2stack).ToString();
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
        M3text.text = "Rapid   x" + M3stack.ToString();
        UM3text.text = "Rapid   x" + UM3stack.ToString();
        RPD.text = "RPD:  +"+rapid.ToString()+" -> ";
        ARPD.text = (rapid+UM3stack).ToString();
    }

    public void SwitchM4(){
        if(EventSystem.current.currentSelectedGameObject == M4){
            M4stack--; UM4stack++;
            if(M4stack == 0){
                M4.SetActive(false);
            }
            if(UM4stack == 1){
                UM4.SetActive(true);
            }
        }else{
            M4stack++; UM4stack--;
            if(M4stack == 1){
                M4.SetActive(true);
            }
            if(UM4stack == 0){
                UM4.SetActive(false);
            }
        }
        M4text.text = "HP   x" + M4stack.ToString();
        UM4text.text = "HP   x" + UM4stack.ToString();
        HP.text = "HP:  +"+health.ToString()+" -> ";
        AHP.text = (health+UM4stack).ToString();
    }
    
    public void EnhancementDone(){
        if(UM1stack+UM2stack+UM3stack+UM4stack > 0){
            result();
            if(UM1stack >0){//attack
                for(int i=0;i<UM1stack;i++){
                    ProjectileUpgrade.Upgrade();
                }
            }
            if(UM2stack >0){//speed
                for(int i=0;i<UM2stack;i++){
                    ParameterUpgrade.SpeedUpgrade();
                }
            }
            if(UM3stack >0){//Rapid
                for(int i=0;i<UM3stack;i++){
                    WeaponUpgrade.Upgrade();
                }
            }
            if(UM4stack >0){
                for(int i=0;i<UM4stack;i++){
                    ParameterUpgrade.HealthUpgrade();
                }
            }
        }
        attack += UM1stack;
        speed += UM2stack;
        rapid += UM3stack;
        health += UM4stack;
        UM1stack = 0; UM2stack = 0; UM3stack = 0; UM4stack = 0;
        UM1.SetActive(false); UM2.SetActive(false); UM3.SetActive(false); UM4.SetActive(false);
        ATK.text = "ATK: +"+attack.ToString()+" -> ";
        SPD.text = "SPD: +"+speed.ToString()+" -> ";
        RPD.text = "RPD: +"+rapid.ToString()+" -> ";
        HP.text  = "HP:  +"+health.ToString()+" -> ";
        AATK.text = attack.ToString();
        ASPD.text = speed.ToString();
        ARPD.text = rapid.ToString();
        AHP.text = health.ToString();
        ItemStaticData.itemDictionary["AttackPart"] = M1stack;
        ItemStaticData.itemDictionary["SpeedPart"] = M2stack;
        ItemStaticData.itemDictionary["RapidPart"] = M3stack;
        ItemStaticData.itemDictionary["HitPointPart"] = M4stack;
    }
    
    public void result(){
        ResultPanel.SetActive(true);
        BATK.text = "ATK: +"+attack.ToString()+" -> ";
        BSPD.text = "SPD: +"+speed.ToString()+" -> ";
        BRPD.text = "RPD: +"+rapid.ToString()+" -> ";
        BHP.text = "HP:  +" +health.ToString()+" -> ";
        RATK.text = (attack+UM1stack).ToString();
        RSPD.text = (speed+UM2stack).ToString();
        RRPD.text = (rapid+UM3stack).ToString();
        RHP.text = (health+UM4stack).ToString();
    }

}
