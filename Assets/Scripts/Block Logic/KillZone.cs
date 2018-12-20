using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			BeetleGirl bg = collider.GetComponentInParent<BeetleGirl>();
			if (bg != null)
				bg.Kill();
		}
	}
}
