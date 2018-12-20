using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour
{
	public float speed = -180;

	void Start()
	{
		transform.Rotate(Vector3.forward * Random.Range (0f, 360f));
	}

	void Update()
	{
		transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}
}
