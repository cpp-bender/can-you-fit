using UnityEngine;

public class GateLightController : MonoBehaviour
{
    public enum GateLightState { Jump, Crawl, Hang }

    [Header("Materials")]
    public Material gateLightRedMat;
    public Material gateLightGreenMat;
    public Material pointerGreenMat;
    public Material pointerRedMat;

    [Header("Renderers")]
    public MeshRenderer pointerRenderer;

    [Header("Other Params")]
    public GateLightState lightState = GateLightState.Crawl;
    public float scaleValueToCheck = .25f;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void HandleGateLightColor(float scaleValue)
    {
        if (lightState == GateLightState.Jump)
        {
            if (scaleValue >= scaleValueToCheck)
            {
                meshRenderer.material = gateLightGreenMat;
                pointerRenderer.material = pointerGreenMat;
            }
            else
            {
                meshRenderer.material = gateLightRedMat;
                pointerRenderer.material = pointerRedMat;
            }
        }
        else if (lightState == GateLightState.Crawl)
        {
            if (scaleValue <= scaleValueToCheck)
            {
                meshRenderer.material = gateLightGreenMat;
                pointerRenderer.material = pointerGreenMat;
            }
            else
            {
                meshRenderer.material = gateLightRedMat;
                pointerRenderer.material = pointerRedMat;
            }
        }
        else if (lightState == GateLightState.Hang)
        {
            if (scaleValue >= scaleValueToCheck)
            {
                meshRenderer.material = gateLightGreenMat;
            }
            else
            {
                meshRenderer.material = gateLightRedMat;
            }
        }
    }
}
