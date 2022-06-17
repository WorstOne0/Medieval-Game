using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {
    public Transform flagHold;
    public Flag startFlag;
    public bool isFlagEquipped = false;
    Flag equipedFlag;

    public void EquipFlag() {
        if (!isFlagEquipped) {
            equipedFlag = Instantiate(startFlag, flagHold.position, flagHold.rotation) as Flag;
            equipedFlag.transform.parent = flagHold;
            isFlagEquipped = true;
        }
    }

    public void UnequipFlag() {
        if (isFlagEquipped) {
            Destroy(equipedFlag.gameObject);
            isFlagEquipped = false;
        }
    }
}
