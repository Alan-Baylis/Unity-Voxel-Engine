using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelGenerator
{
    /// <summary> Class for creating a new voxel grid object </summary>
    /// <remarks> A voxel grid can be used as a chunk in the map. Grids do not have to be perfect squares </remarks>
    [SelectionBase]
    public class VoxelGrid3D : MonoBehaviour
    {
        #region Class Variables
        public int resolution;                  // Number of voxels
        public GameObject voxelPrefab;          // Voxel GameObject, in this case a cube

        private Mesh mesh;
        private List <Vector3> vertices;
        private List <int> triangles;

        private float voxelSize;                // Size of the voxel
        private Voxel3D[] voxels;                  // Array of voxels
        private Material[] voxelMaterials;     // Material to be set to each voxel
        #endregion

        #region Initialization Methods
        /// <summary> Initializes the object upon game starting </summary>
        /// <param name="resolution"> Number of voxels per grid </param>
        /// <param name="size"> Size of each voxel </param>
        public void Initialize(int resolution, float size)
        {
            // Set the resolution for the grid
            this.resolution = resolution;
            // Set the size of each voxel
            voxelSize = size / resolution;
            // Set the voxels array. Because the chunk is 3D, you have to create an array of
            // x * y * z in order to create the grid
            voxels = new Voxel3D[resolution * resolution * resolution];
            // Set the voxel materials array
            voxelMaterials = new Material[voxels.Length];
            // Loop through the x, y, and z axes and create a voxel at those points
            for (int i = 0, y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    for (int x = 0; x < resolution; x++, i++)
                    {
                        CreateVoxel(i, x, y, z);
                    }
                }
            }

            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Terrain";
            vertices = new List<Vector3>();
            triangles = new List<int>();
            Refresh();
        }

        /// <summary> Create a new voxel GameObjext at x, y, and z, and set the transform and scale </summary>
        /// <param name="i"> Current index in the voxels array </param>
        /// <param name="x"> Position for the voxel to be made at on the X-axis </param>
        /// <param name="y"> Position for the voxel to be made at on the Y-axis </param>
        /// <param name="z"> Position for the voxel to be made at on the Z-axis </param>
        private void CreateVoxel(int i, int x, int y, int z)
        {
            // Create a game object
            GameObject o = Instantiate(voxelPrefab) as GameObject;
            // Set the trasform, position, and scale
            o.transform.parent = transform;
            o.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize, (z + 0.5f) * voxelSize);
            o.transform.localScale = Vector3.one * voxelSize * 0.9f;
            voxelMaterials[i] = o.GetComponent<MeshRenderer>().material;
            voxels[i] = new Voxel3D(x, y, z, voxelSize);
        }

        /// <summary> Enable the voxel based on where the user clicked </summary>
        /// <param name="x"> X point where user clicked </param>
        /// <param name="y"> Y point where user clicked </param>
        /// <param name="z"> Z point where user clicked </param>
        /// <param name="state"> If the voxel should be enabled or disabled </param>
        public void SetVoxel(int x, int y, int z, bool state)
        {
            voxels[(y * resolution * resolution) + (z * resolution) + x].state = state;
            Refresh();
        }

        /// <summary> Set the color of the voxel based on it's stata </summary>
        private void SetVoxelColors()
        {
            for (int i = 0; i < voxels.Length; i++)
            {
                voxelMaterials[i].color = voxels[i].state ? Color.black : Color.white;
            }
        }

        private void Refresh()
        {
            SetVoxelColors();
            Triangulate();
        }

        private void Triangulate()
        {
            int cells = resolution - 1;
            for(int i = 0, y = 0; y < cells; y++, i++)
            {
                for(int z = 0; z < cells; z++, i++)
                {
                    for(int x = 0; x < cells; x++, i++)
                    {
                        Voxel3D[] grid = {voxels[i],
                                          voxels[i + 1],
                                          voxels[i + resolution],
                                          voxels[i + resolution + 1]};
                    }
                }
            }
        }

        private void TriangulateCell(Voxel3D[] grid)
        {

        }
        #endregion
    }
}
