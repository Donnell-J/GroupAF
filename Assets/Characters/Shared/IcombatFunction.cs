using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IcombatFunction{
    void getHit(int damage, bool ignoreShield, string dmgType);
    void heal(int heal);
    void applyStatus(string status, int amount, bool isGood);
    void defend(int amount);
    void forceAction(int action);

    void removeStatuses();
    
}
