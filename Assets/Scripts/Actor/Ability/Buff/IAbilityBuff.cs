using UnityEngine;
using System.Collections;

public interface IAbilityBuff : ISortable{

	string GetBuffId();

	ActorBuffType GetType();

	float GetEffectValue();

	float GetLimitTime();

}
