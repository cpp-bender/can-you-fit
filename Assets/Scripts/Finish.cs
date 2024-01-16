using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private GameObject player;
    private CharacterMovement characterMovement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterMovement = player.GetComponent<CharacterMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            characterMovement.canDoMovement = false;
            characterMovement.canMoveSideways = false;
            player.GetComponent<Animator>().SetTrigger("Dance");
            GameManager.instance.LevelComplete();
        }
    }
}
