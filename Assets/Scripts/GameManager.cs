using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	[SerializeField] GameObject player;
	[SerializeField] GameObject[] spawnPoints;
	[SerializeField] GameObject tanker;
	[SerializeField] GameObject ranger;
	[SerializeField] GameObject soldier;
	[SerializeField] GameObject arrow ;
	[SerializeField] Text levelText;
	private GameObject newEnemy;

	private bool gameOver = false;
	private int currentLevel;
	private float generatedSpawnTime = 1;
	private float currentSpawnTime = 0;


	private List<EnemyHealth> enemies = new List<EnemyHealth> ();
	private List<EnemyHealth> killedEnemies = new List<EnemyHealth> ();

	public void RegisterEnemy(EnemyHealth enemy){

		enemies.Add (enemy);
	}

	public void killedEnemy(EnemyHealth enemy){

		killedEnemies.Add (enemy);
	}
	public bool GameOver {
		get {return gameOver; }
	}

	public GameObject Player {
		get { return player; }
	}

	public GameObject Arrow {
		get { return arrow; }
	}
	
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {

		// create reurcive function
		StartCoroutine(spawn());

		currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {

		currentSpawnTime += Time.deltaTime;
	}

	public void PlayerHit(int currentHP) {

		if (currentHP > 0) {
			gameOver = false;
		} else {
			gameOver = true;
			//StartCoroutine (endGame ("Defeat"));
		}
	}

	IEnumerator spawn(){

		// check if spawn time is greater than current
		// then set current time spawn to 0
		// and check if the count of enemies is less than current level 
		// then get random number from 0 to spawn point count minus 1
		// and then added the new number to spawnpoint to create new enemy

		if(currentSpawnTime > generatedSpawnTime){

			currentSpawnTime = 0;

			if(enemies.Count < currentLevel){

				int rand = Random.Range(0 , spawnPoints.Length - 1);

				GameObject spawnLocation = spawnPoints[rand];

				int randEnemy = Random.Range(0 , 3);

				if(randEnemy == 0 )

					newEnemy = Instantiate (soldier) as GameObject;

				else if (randEnemy == 1)

					newEnemy = Instantiate (ranger) as GameObject;	

				else if (randEnemy == 2)

					newEnemy = Instantiate (tanker) as GameObject;

				newEnemy.transform.position = spawnLocation.transform.position;

			}

			if(killedEnemies.Count == currentLevel){

				enemies.Clear();

				killedEnemies.Clear();

				yield return new WaitForSeconds(3f);

				currentLevel ++;

				levelText.text = "Level " + currentLevel;

			}
		}

		yield return null;

		// create reurcive function
		StartCoroutine(spawn());

	}
}
