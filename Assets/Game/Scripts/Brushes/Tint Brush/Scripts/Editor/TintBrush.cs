// -----------------------------------------------------
// File: TintBrush.cs
// -----------------------------------------------------

namespace com.sheridancollege
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.Tilemaps;

	[CustomGridBrush(false, false, false, "Tint Brush")]
	public class TintBrush : GridBrushBase 
	{
		public Color tint = Color.white;

		public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
		{
            if (brushTarget.layer == 31)
            {
                return;
            }

			Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
			if (tilemap != null)
			{
				TileBase tile = tilemap.GetTile(position);
				if (tile != null)
				{
                    //Undo.RegisterCompleteObjectUndo(tilemap, "TintChange Color");
                    TileFlags tileFlag = tilemap.GetTileFlags(position);
                    if ((tileFlag & TileFlags.LockColor) != 0)
                    {
                        tilemap.SetTileFlags(position, (tileFlag & ~TileFlags.LockColor));
                    }
                    tilemap.SetColor(position, tint);
					//EditorUtility.SetDirty(tile);
				}
			}
		}
	}
}