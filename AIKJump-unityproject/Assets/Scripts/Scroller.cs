﻿using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour 
{
	public float amount = 54.6f;		//amount to move the scenery forward
	
	
	void OnTriggerEnter2D(Collider2D other)
	{
		//if this object's trigger collider hits another object tagged "scenery"...
		if (other.tag == "Scenery") 
		{
			//...get the other object's position...
			Vector3 pos = other.transform.position;
			//...add the amount to move it on the y-axis...
			pos.y += amount;
			pos.z = 0;
			//...apply that to the other object's position.
			other.transform.position = pos;	
		}
	}
}
