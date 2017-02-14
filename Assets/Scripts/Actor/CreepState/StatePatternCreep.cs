using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatePatternCreep : MonoBehaviour , IActorManager {
	
	private Dictionary<EnumCreepState, ICreepState> stateMap;

	private ICreepState currentState;

	private CreepModel creepModel;
	
	private IActorView actorView;

	private IActorMovement movement;

	private IActorManager next;
	
	[HideInInspector]
	public UnityEngine.AI.NavMeshAgent heroAgent;
	
	private void Awake()
	{
		currentState = null;
		stateMap = new Dictionary<EnumCreepState, ICreepState> ();
		UnityEngine.AI.NavMeshAgent agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent> ();	
		movement = new MoveNormally (agent);
		actorView = GetComponentInChildren<IActorView> ();
	}
	
	public void SetMovement(IActorMovement movement)
	{
		this.movement = movement;
	}
	
	public void Initialize(CreepModel creepModel)
	{
		this.creepModel = creepModel;

		stateMap.Add (EnumCreepState.Born, new CreepStateBorn (this));
		stateMap.Add (EnumCreepState.Stroll, new CreepStateStroll (this));
		stateMap.Add (EnumCreepState.Alert, new CreepStateAlert (this));
		stateMap.Add (EnumCreepState.Fight, new CreepStateFight (this));
		stateMap.Add (EnumCreepState.Hurt, new CreepStateHurt (this));
		stateMap.Add (EnumCreepState.Dead, new CreepStateDead (this));
		
		ToNextState (EnumCreepState.Born);
	}
	
	void Start () 
	{
		
	}
	
	void Update () 
	{
		if (null != currentState)
			currentState.Update (Time.deltaTime);
	}
	
	//	private void OnCollisionEnter(Collision collision)
	//	{
	//		//		currentState.OnCollisionEnter (collision);
	//	}
	
	private void OnTriggerEnter(Collider other)
	{
		currentState.OnTriggerEnter (other);
		GameObject colliderObject = other.gameObject;
		if (colliderObject.layer == Utils.GetAoeLayer ()) 
		{
			IAbilityAOE aoe = colliderObject.GetComponent<IAbilityAOE>();
			aoe.OnActorEnter(gameObject);
		}
	}
	
	public void ToNextState(EnumCreepState type)
	{
		ICreepState state;
		
		if (stateMap.TryGetValue (type, out state)) 
		{
			currentState = state;
			currentState.SetActive(true);
			actorView.SetMovement(movement);
		}
	}

	public IActorView GetActorView()
	{
		return actorView;
	}
	
	public IActorMovement GetMovement()
	{
		return movement;
	}
	
	public CreepModel GetModel()
	{
		return creepModel;
	}
	
	public string GetActorId()
	{
		return creepModel.actorId;
	}
	
	public void SetCamp(HeroCamp camp)
	{
//		creepModel.camp = camp;
	}
	
	public HeroCamp GetCamp()
	{
//		return creepModel.camp;
		return null;
	}
	
	public ECamp GetCampType()
	{
//		return creepModel.camp.GetCampType ();
		return creepModel.campType;
	}
	
	public EAttackResult BeenAttacted(IAbilityAOE buff)
	{
		creepModel.AddAbilityAOE (buff);
		return EAttackResult.Succeed;
	}
	
	public void AddHp(float hpIncrement)
	{
		creepModel.hp += hpIncrement;
	}
	
	public void DecHp(float hpDecrement)
	{
		creepModel.hp -= hpDecrement;
	}
	
	public void AddMp(float mpIncrement)
	{
		creepModel.mp += mpIncrement;
	}
	
	public void DecMp(float mpDecrement)
	{
		creepModel.mp -= mpDecrement;
	}
	
	public bool BeenRecruited(HeroCamp camp)
	{
		return false;
	}
	
	public void Died()
	{
		
	}
	
	public IActorManager GetNext()
	{
		return null;
	}
	
	public void SetNext(IActorManager next)
	{
//		this.next = next;
	}
	
	public IActorManager GetPrev()
	{
		return null;
	}
	
	public void SetPrev(IActorManager prev)
	{
		
	}
	
	public GameObject GetGameObject()
	{
		return gameObject;
	}

	public void Destroy()
	{
		GamePlayManager manager = Camera.main.GetComponent<GamePlayManager> ();
		manager.DestroyActor (gameObject);
	}
}
