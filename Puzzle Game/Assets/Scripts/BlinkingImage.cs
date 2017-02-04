using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour {

	private RawImage image;

	void Start ()
	{
		image = GetComponent<RawImage>();
		StartBlinking();
	}


	IEnumerator Blink()
	{
		while (true)
		{
			switch(image.color.a.ToString())
			{
			case "0":
				image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
				//Play sound
				yield return new WaitForSeconds(1f);
				break;
			case "1":
				image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
				//Play sound
				yield return new WaitForSeconds(1f);
				break;
			}
		}
	}

	void StartBlinking()
	{
		StopAllCoroutines();
		StartCoroutine("Blink");
	}

	void StopBlinking()
	{
		StopAllCoroutines();
	}

}