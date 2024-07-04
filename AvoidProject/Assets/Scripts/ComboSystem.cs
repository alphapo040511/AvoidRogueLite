using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    public ComboAction comboAction;                 //스크립터블 오브젝트 콤보 데이터
    public int currentActionIndex = 0;              //액션 인덱스 설정
    public float lastActionTime;                    //액션 시간

    public ActionData PerformAction (Animator animator)
    {
        if(comboAction != null && currentActionIndex < comboAction.actions.Count)
        {
            if(Time.time - lastActionTime > 2.0f)   //2초 이상 시간이 지났는지 검사
            {
                Debug.Log("콤보 시간이 초과 돼서 리셋 되었습니다.");
                currentActionIndex = 0;
            }

            ActionData currentAction = comboAction.actions[currentActionIndex];

            lastActionTime = Time.time;             //액션 실행 시간 갱신
            currentActionIndex++;                   //다음 액션으로 이동

            if(currentActionIndex >= comboAction.actions.Count)     //마지막 액션 실행 후 체크하여 인덱스 초기화
            {
                Debug.Log("콤보 완료");
                currentActionIndex = 0;
            }
            return currentAction;
        }
        return null;
    }
}
