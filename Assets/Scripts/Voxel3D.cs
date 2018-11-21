using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxel3D
{
    public bool state;

    public Vector3 position, xEdgePos, yEdgePos, zEdgePos;

    public Voxel3D(int x, int y, int z, float size)
    {
        position.x = (x + 0.5f) * size;
        position.y = (y + 0.5f) * size;
		position.z = (z + 0.5f) * size;

        xEdgePos = position;
        xEdgePos.x += size * 0.5f;
        yEdgePos = position;
        yEdgePos.y += size * 0.5f;
    }
}
