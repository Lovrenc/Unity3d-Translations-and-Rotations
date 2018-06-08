using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.WSA;

public class UnitySquareController : MonoBehaviour {

	public float MoveTime = .2f;
	
	
	private bool _isMoving;

	private Vector3 _leftPivotOffset;
	private Vector3 _rightPivotOffset;
	
	
	void Awake ()
	{
		// Calculate pivot points, the points we will rotate our square around when moving
		var size = GetComponent<Renderer>().bounds.size;
		_leftPivotOffset = new Vector3(-size.x / 2, -size.y / 2);
		_rightPivotOffset = new Vector3(size.x / 2, -size.y / 2);
	}
	
	// Update is called once per frame
	void Update ()
	{
		var h = Input.GetAxis("Horizontal");
		if (h != 0)
		{
			Move(h < 0);
		}
	}

	
	private void Move(bool goLeft)
	{
		// We are already moving. Ignore the move command.
		if (_isMoving)
			return;

		StartCoroutine(MoveCoroutine(goLeft));

	}
	
	IEnumerator MoveCoroutine(bool left)
	{		
		_isMoving = true;

		// Get the appropriate pivot point
		var pivot = transform.position;
		pivot += left ? _leftPivotOffset : _rightPivotOffset;
		
		// Get the axis to rotate around. We could also just reverse the angle.
		var axis = left ? Vector3.forward : Vector3.back;
		var animationTime = 0f;
		
		while (animationTime < MoveTime)
		{
			// Make sure you do not rotate too much on the last frame
			var time = animationTime + Time.deltaTime < MoveTime 
				? Time.deltaTime : MoveTime - animationTime;
			
			var frameAngle = time / MoveTime * 90f;
			transform.RotateAround(pivot, axis, frameAngle);
			animationTime += Time.deltaTime;
			yield return null;
		}

		_isMoving = false;
	}
}
