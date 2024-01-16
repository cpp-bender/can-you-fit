using UnityEngine;
using DG.Tweening;

public class TreeController : MonoBehaviour
{
    private PlayerController player;
    private bool isAlreadyTakenDown;

    [Header("Tree take-down tween params")]
    public float tweeenEndYValue = -10f;
    public float tweenCompletionTime = 2f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    private void Update()
    {
        if (!isAlreadyTakenDown && player.transform.position.z > transform.position.z)
        {
            TakeDownTree();
        }
    }

    private void TakeDownTree()
    {
        transform.DOMoveY(-10, 2f)
            .OnComplete(delegate
            {
                isAlreadyTakenDown = true;
            })
            .Play();
    }
}
