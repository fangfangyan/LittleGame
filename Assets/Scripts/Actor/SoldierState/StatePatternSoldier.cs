using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatePatternSoldier : MonoBehaviour , IActorManager {
	
	private Dictionary<EnumSoldierState, ISoldierState> stateMap;

	private ISoldierState currentState;

	private SoldierModel soldierModel;


	private IActorMovement movement;

	private IActorView actorView;

	private IActorManager next;
	
	private IActorManager prev;
	
	public UnityEngine.AI.NavMeshAgent heroAgent;
	
	private void Awake()
	{
		currentState = null;
		stateMap = new Dictionary<EnumSoldierState, ISoldierState> ();
//		NavMeshAgent agent = GetComponentInParent<NavMeshAgent> ();	
//		movement = new MoveFollowHero (this);
		actorView = GetComponentInChildren<IActorView> ();
	}

	public void SetMovement(IActorMovement movement)
	{
		this.movement = movement;
	}

	public void Initialize(SoldierModel soldierModel)
	{
		this.soldierModel = soldierModel;

		stateMap.Add (EnumSoldierState.Born, new SoldierStateBorn (this));
		stateMap.Add (EnumSoldierState.Stand, new SoldierStateStand (this));
		stateMap.Add (EnumSoldierState.Stroll, new SoldierStateStroll (this));
		stateMap.Add (EnumSoldierState.Fight, new SoldierStateFight (this));
		stateMap.Add (EnumSoldierState.Hurt, new SoldierStateHurt (this));
		stateMap.Add (EnumSoldierState.Dead, new SoldierStateDead (this));
		
		ToNextState (EnumSoldierState.Stand);
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
	
	public void ToNextState(EnumSoldierState type)
	{
		ISoldierState state;
		
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
	
	public SoldierModel GetModel()
	{
		return soldierModel;
	}
	
	public string GetActorId()
	{
		return soldierModel.actorId;
	}
	
	public void SetCamp(HeroCamp camp)
	{
		soldierModel.camp = camp;
	}
	
	public HeroCamp GetCamp()
	{
		return soldierModel.camp;
	}
	
	public ECamp GetCampType()
	{
		return soldierModel.camp.GetCampType ();
	}
	
	public EAttackResult BeenAttacted(IAbilityAOE buff)
	{
		soldierModel.AddAbilityAOE (buff);
		return EAttackResult.Succeed;
	}
	
	public void AddHp(float hpIncrement)
	{
		soldierModel.hp += hpIncrement;
	}
	
	public void DecHp(float hpDecrement)
	{
		soldierModel.hp -= hpDecrement;
	}
	
	public void AddMp(float mpIncrement)
	{
		soldierModel.mp += mpIncrement;
	}
	
	public void DecMp(float mpDecrement)
	{
		soldierModel.mp -= mpDecrement;
	}
	
	public bool BeenRecruited(HeroCamp camp)
	{
		if (null != GetCamp())
			return false;

		GameObject hero = camp.GetHero ().GetGameObject ();

		heroAgent = hero.GetComponent<UnityEngine.AI.NavMeshAgent> ();

		foreach(Transform tran in GetComponentsInChildren<Transform>()){ 
			tran.gameObject.layer = hero.layer;
		} 

		SetCamp (camp);

		movement = new MoveFollowHero (this);

		ToNextState (EnumSoldierState.Stroll);

		return true;
	}
	
	public void Died()
	{
		currentState.ToNextState (EnumSoldierState.Dead);
	}
	
	public IActorManager GetNext()
	{
		return next;
	}
	
	public void SetNext(IActorManager next)
	{
		this.next = next;
	}
	
	public IActorManager GetPrev()
	{
		return prev;
	}
	
	public void SetPrev(IActorManager prev)
	{
		this.prev = prev;
	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}

	public void Destroy()
	{
		GetCamp ().RemoveSoldier (this);
		GamePlayManager manager = Camera.main.GetComponent<GamePlayManager> ();
		manager.DestroyActor (gameObject);
	}
}
