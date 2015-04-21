using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;

	private BoxCollider collider;
	private Vector3 s;
	private Vector3 c;
	
	private float skin = .005f;
	
	[HideInInspector]
	public bool grounded;
	public bool movementStopped;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;
	}

	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;
		
		// Check collisions above and below
		grounded = false;
		
		for (int i = 0; i<3; i ++) {
			float dir = Mathf.Sign (deltaY);
			float x = (p.x + c.x - s.x / 2) + s.x / 2 * i; // Left, centre and then rightmost point of collider
			float y = p.y + c.y + s.y / 2 * dir; // Bottom of collider
			
			ray = new Ray (new Vector2 (x, y), new Vector2 (0, dir));
			//Debug.DrawRay (ray.origin, ray.direction);
			if (Physics.Raycast (ray, out hit, Mathf.Abs (deltaY) + skin, collisionMask)) {
				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaY = dst * dir - skin * dir;
				} else {
					deltaY = 0;
				}
				
				grounded = true;
				
				break;
				
			}
		}
		// Check collisions left and roght

		movementStopped = false;
		for (int i = 0; i<3; i ++) {
			float dir = Mathf.Sign (deltaX);
			if(transform.localRotation.y!=00)dir=-dir;
			float x = p.x + c.x + s.x / 2 * dir; // Left, centre and then rightmost point of collider
			float y = p.y + c.y - s.y / 2 + s.y / 2 * i;
			
			ray = new Ray (new Vector2 (x, y), new Vector2 (dir, 0));
			//Debug.DrawRay (ray.origin, ray.direction);
			if (Physics.Raycast (ray, out hit, Mathf.Abs (deltaX) + skin, collisionMask)) {
				// Get Distance between player and ground
				float dst = Vector3.Distance (ray.origin, hit.point);
				
				// Stop player's downwards movement after coming within skin width of a collider
				if (dst > skin) {
					deltaX = dst * dir - skin * dir;
				} else {
					deltaX = 0;
				}

				movementStopped=true;
				break;
				
			}
		}
		float dir2 = Mathf.Sign (deltaX);
		Vector3 playerDir = new Vector3 (deltaX, deltaY);
		Vector3 playerDir2 = new Vector3 (-deltaX, deltaY);
		/*if (transform.localRotation.y != 00) {
			dir2 = -dir2;
			playerDir=new Vector3 (-deltaX, deltaY);
		}*/

		Vector3 o = transform.position;
		Debug.DrawRay (o, playerDir.normalized*2);
		Debug.DrawRay (o, playerDir2.normalized*2);
		Ray ray1 = new Ray (o, playerDir.normalized * 2);
		Ray ray2 = new Ray (o, playerDir.normalized * 2);

		if (Physics.Raycast (ray1, Mathf.Sqrt (deltaX * deltaX + deltaY * deltaY), collisionMask) && Physics.Raycast (ray2, Mathf.Sqrt (deltaX * deltaX + deltaY * deltaY), collisionMask)) {
			grounded=true;
			deltaY=0;
		}
			Vector2 finalTransform = new Vector2 (deltaX, deltaY);
		
			transform.Translate (finalTransform);
		}

	
}
