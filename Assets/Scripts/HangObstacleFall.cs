using System.Collections;
using UnityEngine;

public class HangObstacleFall : MonoBehaviour
{
    public CheckPointTrigger checkPointTrigger;

    private PlayerController player;
    private CharacterMovement characterMovement;
    private CameraFollow cam;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cam = Camera.main.GetComponent<CameraFollow>();
        characterMovement = player.GetComponent<CharacterMovement>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Transform>().localScale.x < 1)
        {
            StartCoroutine(FallThenRestart());
        }
    }
    private IEnumerator FallThenRestart()
    {
        yield return new WaitForSeconds(.2f);
        player.GetComponent<Rigidbody>().isKinematic = false;
        cam.canFollow = false;
        player.GetComponent<Animator>().SetTrigger("FallDown");
        characterMovement.canDoMovement = false;
        characterMovement.canMoveSideways = false;
        yield return new WaitForSeconds(1f);
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Animator>().SetTrigger("Run");
        StartCoroutine(checkPointTrigger.Respawn());
    }
}
