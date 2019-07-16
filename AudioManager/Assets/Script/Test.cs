using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    [SerializeField]
    private Dropdown dd;

    public void PlayBG()
    {
        string bgName = dd.options[dd.value].text;
        AudioBackGroundMgr.Instance.Play(bgName);
    }

    public void PlayEffect()
    {
        AudioEffectMgr.Instance.PlayUI();
    }
}
