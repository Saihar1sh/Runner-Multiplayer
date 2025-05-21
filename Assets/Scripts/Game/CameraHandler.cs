using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform target; 
    public Camera thisCamera;
    public Vector3 offset = new Vector3(0, 5, -10); 
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target);
    }

    public RenderTexture GetCameraView(RenderTexture renderTextureRef)
    {
        RenderTexture newView = new RenderTexture(renderTextureRef);
        thisCamera.targetTexture = newView;
        return newView;
    }
}