using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEditor
{
    [CustomEditor(typeof(PrefabBrush))]
    public class PrefabBrushEditor : GridBrushEditor { }

    [CreateAssetMenu(fileName = "Prefab Brush", menuName = "Brushes/PrefabBrush")]
    [CustomGridBrush(false, true, false, "Prefab Brush")]
    public class PrefabBrush : GridBrush
    {
        public GameObject prefab;
        public int mZ;

        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            if (brushTarget.layer == 31)
            {
                return;
            }

            // Create an instance of the prefab
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            // if its created
            if (instance)
            {
                Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Chest Brush");

                // Make sure you set the instances' parent to the brushTarget
                instance.transform.SetParent(brushTarget.transform);
                instance.transform.position = gridLayout.LocalToWorld(
                                                    gridLayout.CellToLocalInterpolated(
                                                        new Vector3Int(position.x, position.y, mZ) +
                                                        new Vector3(0.5f, 0.5f, 0.5f)
                                                        ));
            }
            //base.Paint(gridLayout, brushTarget, position);
        }
    }
}
