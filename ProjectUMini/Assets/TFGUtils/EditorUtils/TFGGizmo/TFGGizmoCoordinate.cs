using UnityEngine;

namespace TFGUtils.EditorUtils.TFGGizmo
{
    namespace TFGEditorUtils.TFGGizmo
    {
        public class TFGGizmoCoordinate : MonoBehaviour
        {
            private void OnDrawGizmos()
            {
                Debug.DrawRay(transform.position, transform.forward * 100, Color.blue);
                Debug.DrawRay(transform.position, transform.up * 100, Color.green);
                Debug.DrawRay(transform.position, transform.right * 100, Color.red);
            }
        }
    }
}