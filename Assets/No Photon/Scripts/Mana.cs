using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    public int MasterNowMana;
    public int NoMasterNowMana;
    public int MasterMaxMana;
    public int NoMasterMaxMana;
    public void StartManaSet()
    {
        MasterMaxMana = 3;
        NoMasterMaxMana = 3;
    }
}
