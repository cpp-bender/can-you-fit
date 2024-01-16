using System.Collections;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem smokeParticle;
    [SerializeField] ParticleSystem speedParticle;

    private GameObject player;
    private CheckPoint checkPoint;
    private ReferenceContanier contanier;
    private CameraFollow cam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FailCondition());
        }
    }

    private void Start()
    {
        checkPoint = CheckPoint.Instance;
        contanier = ReferenceContanier.Instance;
        player = GameObject.FindGameObjectWithTag("Player");
        smokeParticle.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        cam = Camera.main.GetComponent<CameraFollow>();
    }

    public IEnumerator Respawn()
    {
        speedParticle.Play();
        yield return new WaitForSeconds(.2f);
        player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        player.transform.position = checkPoint.spawnPoints[checkPoint.ActiveSpawnPointIndex].GetComponent<Transform>().position;
        player.GetComponent<CharacterMovement>().canDoMovement = true;
        player.GetComponent<CharacterMovement>().canMoveSideways = true;
        cam.canFollow = true;
        player.GetComponent<CapsuleCollider>().enabled = true;
    }

    private void FailAnimation()
    {
        player.GetComponent<Animator>().SetTrigger("Fail");
    }

    private void StopCharacter()
    {
        player.GetComponent<CharacterMovement>().canDoMovement = false;
        player.GetComponent<CharacterMovement>().canMoveSideways = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
    }

    private void MoveCharacter()
    {

        player.GetComponent<CharacterMovement>().canDoMovement = true;
        player.GetComponent<Animator>().SetTrigger("Run");
        player.GetComponent<CharacterMovement>().canMoveSideways = true;
    }

    private IEnumerator PlaySmokeParticle()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(smokeParticle, player.transform.localPosition - Vector3.forward * .5f, player.transform.rotation);
        player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    private void SetSmokeParticleSize()
    {
        var targetParticlesize = player.transform.localScale / 2f;
        smokeParticle.transform.localScale = new Vector3(targetParticlesize.x, targetParticlesize.y, targetParticlesize.z);
    }

    public IEnumerator FailCondition()
    {
        StopCharacter();
        FailAnimation();
        SetSmokeParticleSize();
        StartCoroutine(PlaySmokeParticle());
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(Respawn());
        MoveCharacter();
        contanier.ResetDisabledCapsuleControllers();
    }
}
