using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStaticData : ScriptableObject
{
    public static int potion = 0;
　　public static Dictionary<string, int> itemDictionary= new Dictionary<string, int>(){
    {"Potion", 3},
    {"HitPointPart", 0},
    {"AttackPart", 0},
    {"SpeedPart", 0},
    {"RapidPart", 0},
  };
  
}


