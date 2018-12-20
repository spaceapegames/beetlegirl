using UnityEngine;
using System.Collections;

public class Firebeam : MonoBehaviour
{
	public float speed = 90;
	public GameObject firebeamElement;
	public int firebeamLength = 3;

	void Start ()
	{
		for (int i = 0; i < firebeamLength; i++)
		{
			GameObject newObject = Instantiate(firebeamElement, transform.position + new Vector3(0, (i + 1) * 0.75f, -1), Quaternion.identity) as GameObject;
			newObject.transform.parent = transform;
		}
		transform.Rotate(Vector3.forward * Random.Range (0f, 360f));
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}
}
