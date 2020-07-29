using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FollowTarget;
	public Vector3 TargetOffset;
	public float MoveSpeed = 2f;

	public void SetTarget ( Transform aTransform ) {
		FollowTarget = aTransform;	
	}

	private void LateUpdate () {
		if(FollowTarget != null)
			transform.position = Vector3.Lerp( transform.position, new Vector3(FollowTarget.position.x, 0, FollowTarget.position.z) + TargetOffset, MoveSpeed * Time.deltaTime );
	}
}
