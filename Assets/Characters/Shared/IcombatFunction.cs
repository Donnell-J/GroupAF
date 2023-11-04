using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IcombatFunction{
    // Start is called before the first frame update
    void getHit(int damage, bool ignoreShield);
    void heal(int heal);
    void applyStatus(string status, int amount);
    void defend(int amount);
    
}
