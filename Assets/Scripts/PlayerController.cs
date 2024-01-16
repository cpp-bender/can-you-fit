using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using System;
using TMPro;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Skinned Mesh Renderers")]
    public SkinnedMeshRenderer dressRenderer;
    public SkinnedMeshRenderer hairRenderer;
    public SkinnedMeshRenderer wingsRenderer;

    [Header("Dress Mats")]
    public Material playerDressBaseMat;
    public Material playerDressFadeMat;

    [Header("Hair Mats")]
    public Material playerHairBaseMat;
    public Material playerHairFadeMat;

    [Header("Wing Mats")]
    public Material playerWingsBaseMat;
    public Material playerWingsFadeMat;

    [Header("Other Params")]
    [SerializeField] Transform fakeAlicePrefab;
    [SerializeField] ScaleParams scaleParams;
    [SerializeField] float forwardSpeedDamper;
    [SerializeField] float swipeSensitivityDamper;
    public HangObstacleEnter hangObstacleEnter;

    [Header("DO NOT EDIT THESE VALUES!")]
    public List<float> scaleStates;

    private const string runningState = "Run";

    private ReferenceContanier contanier;
    private CameraFollow cam;
    private Animator animator;
    private GameObject instantiatedFakePlayer;
    private CharacterMovement characterMovement;

    //Scale Up Params
    private Tween scaleUpTween;
    public int scaleUpTweenCount;

    //Scale Down Params
    private Tween scaleDownTween;
    public int scaleDownTweenCount;

    //Scale up-down params
    private Action<int, int> scaleCountChange;
    private IEnumerator scalePointer;

    public int currentPlayerStateIndex;

    [Header("DELETE THESE LATER ON")]
    public TextMeshProUGUI scaleText;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        scaleCountChange = OnScaleCountChanged;
    }

    private void Start()
    {
        contanier = ReferenceContanier.Instance;
        cam = Camera.main.GetComponent<CameraFollow>();
        characterMovement = GetComponent<CharacterMovement>();
        currentPlayerStateIndex = 5;
        InitFakeFatman();
        InitScaleState();
        contanier.GetActiveGateLight().HandleGateLightColor(transform.localScale.y);
    }

    private void InitScaleState()
    {
        var count = (int)((scaleParams.maxScale - scaleParams.minScale) / scaleParams.scaleUpDamper);
        var currentScale = scaleParams.minScale;
        for (int i = 0; i <= count; i++)
        {
            scaleStates.Add(currentScale);
            currentScale += scaleParams.scaleUpDamper;
        }
        scaleStates.Add(scaleParams.maxScale);
    }

    private void InitFakeFatman()
    {
        instantiatedFakePlayer = Instantiate(fakeAlicePrefab).gameObject;
        instantiatedFakePlayer.gameObject.SetActive(false);
    }

    private void OnScaleCountChanged(int scaleUp, int scaleDown)
    {
        scaleUpTweenCount += scaleUp;
        scaleDownTweenCount += scaleDown;
        if (scalePointer == null)
        {
            scalePointer = ScaleRoutines();
            StartCoroutine(ScaleRoutines());
        }
    }

    private IEnumerator ScaleRoutines()
    {
        while (true)
        {
            if (scaleUpTweenCount <= 0 && scaleDownTweenCount <= 0)
            {
                scalePointer = null;
                yield break;
            }
            if (scaleDownTweenCount > 0)
            {
                var targetScale = transform.localScale - new Vector3(scaleParams.scaleDownDamper, scaleParams.scaleDownDamper, scaleParams.scaleDownDamper);
                scaleDownTween = CreateScaleDownTween(ClampScale(targetScale));
                HandleFakeFatmanScaleDownAnimation();
                yield return scaleDownTween.Play().WaitForCompletion();
            }
            if (scaleUpTweenCount > 0)
            {
                var targetScale = transform.localScale + new Vector3(scaleParams.scaleUpDamper, scaleParams.scaleUpDamper, scaleParams.scaleUpDamper);
                scaleUpTween = CreateScaleUpTween(ClampScale(targetScale));
                HandleFakePlayerScaleUpAnimation();
                yield return scaleUpTween.Play().WaitForCompletion();
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Capsule"))
        {
            var capsule = other.gameObject.GetComponent<CapsuleController>();

            var playerStateIndex = currentPlayerStateIndex;

            contanier.AddCapsuleToPool(capsule);

            if (capsule.scaleType == ScaleType.UP)
            {
                scaleCountChange?.Invoke(1, 0);
                capsule.DoScaleAnim();
                contanier.HandleCapsulesScale(1);
                playerStateIndex = Mathf.Clamp(++playerStateIndex, 0, 10);
            }
            else if (capsule.scaleType == ScaleType.DOWN)
            {
                scaleCountChange?.Invoke(0, 1);
                capsule.DoScaleAnim();
                contanier.HandleCapsulesScale(-1);
                playerStateIndex = Mathf.Clamp(--playerStateIndex, 0, 10);
            }
            else if (capsule.scaleType == ScaleType.NONE)
            {
                Debug.LogError("Capsule scale type not specified!");
                return;
            }
        }
    }

    private void HandleFakePlayerScaleUpAnimation()
    {
        var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var nextScale = new Vector3(transform.localScale.x + scaleParams.scaleUpDamper, transform.localScale.y + scaleParams.scaleUpDamper, transform.localScale.z + scaleParams.scaleUpDamper);
        if (scaleParams.maxScale >= nextScale.x)
        {
            currentPlayerStateIndex++;
            cam.OnPlayerScaleUp(currentPlayerStateIndex).Play();
            //Camera.main.GetComponent<SideViewCameraController>().OnPlayerScaleUp();
            SetAllPlayerStates(currentPlayerStateIndex);
            SetScaleUpFakePlayerMaterials();
            instantiatedFakePlayer.transform.localScale = nextScale;
            instantiatedFakePlayer.SetActive(true);
            instantiatedFakePlayer.GetComponent<Animator>().Play(currentStateInfo.shortNameHash, 0, currentStateInfo.normalizedTime);
        }
    }

    private void SetAllPlayerStates(int index)
    {
        cam.ChangeCamFollowSpeed(index);
        characterMovement.ChangePlayerSpeed(index);
        characterMovement.ChangePlayerSwipeSens(index);
    }

    private void HandleFakeFatmanScaleDownAnimation()
    {
        var currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var nextScale = new Vector3(transform.localScale.x - scaleParams.scaleDownDamper, transform.localScale.y - scaleParams.scaleDownDamper, transform.localScale.z - scaleParams.scaleDownDamper);
        if (scaleParams.minScale <= nextScale.x)
        {
            currentPlayerStateIndex--;
            cam.OnPlayerScaleDown(currentPlayerStateIndex).Play();
            //Camera.main.GetComponent<SideViewCameraController>().OnPlayerScaleDown();
            SetAllPlayerStates(currentPlayerStateIndex);
            SetScaleDownPlayerMaterials();
            instantiatedFakePlayer.transform.localScale = nextScale;
            instantiatedFakePlayer.SetActive(true);
            instantiatedFakePlayer.GetComponent<Animator>().Play(currentStateInfo.shortNameHash, 0, currentStateInfo.normalizedTime);
        }
    }

    private void SetScaleUpFakePlayerMaterials()
    {
        var fakePlayer = instantiatedFakePlayer.GetComponent<FakePlayerController>();
        fakePlayer.ChangeDressMatTo(playerDressFadeMat);
        fakePlayer.ChangeHairMatTo(playerHairFadeMat);
        fakePlayer.ChangeWingsMatTo(playerWingsFadeMat);
    }

    private void SetScaleDownPlayerMaterials()
    {
        dressRenderer.material = playerDressFadeMat;
        hairRenderer.material = playerHairFadeMat;
        wingsRenderer.material = playerWingsFadeMat;
    }

    private void ResetPlayerMaterials()
    {
        dressRenderer.material = playerDressBaseMat;
        hairRenderer.material = playerHairBaseMat;
        wingsRenderer.material = playerWingsBaseMat;
    }

    private void ResetFakePlayerMaterials()
    {
        var fakePlayer = instantiatedFakePlayer.GetComponent<FakePlayerController>();
        fakePlayer.ChangeDressMatTo(playerDressBaseMat);
        fakePlayer.ChangeHairMatTo(playerHairBaseMat);
        fakePlayer.ChangeWingsMatTo(playerWingsBaseMat);
    }

    private Vector3 ClampScale(Vector3 targetScale)
    {
        var clampedTargetScale = new Vector3(Mathf.Clamp(targetScale.x, scaleParams.minScale, scaleParams.maxScale),
            Mathf.Clamp(targetScale.y, scaleParams.minScale, scaleParams.maxScale),
            Mathf.Clamp(targetScale.z, scaleParams.minScale, scaleParams.maxScale));
        return clampedTargetScale;
    }

    private Tween CreateScaleUpTween(Vector3 targetScale)
    {
        Tween tempScaleUpTween;
        tempScaleUpTween = transform.DOScale(targetScale, scaleParams.scaleCompletionTime)
        .SetAs(scaleParams.GetScaleTweenParams())
        .OnStart(delegate
        {
            //scaleUpTweenCount--;
        })
        .OnComplete(delegate
        {
            scaleUpTweenCount--;

            ResetFakePlayerMaterials();
            instantiatedFakePlayer.SetActive(false);
            float scaleValue = scaleStates[currentPlayerStateIndex];
            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

            contanier.GetActiveGateLight().HandleGateLightColor(transform.localScale.y);
        });
        return tempScaleUpTween;
    }

    private Tween CreateScaleDownTween(Vector3 targetScale)
    {
        Tween tempScaleDownTween;
        tempScaleDownTween = transform.DOScale(targetScale, scaleParams.scaleCompletionTime)
            .SetAs(scaleParams.GetScaleTweenParams())
            .OnStart(delegate
            {
                //scaleDownTweenCount--;
            })
            .OnComplete(delegate
            {
                scaleDownTweenCount--;

                ResetPlayerMaterials();
                instantiatedFakePlayer.SetActive(false);
                float scaleValue = scaleStates[currentPlayerStateIndex];
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                contanier.GetActiveGateLight().HandleGateLightColor(transform.localScale.y);
            });
        return tempScaleDownTween;
    }

    public void SetAnimationToRunState()
    {
        animator.SetTrigger(runningState);
        instantiatedFakePlayer.GetComponent<Animator>().SetTrigger(runningState);
    }
}