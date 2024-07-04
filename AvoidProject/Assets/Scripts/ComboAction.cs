using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ActionSystem/ComboAction")]
public class ComboAction : ScriptableObject
{
    public List<ActionData> actions = new List<ActionData>();
    public float comboResetTime = 2.0f;                         //콤보가 리셋 되는 시간

    //다음 액션으로 넘어갈 수 있는지 체크
    public ActionData GetNextAction(int currentActionIndex)
    {
        if(currentActionIndex < actions.Count - 1)
        {
            return actions[currentActionIndex + 1];
        }

        return null;
    }
}
