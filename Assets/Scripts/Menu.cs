using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	[SerializeField] GameObject hero;
	[SerializeField] GameObject monster;
	[SerializeField] GameObject skeleton;
	[SerializeField] GameObject troll;

	private Animator heroAnim;
	private Animator monsterAnim;
	private Animator skeletonAnim;
	private Animator trollAnim;

	void Awake() {

		Assert.IsNotNull (hero);
		Assert.IsNotNull (monster);
		Assert.IsNotNull (skeleton);
		Assert.IsNotNull (troll);

	}

	// Use this for initialization
	void Start () {
	
		heroAnim = hero.GetComponent<Animator> ();
		monsterAnim = monster.GetComponent<Animator> ();
		skeletonAnim = skeleton.GetComponent<Animator> ();
		trollAnim = troll.GetComponent<Animator> ();

		StartCoroutine (showcase ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator showcase() {

		yield return new WaitForSeconds (1f);
		heroAnim.Play ("SpinAttack");
		yield return new WaitForSeconds (1f);
		monsterAnim.Play ("Attack");
		yield return new WaitForSeconds (1f);
		skeletonAnim.Play ("Attack");
		yield return new WaitForSeconds (1f);
		trollAnim.Play ("Attack");
		yield return new WaitForSeconds (1f);

		StartCoroutine (showcase ());
	}

	public void Battle() {
		SceneManager.LoadScene ("Level");
	}

	public void Quit() {
		Application.Quit ();
	}
}
