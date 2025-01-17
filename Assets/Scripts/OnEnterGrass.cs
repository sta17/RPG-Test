using UnityEngine;

public class OnEnterGrass : MonoBehaviour
{
    [Header("Is in Grass")]
    [SerializeField] private bool isInTrigger;

    [Header("Trigger Handling")]
    [SerializeField] private PlayerManager PlayerManager;

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
        PlayerManager.StartBattle();
    }

    void Update()
    {
        if (isInTrigger)
        {
            StartInteraction();
        }
    }
}
