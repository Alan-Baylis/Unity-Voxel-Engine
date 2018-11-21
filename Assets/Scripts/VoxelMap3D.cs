using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelGenerator
{
    /// <summary> Creates a map out of Voxel Grid objects </summary>
    /// <remarks> Grids are reffered to as chunks in this class </remarks>
    public class VoxelMap3D : MonoBehaviour
    {
        #region Class Variables
        public float size = 2.0f;                       // Used to compute the size of the chunk
        public int voxelResolution = 8;                 // Number of voxels per chunk
        public int chunkResolution = 2;                 // Number of grids in a given direction
        public VoxelGrid3D voxelGrid3DPrefab;           // Voxel Grid prefab

        private VoxelGrid3D[] chunks;                   // Stores the chunks in the map
        private float chunkSize, voxelSize, halfSize;   // Variables to be passed to grid elements in chunks
        #endregion

        #region Initialization Methods
        /// <summary> Creates the map when the game starts </summary>
        private void Awake()
        {
            // Used to find the middle of the map
            halfSize = size * 0.5f;
            // Size of each chunk
            chunkSize = size / chunkResolution;
            // Size of each voxel per chunk
            voxelSize = chunkSize / voxelResolution;

            // Initialize the map of chunks
            chunks = new VoxelGrid3D[chunkResolution * chunkResolution * chunkResolution];
            // Assign a new voxel element to a spot on each
            for (int i = 0, y = 0; y < chunkResolution; y++)
            {
                for (int z = 0; z < chunkResolution; z++)
                {
                    for (int x = 0; x < chunkResolution; x++, i++)
                    {
                        CreateChunk(i, x, y, z);
                    }
                }
            }

            // Add a box collider to detect user input
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            // Set the size of the collider
            box.size = new Vector3(size, size, size);
        }
        
        /// <summary> Creates a chunk and adds it to the chunk array </summary>
        /// <param name="i"> Current index in chunks </param>
        /// <param name="x"> Position for the chunk to be made at on the X-axis </param>
        /// <param name="y"> Position for the chunk to be made at on the Y-axis </param>
        /// <param name="z"> Position for the chunk to be made at on the Z-axis </param>
        private void CreateChunk(int i, int x, int y, int z)
        {
            VoxelGrid3D chunk = Instantiate(voxelGrid3DPrefab) as VoxelGrid3D;
            chunk.Initialize(voxelResolution, chunkSize);
            chunk.transform.parent = transform;
            chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize, z * chunkSize - halfSize);
            chunks[i] = chunk;
        }
        #endregion

        #region Update Methods
        /// <summary> Check for mouse input to enable the voxel </summary>
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
                {
                    if (hitInfo.collider.gameObject == gameObject)
                    {
                        EditVoxels(transform.InverseTransformPoint(hitInfo.point));
                    }
                }
            }
        }

        /// <summary> Set the chunk coords and convert the voxel to world space. Then set the voxel to be edited </summary>
        /// <param name="point"> The point which the user clicked </param>
        private void EditVoxels(Vector3 point)
        {
            int voxelX = (int)((point.x + halfSize) / voxelSize);
            int voxelY = (int)((point.y + halfSize) / voxelSize);
            int voxelZ = (int)((point.z + halfSize) / voxelSize);
            int chunkX = voxelX / voxelResolution;
            int chunkY = voxelY / voxelResolution;
            int chunkZ = voxelZ / voxelResolution;

			Debug.Log(voxelX + ", " + voxelY + ", " + voxelZ + " in chunk " + chunkX + ", " + chunkY + ", " + chunkZ);

            voxelX -= chunkX * voxelResolution;
            voxelY -= chunkY * voxelResolution;
			voxelZ -= chunkZ * voxelResolution;
            chunks[(chunkY * chunkResolution * chunkResolution) + (chunkZ * chunkResolution) + chunkX].SetVoxel(voxelX, voxelY, voxelZ, true);
        }
        #endregion
    }
}