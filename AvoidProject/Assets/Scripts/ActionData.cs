using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionSystem/NewAction")]
public class ActionData : ScriptableObject
{
    [SerializeField] public string aniName;
    [SerializeField] private string mecanimName;
    [SerializeField] private string nextNecanimName;

    [SerializeField] public float fxTime;       //이펙트가 나타나는 시간
    [SerializeField] public string layerName;   //메카님 레이어 설정
    [SerializeField] public float waitTime;     //기다리는 시간
    [SerializeField] public float nextWaitTime;
    [SerializeField] public GameObject fxObject;

    private AnimationClip eventClip;

    public AnimationClip EventClip
    {
        get { return this.eventClip; }
        set
        {
            this.eventClip = value;
            if (this.eventClip != null)
            {
                waitTime = eventClip.length;            //클립의 길이를 waitTime에 설정
                mecanimName = eventClip.name;           //클립의 이름을 mecanimName에 설정
            }
        }
    }
    //mecanimName을 외부에서 접근할 수 있게 추가
    public string MecanimName
    {
        get { return mecanimName; }
    }
 }

