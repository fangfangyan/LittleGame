public struct ActorInfo {

	public readonly string ActorId;

	public readonly ECamp campType;

	public readonly float HPoint;

	public readonly float MPoint;

	public ActorInfo(HeroModel heroModel)
	{
		ActorId = heroModel.actorId;
		campType = heroModel.campType;
		HPoint = heroModel.hp;
		MPoint = heroModel.mp;
	}

	public ActorInfo(SoldierModel soldierModel)
	{
		ActorId = soldierModel.actorId;
		campType = soldierModel.campType;
		HPoint = soldierModel.hp;
		MPoint = soldierModel.mp;
	}

	public ActorInfo(CreepModel creepModel)
	{
		ActorId = creepModel.actorId;
		campType = creepModel.campType;
		HPoint = creepModel.hp;
		MPoint = creepModel.mp;
	}
}
