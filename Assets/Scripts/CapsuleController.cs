using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class CapsuleController : MonoBehaviour
{
    public ScaleType scaleType = ScaleType.NONE;
    public ParticleSystem particle;

    private ReferenceContanier contanier;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        contanier = ReferenceContanier.Instance;
        HandleParticleAtStartup();
    }

    private void HandleParticleAtStartup()
    {
        particle.gameObject.SetActive(false);
    }

    public void HandleScale(int scaleFactor)
    {
        var scaleParams = contanier.GetCapsuleScaleParams();
        var currentScale = transform.localScale;
        var targetScale = currentScale + (new Vector3(scaleParams.scaleUpDamper, scaleParams.scaleUpDamper, scaleParams.scaleUpDamper) * scaleFactor);
        transform.localScale = ClampScale(targetScale);
    }

    public void DoScaleAnim()
    {
        transform.DOScale(Vector3.zero, .5f)
            .OnStart(delegate
            {
                boxCollider.enabled = false;
                particle.gameObject.SetActive(true);
            })
            .OnComplete(delegate
            {
                boxCollider.enabled = true;
                particle.gameObject.SetActive(false);
                gameObject.SetActive(false);
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            })
            .Play();
    }

    private Vector3 ClampScale(Vector3 targetScale)
    {
        var scaleParams = contanier.GetCapsuleScaleParams();
        var clampedTargetScale = new Vector3(Mathf.Clamp(targetScale.x, scaleParams.minScale, scaleParams.maxScale),
            Mathf.Clamp(targetScale.y, scaleParams.minScale, scaleParams.maxScale),
            Mathf.Clamp(targetScale.z, scaleParams.minScale, scaleParams.maxScale));
        return clampedTargetScale;
    }
}
