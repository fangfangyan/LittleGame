using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatePatternHero : MonoBehaviour, IHeroControl, IActorManager {
	
	private Dictionary<EnumHeroState, IHeroState> stateMap;
	
	private IHeroState currentState;
	
	private HeroModel heroModel;

	private IActorView actorView;
	
	private IActorMovement movement;
	
	private IActorManager next;

	public static readonly float TimeForSkillCacheable = 1f;

	private void Awake()
	{
		currentState = null;
		stateMap = new Dictionary<EnumHeroState, IHeroState> ();
		UnityEngine.AI.NavMeshAgent agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent> ();
		movement = new MoveByControl (agent);
		actorView = GetComponentInChildren<IActorView> ();
	}

	public void Initialize(HeroModel heroModel)
	{
		this.heroModel = heroModel;

		stateMap.Add (EnumHeroState.Born, new HeroStateBorn (this));
		stateMap.Add (EnumHeroState.Stroll, new HeroStateStroll (this));
		stateMap.Add (EnumHeroState.Fight, new HeroStateFight (this));
		stateMap.Add (EnumHeroState.Hurt, new HeroStateHurt (this));
		stateMap.Add (EnumHeroState.Dead, new HeroStateDead (this));

		ToNextState (EnumHeroState.Born);
	}

	void Start () 
	{

	}

	void Update () 
	{
		if (null != currentState)
			currentState.Update (Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
//		currentState.OnCollisionEnter (collision);
	}

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

	public void ToNextState(EnumHeroState type)
	{
		IHeroState state;

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

	public HeroModel GetModel()
	{
		return heroModel;
	}
	
	//Pass the velocity from the instance of IHeroControl to the movement
	public void SetVelocity(Vector3 velocity){
		heroModel.velocity = velocity;
	}

	//Pass the velocity from the instance of IHeroControl to the movement
	public void SetDestination(Vector3 destination){
		heroModel.distination = destination;
	}
	
	public void TriggerAbility(IActiveAbility skill){
		if (null == skill)
			return;

		heroModel.releaseAbilities = skill;
		heroModel.releaseTimeLimit = Time.time + TimeForSkillCacheable;
	}
	
	public IActiveAbility[] GetAvailableSkill(){
		return heroModel.activeAbilities.ToArray ();
	}

	public string GetActorId()
	{
		return heroModel.actorId;
	}
	
	public void SetCamp(HeroCamp camp)
	{
		heroModel.camp = camp;
	}
	
	public HeroCamp GetCamp()
	{
		return heroModel.camp;
	}
	
	public ECamp GetCampType()
	{
		return heroModel.camp.GetCampType ();
	}
	
	public EAttackResult BeenAttacted(IAbilityAOE buff)
	{
		heroModel.AddAbilityAOE (buff);
		return EAttackResult.Succeed;
	}
	
	public void AddHp(float hpIncrement)
	{
		heroModel.hp += hpIncrement;
	}
	
	public void DecHp(float hpDecrement)
	{
		heroModel.hp -= hpDecrement;
	}
	
	public void AddMp(float mpIncrement)
	{
		heroModel.mp += mpIncrement;
	}
	
	public void DecMp(float mpDecrement)
	{
		heroModel.mp -= mpDecrement;
	}
	
	public bool BeenRecruited(HeroCamp camp)
	{
		return false;
	}
	
	public void Died()
	{
		currentState.ToNextState (EnumHeroState.Dead);
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

