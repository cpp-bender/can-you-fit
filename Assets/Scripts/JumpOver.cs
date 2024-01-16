using System.Collections;
using UnityEngine;

public class JumpOver : MonoBehaviour
{
    public ColliderPlaceholderController colliderPlaceholder;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.GetComponent<Transform>().localScale.x >= 1f)
            {
                StartCoroutine(JumpOverAnimation());
            }
        }
    }

    public IEnumerator JumpOverAnimation()
    {
        player.GetComponent<CharacterMovement>().canMoveSideways = false;
        colliderPlaceholder.TurnOffColliders();
        player.GetComponent<Animator>().SetInteger("JumpIndex", UnityEngine.Random.Range(0, 2));
        player.GetComponent<Animator>().SetTrigger("JumpOver");
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<CharacterMovement>().canMoveSideways = true;
        colliderPlaceholder.TurnOnColliders();
        player.GetComponent<Animator>().SetTrigger("Run");
    }
}
