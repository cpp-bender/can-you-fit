using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public CameraData cameraData;
    public bool canFollow;

    private Tween playerScaleUpCamTween;
    private Tween playerScaleDownCamTween;
    private Tween playerTransitionScaleCamTween;

    private void Start()
    {
        cameraData.CurrentFollowOffset = transform.position;
        cameraData.CurrentFollowSpeed = cameraData.FollowSpeeds[5];
        canFollow = true;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        if (!canFollow)
        {
            return;
        }
        var currentPos = transform.position;
        var targetPos = target.position + cameraData.CurrentFollowOffset;
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        transform.position = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * cameraData.CurrentFollowSpeed);
    }

    public Tween OnPlayerScaleUp(int playerStateIndex)
    {
        var rotationX = cameraData.RotationXValues[playerStateIndex];
        Quaternion lookQuaternion = Quaternion.Euler(rotationX, transform.localRotation.y, transform.localRotation.z);
        var nextPosY = cameraData.PositionYValues[playerStateIndex];
        playerScaleUpCamTween = transform.DOMoveY(nextPosY, .5f)
            .OnStart(delegate
            {
                cameraData.CurrentFollowSpeed = cameraData.ScaleTransitionFollowSpeed;
                var clampedCamPosZ = cameraData.PositionZValues[playerStateIndex];
                cameraData.CurrentFollowOffset = new Vector3(cameraData.CurrentFollowOffset.x, cameraData.CurrentFollowOffset.y, clampedCamPosZ);
                transform.DORotate(lookQuaternion.eulerAngles, .5f).Play();
            });
        return playerScaleUpCamTween;
    }

    public Tween OnPlayerScaleDown(int playerStateIndex)
    {
        var rotationX = cameraData.RotationXValues[playerStateIndex];
        Quaternion lookQuaternion = Quaternion.Euler(rotationX, transform.localRotation.y, transform.localRotation.z);
        var nextPosY = cameraData.PositionYValues[playerStateIndex];
        playerScaleDownCamTween = transform.DOMoveY(nextPosY, .5f)
            .OnStart(delegate
            {
                cameraData.CurrentFollowSpeed = cameraData.ScaleTransitionFollowSpeed;
                var clampedCamPosZ = cameraData.PositionZValues[playerStateIndex];
                cameraData.CurrentFollowOffset = new Vector3(cameraData.CurrentFollowOffset.x, cameraData.CurrentFollowOffset.y, clampedCamPosZ);
                transform.DORotate(lookQuaternion.eulerAngles, .5f).Play();
            });
        return playerScaleDownCamTween;
    }

    public void ChangeCamFollowSpeed(int followSpeedIndex)
    {
        playerTransitionScaleCamTween = DOTween.To(() => cameraData.CurrentFollowSpeed, x => cameraData.CurrentFollowSpeed = x, cameraData.FollowSpeeds[followSpeedIndex], cameraData.ScaleTransitionFollowCompletionTime).Play();
    }
}
