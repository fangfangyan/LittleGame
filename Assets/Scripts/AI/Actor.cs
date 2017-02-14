using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Actor : MonoBehaviour {

//	public enum Camp
//	{
//		Friendly, 
//		Enemy, 
//		Creep,
//		Soldier
//	}
//
//	public enum State
//	{
//		Stand,
//		Move,
//		Hurt,
//		Die,
//		SpecialSkill
//	}
//
//	public enum Movement 
//	{
//		Standstill,
//		Navigation,
//		Follow,
//		Normal
//	}
//
//	//
//	protected Camp camp;
//	//
//	protected List<Skill> skills;
//	//
//	protected Skill currentSkill;
//	//
//	protected State state;
//
//	protected int hp;
//
//	private NavMeshAgent agent;
//
//	private ActorController header;
//
//	private Movement movement;
//
////	private int sequenceInQueue;
//	
//	// Use this for initialization
//	void Start () {
//		agent = GetComponent<NavMeshAgent> ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//	}
//
//	void LaterUpdate() {
//
//	}
//
//	public void SetPosition(Vector3 position){
//		if (movement == Movement.Navigation)
//			agent.SetDestination (position);
//		else
//			agent.transform.localPosition = position;
//	}
//
//	public Vector3 GetPosition()
//	{
//		return agent.transform.localPosition;
//	}
//
//	//set actor’s movement follow the previous
//	public void FollowTheActor(ActorController header)
//	{
//		header.AddSoldier (this);
//		this.header = header;
//		movement = Movement.Follow;
//	}
//	
//	//set actor’s movement auto navigation
//	public void SetAutoNav()
//	{
//		movement = Movement.Navigation;
//	}
//	
//	//set actor’s velocity
//	public void SetVelocity(Vector3 velocity)
//	{
//		movement = Movement.Normal;
//		agent.velocity = velocity;
//	}
//	
//	public void LaunchSkill(string skillId)
//	{
//		
//	}
//	
//	public void AddDamage(int damage)
//	{
//		
//	}
//	
//	public void SetHP(int hp)
//	{
//		
//	}
//	
//	public void SetActorHurt()
//	{
//		
//	}
//	
//	public void SetActorDie()
//	{
//		
//	}
//
//	public int AddSoldier(ActorController soldier)
//	{
//		return -1;
//	}
}
