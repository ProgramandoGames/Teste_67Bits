using UnityEngine;

public class Raycasting : MonoBehaviour {

    private float rayLength = 2f;

    public bool Detects(out RaycastHit hit, LayerMask layer) {

        bool hitsTarget = Physics.Raycast(transform.position,
                                          transform.forward,
                                          out hit,
                                          rayLength,
                                          layer);
        return hitsTarget;

    }

}
