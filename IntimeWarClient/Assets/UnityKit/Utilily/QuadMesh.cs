using UnityEngine;
using System.Collections;

namespace Haruna.Utility
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	public class QuadMesh : MonoBehaviour
	{
		public float _width = 1f;
		public float _height = 1f;

		public float _uvBorder = 0.0f;
		public float _uvRatioWidth = 1f;
		public float _uvRatioHeight = 1f;

		MeshFilter meshFilter;

		public bool _update = true;

		void Start()
		{
			var calcRatioWidth = _uvBorder + (1 - _uvBorder * 2) * _uvRatioWidth;
			var calcRatioHeight = _uvBorder + (1 - _uvBorder * 2) * _uvRatioHeight;

			meshFilter = GetComponent<MeshFilter>();

			var mesh = new Mesh();
			meshFilter.mesh = mesh;
			
			var vertices = new Vector3[4];
			
			vertices[0] = new Vector3(0, _height * calcRatioHeight / -2f, 0);
			vertices[1] = new Vector3(_width * calcRatioWidth, _height * calcRatioHeight / -2f, 0);
			vertices[2] = new Vector3(0, _height * calcRatioHeight / 2f, 0);
			vertices[3] = new Vector3(_width * calcRatioWidth, _height * calcRatioHeight / 2f, 0);
			
			mesh.vertices = vertices;
			
			var tri = new int[6];
			
			tri[0] = 0;
			tri[1] = 2;
			tri[2] = 1;
			
			tri[3] = 2;
			tri[4] = 3;
			tri[5] = 1;
			
			mesh.triangles = tri;
			
			var normals = new Vector3[4];
			
			normals[0] = -Vector3.forward;
			normals[1] = -Vector3.forward;
			normals[2] = -Vector3.forward;
			normals[3] = -Vector3.forward;
			
			mesh.normals = normals;
			
			var uv = new Vector2[4];
			
			uv[0] = new Vector2(0, 0);
			uv[1] = new Vector2(calcRatioWidth, 0);
			uv[2] = new Vector2(0, calcRatioHeight);
			uv[3] = new Vector2(calcRatioWidth, calcRatioHeight);
			
			mesh.uv = uv;
		}

		// Update is called once per frame
		void Update()
		{
			if (_update)
			{
				var calcRatioWidth = _uvBorder + (1 - _uvBorder * 2) * _uvRatioWidth;
				var calcRatioHeight = _uvBorder + (1 - _uvBorder * 2) * _uvRatioHeight;

				var vertices = meshFilter.sharedMesh.vertices;
				vertices[0] = new Vector3(0, _height * calcRatioHeight / -2f, 0);
				vertices[1] = new Vector3(_width * calcRatioWidth, _height * calcRatioHeight / -2f, 0);
				vertices[2] = new Vector3(0, _height * calcRatioHeight / 2f, 0);
				vertices[3] = new Vector3(_width * calcRatioWidth, _height * calcRatioHeight / 2f, 0);
				meshFilter.sharedMesh.vertices = vertices;

				var uv = meshFilter.sharedMesh.uv;
				uv[1] = new Vector2(calcRatioWidth, 0);
				uv[2] = new Vector2(0, calcRatioHeight);
				uv[3] = new Vector2(calcRatioWidth, calcRatioHeight);
				meshFilter.sharedMesh.uv = uv;
			}
		}
	}
}