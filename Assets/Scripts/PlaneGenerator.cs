//////////////////////////////////////////////////////////////////////////////
// SPECIAL THANKS TO https://github.com/izmhr/KinectV2DepthPoints for the   //
// CODE [now modified] AND INSPIRATION                                      //
//////////////////////////////////////////////////////////////////////////////



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
	// width
	[SerializeField]
	private int w = 1024/2;

	// height
	[SerializeField]
	private int h = 844/2;


	[SerializeField]
	private float pitch = .01f;

	void Awake()
	{
		GetComponent<MeshFilter>().mesh = MeshGenerate();
	}

	Mesh GenerateMesh(int w, int h, float pitch)
	{
		Mesh mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

		Vector3[] vertices = new Vector3[w * h];
		
		for (var y = 0; y < h; y++)
		{
			for (var x = 0; x < w; x++)

			{
				vertices[y * w + x] = new Vector3((float)(x - w / 2) * pitch, (float)(y - h / 2) * pitch, 0f);
			}
		}
		mesh.vertices = vertices;

		Vector2[] uv = new Vector2[w * h];
		for (var y = 0; y < h; y++)
		{
			for (var x = 0; x < w; x++)
			{
				uv[y * w + x] = new Vector2(((float)x + 0.5f) / (float)w, ((float)y + 0.5f) / (float)h);
			}
		}
		mesh.uv = uv;

		int[] triangles = new int[(w - 1) * (h - 1) * 2 * 3];
		for (var y = 0; y < h - 1; y++)
		{
			for (var x = 0; x < w - 1; x++)
			{
				triangles[(y * (w - 1) + x) * 2 * 3 + 0] = (y * w + x);
				triangles[(y * (w - 1) + x) * 2 * 3 + 1] = (y * w + x + 1);
				triangles[(y * (w - 1) + x) * 2 * 3 + 2] = ((y + 1) * w + x + 1);
				triangles[(y * (w - 1) + x) * 2 * 3 + 3] = (y * w + x);
				triangles[(y * (w - 1) + x) * 2 * 3 + 4] = ((y + 1) * w + x + 1);
				triangles[(y * (w - 1) + x) * 2 * 3 + 5] = ((y + 1) * w + x);
			}
		}
		mesh.triangles = triangles;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	Mesh MeshGenerate()
	{
		return GenerateMesh(w, h, pitch);
	}
}
