using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataMenager : MonoBehaviour
{
    private void Awake() {
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
