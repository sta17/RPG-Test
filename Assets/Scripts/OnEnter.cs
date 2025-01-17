using UnityEngine;

public class OnEnter : MonoBehaviour
{

    [SerializeField] private PCControls PCControls;
    [SerializeField] private bool isInteractPressed;
    [SerializeField] private bool isInTrigger;


    private void OnTriggerEnter()
    {
        isInTrigger = true;
    }

    private void OnTriggerExit()
    {
        isInTrigger = false;
    }

    public virtual void StartInteraction()
    {
        //Debug.Log("On Enter Works.");
    }

    private void Awake()
    {
        PCControls = new PCControls();
    }

    void Update()
    {
        isInteractPressed = PCControls.CharacterControls.Interact.WasPerformedThisFrame();

        if (isInteractPressed & isInTrigger)
        {
            StartInteraction();
        }
    }

    void OnEnable()
    {
        PCControls.CharacterControls.Enable();
    }

    void OnDisable()
    {
        PCControls.CharacterControls.Disable();
    }
}
