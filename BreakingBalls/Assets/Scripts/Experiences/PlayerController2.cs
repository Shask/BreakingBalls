using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider2D))]
public class PlayerController2 : MonoBehaviour {

	const float skinWidth = 0.015f;

	public LayerMask collisionMask;

	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;


	void Start()
	{
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}
	
	public void Move(Vector3 velocity)
	{
		UpdateRaycastOrigines ();
		if (velocity.x != 0) {
			//HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}
		
		transform.Translate (velocity);
	}
	void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i =0; i<verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY==-1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right*(verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit=Physics2D.Raycast (rayOrigin, Vector2.up * directionY,rayLength,collisionMask);

			Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right*verticalRaySpacing*i,Vector2.up*-2,Color.red);
		if(hit)		
			{
				velocity.y =(hit.distance-skinWidth) *directionY;
				rayLength=hit.distance;
			}
		}

	}

	void UpdateRaycastOrigines()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}


	void CalculateRaySpacing()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

	}

	struct RaycastOrigins {
		public Vector2 topLeft,topRight;
		public Vector2 bottomLeft,bottomRight;
	}
}
