using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LiftConnection : MonoBehaviour
{
    [SerializeField] Lift lift;
    [SerializeField] LineRenderer line;
    [SerializeField] LineRenderer overlayLine;
    private void Awake()
    {
        lift = GetComponentInParent<Lift>();
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        ConnectionLine();
    }
    void ConnectionLine()
    {
        line.SetPosition(0, lift.startPointTransform.localPosition);
        line.SetPosition(1, lift.endPointTransform.localPosition);

        overlayLine.SetPosition(0, lift.startPointTransform.localPosition);
        overlayLine.SetPosition(1, lift.endPointTransform.localPosition);
    }
}
