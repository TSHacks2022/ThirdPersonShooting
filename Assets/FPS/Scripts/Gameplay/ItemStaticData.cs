using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStaticData : ScriptableObject
{
    public static int potion = 0;
　　public static Dictionary<string, int> itemDictionary= new Dictionary<string, int>(){
    {"Potion", 0},
    {"HitPointPart", 100},
    {"AttackPart", 400},
    {"SpeedPart", 300},
    {"RapidPart", 100},
  };
  
}


