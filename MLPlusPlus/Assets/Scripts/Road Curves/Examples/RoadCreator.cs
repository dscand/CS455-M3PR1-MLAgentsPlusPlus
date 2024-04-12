using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadCreator : MonoBehaviour
{
	public Wall WallRight;
	public Wall WallLeft;

	[Range(.05f, 1.5f)]
	public float spacing = 1;
	public float roadWidth = 1;
	public bool autoUpdate;
	public float tiling = 1;

	public int CheckpointSpacing = 4;
	public GameObject Checkpoint;

	public void UpdateRoad()
	{
		Path path = GetComponent<PathCreator>().path;
		Vector2[] points = path.CalculateEvenlySpacedPoints(spacing);
		GetComponent<MeshFilter>().mesh = CreateRoadMesh(points, path.IsClosed);

		(WallLeft.GetComponent<MeshFilter>().mesh, WallRight.GetComponent<MeshFilter>().mesh) = CreateWallMeshes(points, path.IsClosed);

		int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * .05f);
		GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);


		/*Mesh checkPointmesh = CreateCheckpointMesh(roadWidth);
		foreach (Checkpoint item in GetComponentsInChildren<Checkpoint>())
		{
			DestroyImmediate(item.gameObject);
		}

		Vector2[] checkpointPoints = path.CalculateEvenlySpacedPoints(CheckpointSpacing);
		List<GameObject> checkPoints = new();
		for (int i = 0; i < checkpointPoints.Length; i++) {
			GameObject newCheckpoint = Instantiate(Checkpoint, transform);
			newCheckpoint.transform.localPosition = checkpointPoints[i];


			Vector2 forward = Vector2.zero;
			if (i < checkpointPoints.Length - 1 || path.IsClosed)
			{
				forward += checkpointPoints[(i + 1)%checkpointPoints.Length] - checkpointPoints[i];
			}
			if (i > 0 || path.IsClosed)
			{
				forward += checkpointPoints[i] - checkpointPoints[(i - 1 + checkpointPoints.Length)%checkpointPoints.Length];
			}

			forward.Normalize();
			Vector2 left = new Vector2(-forward.y, forward.x);

			newCheckpoint.transform.localRotation = Quaternion.LookRotation(forward, Vector3.up);
			newCheckpoint.transform.localRotation = Quaternion.Euler(new Vector3(
				newCheckpoint.transform.localRotation.eulerAngles.x,
				90f,
				0f
			));


			newCheckpoint.GetComponent<MeshFilter>().mesh = checkPointmesh;
			newCheckpoint.GetComponent<MeshCollider>().sharedMesh = checkPointmesh;
			checkPoints.Add(newCheckpoint);
		}*/
	}

	Mesh CreateRoadMesh(Vector2[] points, bool isClosed)
	{
		Vector3[] verts = new Vector3[points.Length * 2];
		Vector2[] uvs = new Vector2[verts.Length];
		int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
		int[] tris = new int[numTris * 3];
		int vertIndex = 0;
		int triIndex = 0;

		for (int i = 0; i < points.Length; i++)
		{
			Vector2 forward = Vector2.zero;
			if (i < points.Length - 1 || isClosed)
			{
				forward += points[(i + 1)%points.Length] - points[i];
			}
			if (i > 0 || isClosed)
			{
				forward += points[i] - points[(i - 1 + points.Length)%points.Length];
			}

			forward.Normalize();
			Vector2 left = new Vector2(-forward.y, forward.x);

			verts[vertIndex] = points[i] + left * roadWidth * .5f;
			verts[vertIndex + 1] = points[i] - left * roadWidth * .5f;

			float completionPercent = i / (float)(points.Length - 1);
			float v = 1 - Mathf.Abs(2 * completionPercent - 1);
			uvs[vertIndex] = new Vector2(0, v);
			uvs[vertIndex + 1] = new Vector2(1, v);

			if (i < points.Length - 1 || isClosed)
			{
				tris[triIndex] = vertIndex;
				tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
				tris[triIndex + 2] = vertIndex + 1;

				tris[triIndex + 3] = vertIndex + 1;
				tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
				tris[triIndex + 5] = (vertIndex + 3)  % verts.Length;
			}

			vertIndex += 2;
			triIndex += 6;
		}

		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.uv = uvs;

		return mesh;
	}

	(Mesh,Mesh) CreateWallMeshes(Vector2[] points, bool isClosed)
	{
		Mesh meshL = CreateWallMesh(points, isClosed, 0.5f * roadWidth, true);
		Mesh meshR = CreateWallMesh(points, isClosed, -0.5f * roadWidth, false);
		return (meshL, meshR);
	}

	Mesh CreateWallMesh(Vector2[] points, bool isClosed, float offset, bool leftSide)
	{
		Vector3[] verts = new Vector3[points.Length * 2];
		Vector2[] uvs = new Vector2[verts.Length];
		int numTris = 2 * (points.Length - 1) + (isClosed ? 2 : 0);
		int[] tris = new int[numTris * 3];
		int vertIndex = 0;
		int triIndex = 0;

		for (int i = 0; i < points.Length; i++)
		{
			Vector2 forward = Vector2.zero;
			if (i < points.Length - 1 || isClosed)
			{
				forward += points[(i + 1)%points.Length] - points[i];
			}
			if (i > 0 || isClosed)
			{
				forward += points[i] - points[(i - 1 + points.Length)%points.Length];
			}

			forward.Normalize();
			Vector2 left = new Vector2(-forward.y, forward.x);

			Vector2 point = points[i] + offset * left;
			verts[vertIndex] = new Vector3(point.x, point.y, leftSide ? -2f : 0);
			verts[vertIndex + 1] = new Vector3(point.x, point.y, !leftSide ? -2f : 0);

			float completionPercent = i / (float)(points.Length - 1);
			float v = 1 - Mathf.Abs(2 * completionPercent - 1);
			uvs[vertIndex] = new Vector2(0, v);
			uvs[vertIndex + 1] = new Vector2(1, v);

			if (i < points.Length - 1 || isClosed)
			{
				tris[triIndex] = vertIndex;
				tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
				tris[triIndex + 2] = vertIndex + 1;

				tris[triIndex + 3] = vertIndex + 1;
				tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
				tris[triIndex + 5] = (vertIndex + 3)  % verts.Length;
			}

			vertIndex += 2;
			triIndex += 6;
		}

		Mesh mesh = new Mesh();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.uv = uvs;

		return mesh;
	}

	Mesh CreateCheckpointMesh(float roadWidth) {
		Vector3[] verts = new Vector3[4] {
			new(0,roadWidth/2,0),
			new(0,-roadWidth/2,0),
			new(2f,roadWidth/2,0),
			new(2f,-roadWidth/2,0),
		};

		Vector2[] uvs = new Vector2[4] {
			new(0,0),
			new(0,1),
			new(1,1),
			new(1,0),
		};

		int[] tris = new int[6] {
			2,1,0,
			1,2,3,
		};

        Mesh mesh = new() {
			name = "CheckpointMesh",
            vertices = verts,
            triangles = tris,
            uv = uvs
        };

        return mesh;
	}
}
