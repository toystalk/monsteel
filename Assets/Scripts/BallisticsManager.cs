using UnityEngine;
using System.Collections;

public class BallisticsManager : MonoBehaviour {

	public GameObject enemy;
	public GameObject hero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.S)) {
			GetComponent<Ballistics>().Shoot(enemy, new Vector3(0, 1f, 0), 45f);
		}
	}
}
