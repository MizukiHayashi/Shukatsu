using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEffectScript : MonoBehaviour
{
    [SerializeField]
    private GameObject panelEffect;

    //パネルをアクティブに
    public void PanelEffect_ON()
    {
        panelEffect.SetActive(true);
    }
    //パネルを非アクティブに
    public void PanelEffect_OFF()
    {
        panelEffect.SetActive(false);
    }
}
