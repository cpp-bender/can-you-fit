using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem potionParticle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Capsule"))
            {
                StartCoroutine(PlayParticle());
            }
        }
    }

    private IEnumerator PlayParticle()
    {
        Instantiate(potionParticle, transform.position + Vector3.up * .5f, potionParticle.transform.rotation);
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
