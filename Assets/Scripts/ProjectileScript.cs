using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public GameObject hero;

	// Kill enemy
	void OnCollisionEnter(Collision info) {
		// Play splash effect
		GameObject other = info.gameObject;
		hero.GetComponent<PlayerController>().SplashAmeba(other.transform.position, true);
		
		// Play splash audio effect
		AudioManager.instance.playEffect("amebaSplash");

		// Destroy the ameba
		Destroy(other, 0.90f);
		
		// Destroy the projectile
		Destroy (collider.gameObject);
		other.GetComponent<Animator>().SetTrigger("Die");
	}
}
