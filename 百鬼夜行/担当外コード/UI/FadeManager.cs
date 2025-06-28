using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{

	// 
	[SerializeField] 
	private Image startImage;
													  
	private void Awake()
	{
		StartCoroutine(ShowStartImage());
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private IEnumerator ShowStartImage()
	{
		
		Color c = startImage.color;
		c.a = 0.85f;
		startImage.color = c;

		yield return new WaitForSeconds(1f);

		
		c.a = 0f;
		startImage.color = c;
	}
}
