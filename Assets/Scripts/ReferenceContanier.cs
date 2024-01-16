using System.Collections.Generic;
using DG.Tweening;
using System.Linq;
using UnityEngine;

public class ReferenceContanier : SingletonMonoBehaviour<ReferenceContanier>
{
    [Header("Capsule Controllers Fields")]
    [SerializeField] ScaleParams capsulesScaleParams;

    [HideInInspector]
    public int activeGateLightIndex;

    private List<CapsuleController> capsuleControllers;
    private List<CapsuleController> disabledCapsuleControllers;
    public List<GameObject> gateLigths;

    protected override void Awake()
    {
        base.Awake();
        InitDOTween();
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        GetAllCapsules();
        GetAllGateLights();
        capsuleControllers = new List<CapsuleController>();
        disabledCapsuleControllers = new List<CapsuleController>();
    }

    private void InitDOTween()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        DOTween.defaultAutoPlay = AutoPlay.None;
        DOTween.defaultEaseType = Ease.Linear;
        DOTween.defaultAutoKill = true;
    }

    private void GetAllCapsules()
    {
        capsuleControllers = FindObjectsOfType<CapsuleController>().ToList();
    }

    private void GetAllGateLights()
    {
        gateLigths = GameObject.FindGameObjectsWithTag("GateLight").ToList();
        int n = gateLigths.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var currentGateLight = gateLigths[i].gameObject;
                var currentGateLightIndex = i;
                var nextGateLight = gateLigths[j].gameObject;
                var nextGateLightIndex = j;
                if (currentGateLight.transform.position.z > nextGateLight.transform.position.z)
                {
                    var tempGateLight = currentGateLight;
                    gateLigths[currentGateLightIndex] = nextGateLight;
                    gateLigths[nextGateLightIndex] = tempGateLight;
                }
            }
        }
    }

    public GateLightController GetActiveGateLight()
    {
        return gateLigths[activeGateLightIndex].GetComponent<GateLightController>();
    }

    public ScaleParams GetCapsuleScaleParams()
    {
        return capsulesScaleParams;
    }

    public void HandleCapsulesScale(int scaleFactor = 1)
    {
        for (int i = 0; i < capsuleControllers.Count; i++)
        {
            capsuleControllers[i].HandleScale(scaleFactor);
        }
    }

    public void AddCapsuleToPool(CapsuleController capsuleController)
    {
        disabledCapsuleControllers.Add(capsuleController);
    }

    public void ResetDisabledCapsuleControllers()
    {
        for (int i = 0; i < disabledCapsuleControllers.Count; i++)
        {
            var currentCapsuleGO = disabledCapsuleControllers[i].gameObject;
            currentCapsuleGO.SetActive(true);
        }
        disabledCapsuleControllers.Clear();
    }
}
