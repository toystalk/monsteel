using UnityEngine;
using System.Collections;

/**
 * Functions for basic ballistics calculations.
 * Author: Rafael Batista, September 2014
 */
public class Ballistics : MonoBehaviour {
	public float gravityFactor = 1f;

	/**
	 * Given an instance of a projectile, a point in 3D space and
	 * an angle, shoot the projectile to pass exactly through the point.
	 */
	public void Shoot(GameObject projectile, Vector3 target, float angle) {
		// Aliases
		Vector3 p = projectile.transform.position, 
		        t = target;
		float a = Mathf.Deg2Rad * angle;
		float h = t.y;
		float h0 = p.y;
		float g = gravityFactor * Physics.gravity.magnitude;
		
		// Calculate ground direction vector. The height will be set
		// according to the angle.
		Vector3 direction = t - p;
		direction.y = 0;
		
		// Calculate the speed needed to shoot the projectile
		// at this angle and reach the target height in the target position.
		// The expression for velocity is the formula for height as a function
		// of distance of a projectile, solved for v.
		float d = direction.magnitude;
		float v = (Mathf.Sqrt(g) * d * (1f / Mathf.Cos(a))) / 
		          (Mathf.Sqrt(2) * Mathf.Sqrt(Mathf.Abs(d * Mathf.Tan(a) - h + h0)));
		
		// Set angle and magnitude of velocity vector.
		Vector3 shot = direction;
		shot.y = d * Mathf.Tan(a);
		shot.Normalize();
		shot *= v;
		
		// Apply to the object
		projectile.rigidbody.useGravity = true;
		projectile.rigidbody.velocity = shot;
	}
}
