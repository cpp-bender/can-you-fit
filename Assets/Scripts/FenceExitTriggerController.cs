using UnityEngine;

public class FenceExitTriggerController : MonoBehaviour
{
    private PlayerController player;
    private CharacterMovement characterMovement;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        characterMovement = player.GetComponent<CharacterMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            characterMovement.leftLimit = -1.8f;
            characterMovement.rightLimit = 1.8f;
        }
    }
}
