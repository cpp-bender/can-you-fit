using UnityEngine;

public class SideViewCameraController : MonoBehaviour
{
    public Transform target;
    public SideViewCameraData cameraData;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        cameraData.Offset = cameraData.InitialPos;
    }


    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + cameraData.Offset, Time.deltaTime * cameraData.Speed);
    }

    public void OnPlayerScaleDown()
    {
        cameraData.Offset = new Vector3(cameraData.Offset.x - cameraData.XDamper, cameraData.Offset.y - cameraData.YDamper,
            cameraData.Offset.z - cameraData.ZDamper);
    }

    public void OnPlayerScaleUp()
    {
        cameraData.Offset = new Vector3(cameraData.Offset.x + cameraData.XDamper, cameraData.Offset.y + cameraData.YDamper,
            cameraData.Offset.z + cameraData.ZDamper);
    }
}
