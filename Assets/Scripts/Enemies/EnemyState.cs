using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public void OnEnter(EnemyStateController stateController);
    public void OnUpdate(EnemyStateController stateController);
    public void OnExit(EnemyStateController stateController);
}
