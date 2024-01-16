using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    private CheckPoint checkPoint;
    private ReferenceContanier contanier;
    private PlayerController player;

    private void Start()
    {
        checkPoint = CheckPoint.Instance;
        contanier = ReferenceContanier.Instance;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            checkPoint.SetActiveSpawnPoint(++checkPoint.ActiveSpawnPointIndex);
            ++contanier.activeGateLightIndex;
            contanier.GetActiveGateLight().HandleGateLightColor(player.transform.localScale.y);
        }
    }
}
