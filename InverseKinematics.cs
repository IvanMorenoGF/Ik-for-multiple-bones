using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    public float rotSpeed;
    public Transform pivot;
    public Transform pole;
    public Transform[] bones;

    float upperLength { get => (bones[1].position - bones[0].position).magnitude; }
    float forearmLength { get => (bones[2].position - bones[1].position).magnitude; }
    float upperToPoleLength { get => (pole.position - bones[0].position).magnitude; }

    private void Update()
    {
        FacePivotToPole();
        SetSegmentsRotation();
    }

    private void FacePivotToPole() => pivot.rotation = Quaternion.LookRotation(pole.position - pivot.position);

    public void MoveSegment(Transform _segment, float _angle)
    {
        if (!float.IsNaN(_angle))
            _segment.localRotation = Quaternion.Slerp(_segment.localRotation, Quaternion.AngleAxis(_angle, Vector3.right), Time.deltaTime * rotSpeed);
    }

    public void SetSegmentsRotation()
    {
        MoveSegment(bones[0], -GetAngle(upperLength, upperToPoleLength, forearmLength));

        foreach (Transform bone in bones)
        {
            if (bone == bones[0]) continue;

            float angle = GetAngle(upperLength, forearmLength, upperToPoleLength);
            if (float.IsNaN(angle)) angle = 180;

            MoveSegment(bone, 180 - angle);
        }
    }

    private float GetAngle(float _adyacentA, float _adyacentB, float _oposite)
    {
        return Mathf.Acos((Mathf.Pow(_adyacentA, 2) + Mathf.Pow(_adyacentB, 2) - Mathf.Pow(_oposite, 2))
            / (2 * _adyacentA * _adyacentB)) * Mathf.Rad2Deg;
    }
}
