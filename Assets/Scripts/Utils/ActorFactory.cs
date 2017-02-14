using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorFactory {

	private static readonly string HeroPrototypeName = "Actor/Hero";
	private static readonly string CreepPrototypeName = "Actor/Creep";
	private static readonly string SoldierPrototypeName = "Actor/Soldier";

	private static readonly string HeroDataPath = "ScriptObject/ActorData/Hero";
	private static readonly string CreepDataPath = "ScriptObject/ActorData/Creep";
	private static readonly string SoldierDataPath = "ScriptObject/ActorData/Soldier";


	private static ActorFactory instance;
	
	private Dictionary<string, GameObject> prototypes; 

	private Dictionary<string, ScriptableObject> scriptableObjects;
	
	private ActorFactory()
	{
		prototypes = new Dictionary<string, GameObject> ();
		scriptableObjects = new Dictionary<string, ScriptableObject>();
	}
	
	public static ActorFactory GetInstance()
	{
		if (null == instance)
			instance = new ActorFactory ();
		return instance;
	}
	
	public GameObject CreateHero()
	{
		GameObject heroObject = CreateGameObject (HeroPrototypeName);
		StatePatternHero hero = heroObject.GetComponent<StatePatternHero> ();
		hero.Initialize (CreateHeroModel (HeroDataPath, hero.transform));
		return heroObject;
	}

	public GameObject CreateSoldier()
	{
		GameObject soldierObject = CreateGameObject (SoldierPrototypeName);
		StatePatternSoldier soldier = soldierObject.GetComponent<StatePatternSoldier> ();
		soldier.Initialize (CreatSoldierModel (SoldierDataPath, soldier.transform));
		return soldierObject;
	}

	public GameObject CreateCreep()
	{
		GameObject creepObject = CreateGameObject (CreepPrototypeName);
		StatePatternCreep creep = creepObject.GetComponent<StatePatternCreep> ();
		creep.Initialize (CreatCreepModel (CreepDataPath, creep.transform));
		return creepObject;
	}

	public CreepModel CreatCreepModel(string name, Transform transform)
	{
		ActorData data = GetScriptableObject<ActorData> (name);
		CreepModel model = new CreepModel (data);
		
		foreach(AbilityData abilityInfo in data.Abilities)
			model.AddAbility(CreateAbility(abilityInfo, transform));
		
		return model;
	}

	public SoldierModel CreatSoldierModel(string name, Transform transform)
	{
		ActorData data = GetScriptableObject<ActorData> (name);
		SoldierModel model = new SoldierModel (data);
		
		foreach(AbilityData abilityInfo in data.Abilities)
			model.AddAbility(CreateAbility(abilityInfo, transform));
		
		return model;
	}

	public HeroModel CreateHeroModel(string name, Transform transform)
	{
		ActorData data = GetScriptableObject<ActorData> (name);
		HeroModel model = new HeroModel (data);
		
		foreach(AbilityData abilityInfo in data.Abilities)
			model.AddAbility(CreateAbility(abilityInfo, transform));
		
		return model;
	}

	public IActiveAbility CreateAbility(AbilityData abilityInfo, Transform actorTransform)
	{
		switch (abilityInfo.SkillType) 
		{
		case AbilityData.Type.Gunshoot:
			GunshootAbility gunshootAbility = new GunshootAbility(abilityInfo, actorTransform);
			foreach(AbilityEffectData data in abilityInfo.effectInfos)
				gunshootAbility.AddEffect(CreateEffect(data));		
			return gunshootAbility;
		case AbilityData.Type.Melee:
			return null;
		default:
			return null;
		}
	}
	
	public IAbilityEffect CreateEffect(AbilityEffectData effectInfo)
	{
		switch (effectInfo.EffectType) 
		{
		case AbilityEffectData.Type.PlayEffectAnimation:
		case AbilityEffectData.Type.PlaySound:
		case AbilityEffectData.Type.TriggerSkill:
			return null;
		case AbilityEffectData.Type.TriggerAOE:
			return new AbilityEffectTriggerAOE (effectInfo);
		case AbilityEffectData.Type.PlayActorAnimation:
			return new PlayAnimation (effectInfo);
		default:
			return null;		
		}
	}

	public IAbilityAOE CreateAOE(string name)
	{
		GameObject gameObject = CreateGameObject (name);
		IAbilityAOE abilityAOE = gameObject.GetComponent<IAbilityAOE> ();
		return abilityAOE;
	}

	private GameObject CreateGameObject(string name)
	{
		return MonoBehaviour.Instantiate (GetPrototypeByName (name));
	}

	private T CreateScriptableObject<T>()where T : ScriptableObject
	{
		T scriptableObject = ScriptableObject.CreateInstance<T> ();
		return scriptableObject;
	}

	private T GetScriptableObject<T> (string name) where T : ScriptableObject
	{
		ScriptableObject sObject;
		if (scriptableObjects.TryGetValue (name, out sObject))
			return sObject as T;
		sObject = Resources.Load(name) as T;
		scriptableObjects.Add (name, sObject);
		return sObject as T;
	}

	private GameObject GetPrototypeByName(string name)
	{
		GameObject prototype;
		if (prototypes.TryGetValue (name, out prototype))
			return prototype;
		prototype = Resources.Load(name) as GameObject;
		prototypes.Add (name, prototype);
		return prototype;
	}
}
