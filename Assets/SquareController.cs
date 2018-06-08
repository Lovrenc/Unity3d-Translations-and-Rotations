using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations;

public class SquareController : MonoBehaviour
{
	public float MoveTime = .2f;
	
	
	private bool _isMoving;
	private float _squareDiagonalHalf;
	private float _squareSize;
	private float _initialHeight;

	
	
	void Awake ()
	{
		// Lengtht of a side of a squere
		_squareSize = GetComponent<Renderer>().bounds.size.x;
		
		// Initial height of our square. 
		_initialHeight = transform.position.y;
		
		// Distance from the pivot point to a vertice of a square.
		_squareDiagonalHalf = Mathf.Sqrt(_squareSize * _squareSize / 2);
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

		var angleToDo = 90f;
		var rotationSpeed = angleToDo / MoveTime;
		var rotationAxis = left ? Vector3.forward : Vector3.back;
		

		while (Math.Abs(angleToDo) > 0e-5)
		{
			angleToDo = RotateForFrame(rotationSpeed, angleToDo, rotationAxis);
			MoveForFrame(left);
			yield return null;
		}

		_isMoving = false;
	}

	/// <summary>
	/// This is simple horizontal movement. The speed is constant.
	/// </summary>
	/// <param name="left"></param>
	private void MoveForFrame(bool left)
	{
		var move = new Vector3(_squareSize / MoveTime * Time.deltaTime, 0f, 0f);
		transform.Translate(left ? -move : move, Space.World);
	}
	
	private float RotateForFrame(float rotationSpeed, float rotationLeft, Vector3 axis)
	{
		// Calculate the rotation angle for this frame
		var frameRotation = rotationSpeed * Time.deltaTime;
		frameRotation = Math.Min(rotationLeft, frameRotation);
		transform.Rotate(axis, frameRotation);
		
		// Calculate the height of the centre for current rotation
		var h = Mathf.Sin(Mathf.Deg2Rad * (45f + 90f - rotationLeft + frameRotation)) * _squareDiagonalHalf;
		var newPosition = new Vector3(transform.position.x, h + _initialHeight - _squareSize / 2f, transform.position.z);
		transform.position = newPosition;
		
		return rotationLeft - frameRotation;
	}
	
}
