using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class ButtonCloro : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] 
    private TextMeshProUGUI text;
	[SerializeField] 
    private Color normalColor = Color.black;
	[SerializeField] 
    private Color highlightColor = Color.yellow;

    private bool isSelected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            if (!isSelected)
            {
                text.color = highlightColor;
                isSelected = true;
            }
        }
        else
        {
            if (isSelected)
            {
                text.color = normalColor;
                isSelected = false;
            }
        }
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		text.color = highlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		text.color = normalColor;
	}
}
