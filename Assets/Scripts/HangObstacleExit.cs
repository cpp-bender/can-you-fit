using UnityEngine;
using DG.Tweening;

public class HangObstacleExit : MonoBehaviour
{
    public float hangAnimationExitTime = .2f;

    private GameObject hangLine;
    private GameObject player;
    private float timer = 0f;

    private void Start()
    {
        hangLine = GameObject.FindGameObjectWithTag("HangLine");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DOTween.To(() => timer, x => timer = x, 1f, hangAnimationExitTime)
                .OnStart(delegate
                {
                    hangLine.transform.SetParent(transform.parent);
                })
                .OnComplete(delegate
                {
                    //transform.parent.gameObject.SetActive(false);
                    other.GetComponent<Animator>().SetTrigger("Run");
                    player.GetComponent<CharacterMovement>().canDoMovement = true;
                    player.GetComponent<CharacterMovement>().canMoveSideways = true;
                    player.transform.DOMoveY(0f, 0.25f).Play();
                })
                .Play();


        }
    }
}
