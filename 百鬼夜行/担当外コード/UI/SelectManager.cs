using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectManager : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;

    private int CurrentIndex = 0;

    private float inputCooldown = 1.0f;

    private float inputTimer = 0f;
    // ƒV?ƒ“‚ªŽn‚Ü‚Á‚½‚çˆê”ÔÅ‰‚É“ü‚ê‚ç‚ê‚½??ƒ“‚ð‘I‘ð‚·‚é‚æ‚¤‚É‚·‚é
    void Start()
    {
        if (buttons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
            buttons[0].Select();
            //Debug.Log("Inside select");
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputTimer += Time.unscaledDeltaTime;

        float vertical = Input.GetAxisRaw("Vertical");

        if (inputTimer >= inputCooldown)
        {
            if (vertical > 0.5f)
            {
                CurrentIndex = (CurrentIndex - 1 + buttons.Length) % buttons.Length;
                SelectButton(CurrentIndex);
                inputTimer = 0f;
            }
            else if (vertical < -0.5f)
            {
                CurrentIndex = (CurrentIndex + 1) % buttons.Length;
                SelectButton(CurrentIndex);
                inputTimer = 0f;
            }
        }
    }

    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    
}
