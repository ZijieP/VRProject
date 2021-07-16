using System;
using UnityEngine;

public class Player
{
	int id;
	Vector3 position;
	Quaternion rotation;
	VRHelmet vrHelmet;


	public int Id { get => id; set => id = value; }
	public Vector3 Position { get => position; set => position = value; }
	public Quaternion Rotation { get => rotation; set => rotation = value; }
    public VRHelmet VRHelmet { get => vrHelmet; set => vrHelmet = value; }

	public Player(int id, Vector3 position, Quaternion rotation, VRHelmet vrHelmet)
	{
		Id = id;
		Position = position;
		Rotation = rotation;
		VRHelmet = vrHelmet;
	}
	public Player()
	{

	}

    
}
