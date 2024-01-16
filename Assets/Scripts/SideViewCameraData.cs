using UnityEngine;

[CreateAssetMenu(fileName = "Can You Fit/ Side View Camera Data", menuName = "SideViewCamData")]
public class SideViewCameraData : ScriptableObject
{
    [SerializeField] Vector3 initialPos;
    [SerializeField] Vector3 offset;
    [SerializeField] float speed;
    [SerializeField] float yDamper;
    [SerializeField] float xDamper;
    [SerializeField] float zDamper;

    public Vector3 Offset { get => offset; set => offset = value; }
    public float Speed { get => speed; set => speed = value; }
    public float YDamper { get => yDamper; set => yDamper = value; }
    public float XDamper { get => xDamper; set => xDamper = value; }
    public float ZDamper { get => zDamper; set => zDamper = value; }
    public Vector3 InitialPos { get => initialPos; set => initialPos = value; }
}
