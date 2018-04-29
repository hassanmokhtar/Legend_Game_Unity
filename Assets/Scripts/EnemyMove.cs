using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class EnemyMove : MonoBehaviour {

	private Transform player;
//	[SerializeField] Transform player;  // edited by asem
	private UnityEngine.AI.NavMeshAgent nav;
	private Animator anim;
	private EnemyHealth enemyHealth;

	// Use this for initialization
	void Start () {

		player = GameManager.instance.Player.transform;
		enemyHealth = GetComponent<EnemyHealth> ();
		anim = GetComponent<Animator> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		//nav.SetDestination (player.position);   // edited by asem

		if (!GameManager.instance.GameOver && enemyHealth.IsAlive) {        // game is not over and enemy alive
			nav.SetDestination (player.position);
		} else if ((!GameManager.instance.GameOver || GameManager.instance.GameOver) && !enemyHealth.IsAlive) {   // if enemy died and  not matter game is over or not
			nav.enabled = false;
		
		} else {                                                              // if game is over and enemy alive
			nav.enabled = false;
			anim.Play ("Idle");
		}
	
	}
}
