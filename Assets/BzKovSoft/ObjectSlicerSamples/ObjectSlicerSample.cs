using BzKovSoft.ObjectSlicer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BzKovSoft.ObjectSlicerSamples
{
	public class ObjectSlicerSample : BzSliceableObjectBase
	{
		[Header("Slice Effects")]
		[SerializeField] private float sliceForce = 8f;
		[SerializeField] private float torqueForce = 10f;
		[SerializeField] private float gravityScale = 1f;
		[SerializeField] private float destroyDelay = 5f;
		//[SerializeField] private float separationForce = 3f; // Lực tách hai miếng ra

		protected override BzSliceTryData PrepareData(Plane plane)
		{
			// remember some data. Later we could use it after the slice is done.
			// here I add Stopwatch object to see how much time it takes
			// and vertex count to display.
			ResultData addData = new ResultData();

			// count vertices
			var filters = GetComponentsInChildren<MeshFilter>();
			for (int i = 0; i < filters.Length; i++)
			{
				addData.vertexCount += filters[i].sharedMesh.vertexCount;
			}

			// remember start time
			addData.stopwatch = Stopwatch.StartNew();

			// save the plane for later use
			addData.slicePlane = plane;

			// colliders that will be participating in slicing
			var colliders = gameObject.GetComponentsInChildren<Collider>();

			// return data
			return new BzSliceTryData()
			{
				// componentManager: this class will manage components on sliced objects
				componentManager = new StaticComponentManager(gameObject, plane, colliders),
				plane = plane,
				addData = addData,
			};
		}

		protected override void OnSliceFinished(BzSliceTryResult result)
		{
			if (!result.sliced)
				return;

			// on sliced, get data that we saved in 'PrepareData' method
			var addData = (ResultData)result.addData;
			addData.stopwatch.Stop();
			drawText += gameObject.name +
				". VertCount: " + addData.vertexCount.ToString() + ". ms: " +
				addData.stopwatch.ElapsedMilliseconds.ToString() + Environment.NewLine;

			if (drawText.Length > 1500) // prevent very long text
				drawText = drawText.Substring(drawText.Length - 1000, 1000);

			// Apply physics effects to sliced pieces
			ApplySliceEffects(result);
		}

		private void ApplySliceEffects(BzSliceTryResult result)
		{
			// Get the saved plane from addData
			var addData = (ResultData)result.addData;
			Plane slicePlane = addData.slicePlane;

			// Get the sliced objects
			var slicedObjects = new List<GameObject>();

			// Add the main sliced objects
			if (result.outObjectNeg != null) slicedObjects.Add(result.outObjectNeg);
			if (result.outObjectPos != null) slicedObjects.Add(result.outObjectPos);

			// Main direction: DOWN and slightly to the sides
			Vector3 baseDirection = Vector3.down;

			// Apply forces to each sliced piece
			foreach (var piece in slicedObjects)
			{
				if (piece == null) continue;

				// Add Rigidbody if not present
				Rigidbody rb = piece.GetComponent<Rigidbody>();
				if (rb == null)
				{
					rb = piece.AddComponent<Rigidbody>();
				}

				// Set gravity scale
				rb.useGravity = true;
				if (rb.mass > 0)
					rb.mass *= gravityScale;

				// Simple downward direction (uncomment this for straight down fall)
				Vector3 forceDirection = baseDirection;

				// OR use separation (comment above line and uncomment below for side separation)
				/*
				Vector3 sideDirection = Vector3.zero;
				if (slicedObjects.IndexOf(piece) % 2 == 0)
				{
					sideDirection = Vector3.left * separationForce;
				}
				else
				{
					sideDirection = Vector3.right * separationForce;
				}
				Vector3 forceDirection = baseDirection + sideDirection * 0.3f;
				*/

				// Add small random variation for natural look
				Vector3 randomOffset = new Vector3(
					UnityEngine.Random.Range(-0.2f, 0.2f),
					UnityEngine.Random.Range(-0.1f, 0.1f), // Keep mostly downward
					UnityEngine.Random.Range(-0.1f, 0.1f)
				);

				// Final force direction - ensure it's pointing down
				Vector3 finalForce = (forceDirection + randomOffset).normalized * sliceForce;

				// Make sure Y component is negative (downward)
				if (finalForce.y > 0)
					finalForce.y = -Mathf.Abs(finalForce.y);

				// Apply force
				rb.AddForce(finalForce, ForceMode.Impulse);

				// Add random torque for spinning effect
				Vector3 randomTorque = new Vector3(
					UnityEngine.Random.Range(-torqueForce, torqueForce),
					UnityEngine.Random.Range(-torqueForce, torqueForce),
					UnityEngine.Random.Range(-torqueForce, torqueForce)
				);
				rb.AddTorque(randomTorque * 0.7f, ForceMode.Impulse);

				// Destroy the piece after some time to prevent clutter
				Destroy(piece, destroyDelay);
			}
		}

		static string drawText = "-";

		//void OnGUI()
		//{
		//	GUI.Label(new Rect(10, 10, 2000, 2000), drawText);
		//}

		// DTO that we pass to slicer and then receive back
		class ResultData
		{
			public int vertexCount;
			public Stopwatch stopwatch;
			public Plane slicePlane; // Add this to store the slice plane
		}
	}
}