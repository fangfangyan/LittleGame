using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour {

	public GameObject DesText;

	public GameObject ScoreText;

	public GameObject PlayButton;

	public GameData gameData;

	private int score;

	private ActorManager manager;

	private GameObject hero;

	private List<GameObject> soldiers;

	private LinkedList<GameObject> creeps;

	public HeroCamp camp;

	IHeroControl heroControl;

	private bool enableUpdate;

//	IActorDirector actorDirector;

//	LayerMask layerMask;

	// Use this for initialization
	void Awake(){
		manager = ActorManager.GetInstance ();

		DesText.SetActive (false);
		ScoreText.SetActive (false);

		soldiers = new List<GameObject> ();
		creeps = new LinkedList<GameObject> ();
	}

	public void InitGame()
	{
		enableUpdate = true;

		hero = manager.CreateHero ();
		hero.transform.localPosition = new Vector3 (-3f, 0, -3f);		

		IActorManager heroManager = hero.GetComponent<IActorManager> ();
		camp = new HeroCamp (0, heroManager);
		heroManager.SetCamp (camp);

		GetComponent<CameraFollow> ().SetTarget (hero);

		heroControl = hero.GetComponent<IHeroControl> ();

		PlayButton.SetActive (false);

		CreateSoldiers ();
		CreateCreeps ();
		score = 0;
	}

	public void DestroyActor(GameObject actor)
	{
		if (actor == hero) 
		{
			GameOver ();
			return;
		}
		else if (soldiers.Contains (actor)) 
		{
			int index = soldiers.IndexOf (actor);
			soldiers [index] = null;
			StartCoroutine(CreateSoldier(index, 10f)); 
		} 
		else if (creeps.Contains (actor)) 
		{
			creeps.Remove (actor);
			score++;
		}
		Destroy (actor);
	}

	IEnumerator CreateSoldier(int index, float delaySecond)
	{
		yield return new WaitForSeconds(delaySecond);
		GameObject gameObject0 = manager.CreatSoldier ();
		gameObject0.transform.localPosition = gameData.soldierLocations[index];
		soldiers [index] = gameObject0;
	}

	private void CreateSoldiers()
	{
		foreach (Vector3 location in gameData.soldierLocations) 
		{
			GameObject gameObject0 = manager.CreatSoldier ();
			gameObject0.transform.localPosition = location;
			soldiers.Add (gameObject0);
		}
	}

	private void CreateCreeps()
	{
		if (!enableUpdate)
			return;
		foreach (Vector3 location in gameData.creepLocations) 
		{
			GameObject gameObject0 = manager.CreateCreep ();
			gameObject0.transform.localPosition = location;
			creeps.AddLast (gameObject0);
		}
		Invoke ("CreateCreeps", 10f);
	}

	// Update is called once per frame
	void Update () 
	{
		if (!enableUpdate)
			return;
		
		UpdateCamera ();
	}

	public void GameOver()
	{
		enableUpdate = false;
		heroControl = null;

		DesText.SetActive (true);
		ScoreText.SetActive (true);
		PlayButton.SetActive (true);
		Text sText = ScoreText.GetComponent<Text> ();
		sText.text = "YOUR SCORE IS: " + score;

		ClearActor ();
	}

	public void ClearActor()
	{
		foreach (GameObject gameObject0 in creeps) 
		{
			Destroy (gameObject0);
		}
		creeps.Clear ();

		foreach (GameObject gameObject0 in soldiers) 
		{
			Destroy (gameObject0);
		}
		soldiers.Clear ();

		Destroy (hero);
	}

	private void UpdateCamera()
	{
		if (heroControl == null)
			return;

		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				//maybe the property of tag is not exsist in unity v5.2+
				if(hit.collider.tag == "Ground")
				{
					heroControl.SetDestination(hit.point);
				}
			}

		}
	}
}
