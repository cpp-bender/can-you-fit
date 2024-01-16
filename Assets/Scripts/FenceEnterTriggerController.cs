using UnityEngine;

public class FenceEnterTriggerController : MonoBehaviour
{
    public float leftLimit = 1f;
    public float rightLimit = 1f;

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
            characterMovement.leftLimit = leftLimit;
            characterMovement.rightLimit = rightLimit;
        }
    }
}
