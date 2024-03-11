using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Vector3   positionOffset;
    public float     smoothSpeed = 100f;

    private void Start() {

#if UNITY_EDITOR
        if(target == null) {
            Debug.LogError("CameraController: target is not assigned!");
            Debug.Break();
        }
#endif

    }

    private void LateUpdate() {

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, 
                                                target.position + positionOffset, 
                                                smoothSpeed * Time.deltaTime);


        transform.position = smoothedPosition;

        transform.LookAt(target);

    }

    public void ZoomOut() {
        positionOffset.z += 1f;
    }
   
}
