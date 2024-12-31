using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public static MapGenerator Instance { get; private set; }

    [SerializeField]
    private GameObject planePrefab; // The original plane asset

    [SerializeField]
    private int gridSize = 3; // The size of the grid (3x3)

    [SerializeField]
    private List<GameObject> plantPrefabs; // List of plant types to spawn

    [SerializeField]
    private GameObject wallPrefab; // The wall prefab

    private Vector3 planeSize; // Size of the plane for positioning

    private Vector3 mapBoundsMin; // Minimum bounds of the map
    private Vector3 mapBoundsMax; // Maximum bounds of the map

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (planePrefab == null)
        {
            Debug.LogError("Plane prefab is not assigned!");
            return;
        }

        // Calculate the size of the plane
        MeshRenderer planeRenderer = planePrefab.GetComponent<MeshRenderer>();
        if (planeRenderer != null)
        {
            planeSize = planeRenderer.bounds.size;
        }
        else
        {
            Debug.LogError("Plane prefab does not have a MeshRenderer component!");
            return;
        }

        GenerateMap();
        CalculateMapBounds();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < gridSize + 1; x++)
        {
            for (int z = 0; z < gridSize + 1; z++)
            {
                // Calculate the position for each plane
                Vector3 position = new Vector3(x * planeSize.x, 2, z * planeSize.z);

                // Instantiate a new plane at the calculated position
                GameObject newPlane = Instantiate(planePrefab, position, Quaternion.identity);

                // Optional: Parent the planes to the MapGenerator for organization
                newPlane.transform.parent = this.transform;

                newPlane.layer = LayerMask.NameToLayer("Ground");

                // Spawn plants on this plane
                SpawnPlantsOnPlane(newPlane.transform.position);
            }
        }

        GenerateWalls();
    }

    private void SpawnPlantsOnPlane(Vector3 planePosition)
    {

        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            // Choose a random plant prefab
            GameObject plantPrefab = plantPrefabs[Random.Range(0, plantPrefabs.Count)];

            // Calculate a random position within the bounds of the plane
            float randomX = Random.Range(planePosition.x, planePosition.x + planeSize.x);
            float randomZ = Random.Range(planePosition.z, planePosition.z + planeSize.z);
            Vector3 plantPosition = new Vector3(randomX, planePosition.y, randomZ);

            // Instantiate the plant at the calculated position
            GameObject newPlant = Instantiate(plantPrefab, plantPosition, Quaternion.identity);

            // Optional: Parent the plants to the MapGenerator for organization
            newPlant.transform.parent = this.transform;
        }
    }

    //private void GenerateWalls()
    //{
    //    // Calculate map bounds
    //    float mapWidth = gridSize * planeSize.x;
    //    float mapHeight = gridSize * planeSize.z;

    //    // Place fence parts for each side of the map
    //    PlaceFence(new Vector3(0, 2, mapHeight + planeSize.z / 2), mapWidth, true);  // Top
    //    PlaceFence(new Vector3(0, 2, -planeSize.z / 2), mapWidth, true);            // Bottom
    //    PlaceFence(new Vector3(-planeSize.x / 4, 2, 0), mapHeight, false);          // Left
    //    PlaceFence(new Vector3(mapWidth + planeSize.x / 4, 2, 0), mapHeight, false); // Right
    //}

    private void GenerateWalls()
    {
        // Calculate map bounds
        float mapWidth = gridSize * planeSize.x;
        float mapHeight = gridSize * planeSize.z;

        // Place fence parts for each side of the map
        PlaceFence(new Vector3(0, 2, mapHeight + planeSize.z / 2), mapWidth, true);  // Top
        PlaceFence(new Vector3(0, 2, -planeSize.z / 2), mapWidth, true);            // Bottom
        PlaceFence(new Vector3(-planeSize.x / 2, 2, 0), mapHeight, false);          // Left
        PlaceFence(new Vector3(mapWidth + planeSize.x / 2, 2, 0), mapHeight, false); // Right

        // Add fences at the four corners to close gaps
        PlaceCornerFence(new Vector3(-planeSize.x / 2, 2, -planeSize.z / 2)); // Bottom-left corner
        PlaceCornerFence(new Vector3(-planeSize.x / 2, 2, mapHeight + planeSize.z / 2)); // Top-left corner
        PlaceCornerFence(new Vector3(mapWidth + planeSize.x / 2, 2, -planeSize.z / 2)); // Bottom-right corner
        PlaceCornerFence(new Vector3(mapWidth + planeSize.x / 2, 2, mapHeight + planeSize.z / 2)); // Top-right corner
    }

    private void PlaceFence(Vector3 startPosition, float length, bool isHorizontal)
    {
        // Determine the size of a single fence piece
        MeshRenderer fenceRenderer = wallPrefab.GetComponent<MeshRenderer>();
        float fenceSize = isHorizontal ? fenceRenderer.bounds.size.x : fenceRenderer.bounds.size.z;

        // Calculate the number of fence pieces needed
        int numberOfFences = Mathf.CeilToInt(length / fenceSize);

        // Loop to instantiate fence pieces along the specified axis
        for (int i = 0; i < numberOfFences + 1; i++)
        {
            Vector3 position;
            Quaternion rotation;

            if (isHorizontal)
            {
                // Horizontal placement (along X-axis)
                position = new Vector3(startPosition.x + i * fenceSize, 2, startPosition.z);
                rotation = Quaternion.identity; // Default rotation
            }
            else
            {
                // Vertical placement (along Z-axis)
                position = new Vector3(startPosition.x, 2, startPosition.z + i * fenceSize);
                rotation = Quaternion.Euler(0, 90, 0); // Rotate 90 degrees on the Y-axis
            }

            // Instantiate the fence piece
            GameObject newFence = Instantiate(wallPrefab, position, rotation);
            newFence.transform.parent = this.transform; // Parent for organization
        }
    }

    private void PlaceCornerFence(Vector3 position)
    {
        // Place a single fence piece at the given position
        GameObject cornerFence = Instantiate(wallPrefab, position, Quaternion.identity);
        cornerFence.transform.parent = this.transform; // Parent for organization
    }


    private void CalculateMapBounds()
    {
        mapBoundsMin = new Vector3(0, 2, 0);
        mapBoundsMax = new Vector3(gridSize * planeSize.x, 2, gridSize * planeSize.z);
    }

    public Vector3 GetRandomPositionWithinBounds()
    {
        float randomX = Random.Range(mapBoundsMin.x, mapBoundsMax.x);
        float randomZ = Random.Range(mapBoundsMin.z, mapBoundsMax.z);
        return new Vector3(randomX, 2, randomZ);
    }

}