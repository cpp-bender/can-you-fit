using UnityEngine;

public class FakePlayerController : MonoBehaviour
{
    public float customizedZ;
    public SkinnedMeshRenderer dressRenderer;
    public SkinnedMeshRenderer hairRenderer;
    public SkinnedMeshRenderer wingsRenderer;


    private Transform player;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z - customizedZ);
    }

    public void ChangeDressMatTo(Material newMat)
    {
        dressRenderer.material = newMat;
    }

    public void ChangeHairMatTo(Material newMat)
    {
        hairRenderer.material = newMat;
    }

    public void ChangeWingsMatTo(Material newMat)
    {
        wingsRenderer.material = newMat;
    }
}
