using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Data", menuName = "Can You Fit/Camera Data")]
public class CameraData : ScriptableObject
{
    [Header("Follow Params")]
    [SerializeField] Vector3 currentFolllowOffset;
    [SerializeField] float currentFolllowSpeed;
    [SerializeField] float scaleTransitionFollowSpeed;
    [SerializeField] float scaleTransitionFollowCompletionTime;

    [Header("Camera Main Follow Logic")]
    [SerializeField] List<float> rotationXValues;
    [SerializeField] List<float> positionYValues;
    [SerializeField] List<float> positionZValues;
    [SerializeField] List<float> followSpeeds;

    public float CurrentFollowSpeed { get => currentFolllowSpeed; set => currentFolllowSpeed = value; }
    public float ScaleTransitionFollowSpeed { get => scaleTransitionFollowSpeed; set => scaleTransitionFollowSpeed = value; }
    public float ScaleTransitionFollowCompletionTime { get => scaleTransitionFollowCompletionTime; set => scaleTransitionFollowCompletionTime = value; }
    public Vector3 CurrentFollowOffset { get => currentFolllowOffset; set => currentFolllowOffset = value; }
    public List<float> RotationXValues { get => rotationXValues; }
    public List<float> PositionYValues { get => positionYValues; }
    public List<float> PositionZValues { get => positionZValues; }
    public List<float> FollowSpeeds { get => followSpeeds; }
}
