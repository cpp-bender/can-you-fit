using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HangObstacleEnter : MonoBehaviour
{
    private GameObject player;
    private GameObject hangLine;
    private CameraFollow cam;
    private CharacterMovement characterMovement;

    public CheckPointTrigger checkPointTrigger;
    public List<float> playerHangPosY;
    public GameObject hangLinePosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hangLine = GameObject.FindGameObjectWithTag("HangLine");
        cam = Camera.main.GetComponent<CameraFollow>();
        characterMovement = player.GetComponent<CharacterMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Transform>().localScale.x >= 1f)
            {
                StartCoroutine(Hang());
            }
        }
    }

    private IEnumerator Hang()
    {
        characterMovement.canDoMovement = false;
        characterMovement.canMoveSideways = false;
        player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
        player.GetComponent<Animator>().SetTrigger("JumpBeforeHang");
        yield return new WaitForSeconds(0f);
        player.GetComponent<Animator>().SetTrigger("Hang");
        characterMovement.canDoMovement = true;

        // MoveHangLinePosition();
        ChangePlayerPosition(player.GetComponent<PlayerController>().currentPlayerStateIndex);
        // yield return new WaitForSeconds(.2f);
        // hangLine.transform.SetParent(player.transform);

    }

    private void MoveHangLinePosition()
    {
        hangLine.transform.position = new Vector3(0f, hangLinePosition.transform.position.y, hangLinePosition.transform.position.z);
        hangLine.transform.SetParent(player.transform);
    }

    private void ChangePlayerPosition(int playerStateIndex)
    {

        player.transform.DOMoveY(playerHangPosY[playerStateIndex], 0.25f).Play()
            .OnComplete(delegate
            {
                hangLine.transform.SetParent(player.transform);
                hangLine.transform.localPosition = new Vector3(hangLine.transform.localPosition.x, hangLine.transform.localPosition.y, -0.163f);
                //if (playerStateIndex == 4)
                //{
                //    hangLine.transform.localPosition = new Vector3(hangLine.transform.localPosition.x, hangLine.transform.localPosition.y,
                //        -0.16f);
                //}
                //else if (playerStateIndex == 5)
                //{
                //    hangLine.transform.localPosition = new Vector3(hangLine.transform.localPosition.x, hangLine.transform.localPosition.y,
                //        -0.16f);
                //}
                //else if (playerStateIndex == 6)
                //{
                //    hangLine.transform.localPosition = new Vector3(hangLine.transform.localPosition.x, hangLine.transform.localPosition.y,
                //        -0.16f);
                //}
            });
    }

    public void ChangeHangScale(int playerStateIndex)
    {
        var hangParent = gameObject.transform.parent;
        hangParent.position = new Vector3(hangParent.position.x, playerHangPosY[playerStateIndex], hangParent.position.z);
    }
}
