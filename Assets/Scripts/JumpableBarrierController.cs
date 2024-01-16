using UnityEngine;

public class JumpableBarrierController : MonoBehaviour
{
    public CheckPointTrigger checkPointTrigger;

    private CharacterMovement characterMovement;

    private void Start()
    {
        characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Transform>().localScale.x < 1f)
        {
            StartCoroutine(checkPointTrigger.FailCondition());
        }
        else
        {

        }
    }

    private void TurnOffAllColliders()
    {
        //TODO:Code here
    }
}
