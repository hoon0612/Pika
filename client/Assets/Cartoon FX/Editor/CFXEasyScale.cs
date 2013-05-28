using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

public class CFXEasyScale : EditorWindow
{
	[MenuItem("GameObject/CartoonFX Easy Particle System Scale")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CFXEasyScale));
	}
	
	private bool IncludeChildren = true;
	private float ScalingValue = 2.0f;
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0,0,this.position.width,this.position.height));
		GUILayout.Label("",GUILayout.Height(10));
		
		GUILayout.Label("You can easily scale a Prefab's\nparticle systems with this script!");
		GUILayout.FlexibleSpace();
		
		IncludeChildren = GUILayout.Toggle(IncludeChildren, "Include Children");		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Multiplier:",GUILayout.Width(70));
		ScalingValue = EditorGUILayout.FloatField(ScalingValue);
		if(ScalingValue <= 0) ScalingValue = 0.1f;
		if(GUILayout.Button("Scale Selected",GUILayout.Width(110)))
		{
			applyScale();
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("",GUILayout.Height(10));		
		GUILayout.EndArea();
	}
	
	private void applyScale()
	{
		foreach(GameObject go in Selection.gameObjects)
		{
			if(IncludeChildren)
			{
				//Scale Shuriken Particles Values
				ParticleSystem[] systems = go.GetComponentsInChildren<ParticleSystem>(true);
				foreach(ParticleSystem ps in systems)
				{
					ScaleParticleValues(ps, go);
				}
				
				//Scale Lights' range
				Light[] lights = go.GetComponentsInChildren<Light>();
				foreach(Light light in lights)
				{
					light.range *= ScalingValue;
					light.transform.localPosition *= ScalingValue;
				}
			}
			else
			{
				ParticleSystem[] systems = go.GetComponents<ParticleSystem>();
				foreach(ParticleSystem ps in systems)
				{
					//Scale Shuriken Particles Values
					ScaleParticleValues(ps, go);
				}
				
				//Scale Lights' range
				Light[] lights = go.GetComponents<Light>();
				foreach(Light light in lights)
				{
					light.range *= ScalingValue;
					light.transform.localPosition *= ScalingValue;
				}
			}
			
		}
		
	}
	
	private void ScaleParticleValues(ParticleSystem ps, GameObject parent)
	{
		//Particle System
		ps.startSize *= ScalingValue;
		if(ps.startSpeed > 0.01f) ps.startSpeed *= ScalingValue;
		if(ps.gameObject != parent)
			ps.transform.localPosition *= ScalingValue;
		
		SerializedObject psSerial = new SerializedObject(ps);
		
		//Scale Velocity Module
		if(psSerial.FindProperty("VelocityModule.enabled").boolValue)
		{
			psSerial.FindProperty("VelocityModule.x.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("VelocityModule.x.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("VelocityModule.x.maxCurve").animationCurveValue);
			psSerial.FindProperty("VelocityModule.y.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("VelocityModule.y.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("VelocityModule.y.maxCurve").animationCurveValue);
			psSerial.FindProperty("VelocityModule.z.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("VelocityModule.z.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("VelocityModule.z.maxCurve").animationCurveValue);
		}
		
		//Scale Limit Velocity Module
		if(psSerial.FindProperty("ClampVelocityModule.enabled").boolValue)
		{
			psSerial.FindProperty("ClampVelocityModule.x.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.x.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.x.maxCurve").animationCurveValue);
			psSerial.FindProperty("ClampVelocityModule.y.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.y.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.y.maxCurve").animationCurveValue);
			psSerial.FindProperty("ClampVelocityModule.z.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.z.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.z.maxCurve").animationCurveValue);
			
			psSerial.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= ScalingValue;
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.magnitude.minCurve").animationCurveValue);
			IterateKeys(psSerial.FindProperty("ClampVelocityModule.magnitude.maxCurve").animationCurveValue);
		}
		
		//Scale Shape Module
		if(psSerial.FindProperty("ShapeModule.enabled").boolValue)
		{
			psSerial.FindProperty("ShapeModule.boxX").floatValue *= ScalingValue;
			psSerial.FindProperty("ShapeModule.boxY").floatValue *= ScalingValue;
			psSerial.FindProperty("ShapeModule.boxZ").floatValue *= ScalingValue;
			psSerial.FindProperty("ShapeModule.radius").floatValue *= ScalingValue;
			
			//Create a new scaled Mesh if there is a Mesh reference
			//(ShapeModule.type 6 == Mesh)
			if(psSerial.FindProperty("ShapeModule.type").intValue == 6)
			{
				Object obj = psSerial.FindProperty("ShapeModule.m_Mesh").objectReferenceValue;
				if(obj != null)
				{
					Mesh mesh = (Mesh)obj;
					string newMeshPath = AssetDatabase.GetAssetPath(mesh) + " x"+ScalingValue+" (scaled).asset";
					Mesh scaledMesh = (Mesh)AssetDatabase.LoadAssetAtPath(newMeshPath, typeof(Mesh));
					if(scaledMesh == null)
					{
						scaledMesh = DuplicateAndScaleMesh(mesh);
						AssetDatabase.CreateAsset(scaledMesh, newMeshPath);
					}
					
					//Apply new Mesh
					psSerial.FindProperty("ShapeModule.m_Mesh").objectReferenceValue = scaledMesh;
				}
			}
		}
		
		//Apply Modified Properties
		psSerial.ApplyModifiedProperties();
	}
	
	private void IterateKeys(AnimationCurve curve)
	{
		for(int i = 0; i < curve.keys.Length; i++)
		{
			curve.keys[i].value *= ScalingValue;
		}
	}
	
	private Mesh DuplicateAndScaleMesh(Mesh mesh)
	{
		Mesh scaledMesh = new Mesh();
		
		Vector3[] scaledVertices = new Vector3[mesh.vertices.Length];
		for(int i = 0; i < scaledVertices.Length; i++)
		{
			scaledVertices[i] = mesh.vertices[i] * ScalingValue;
		}
		scaledMesh.vertices = scaledVertices;
		
		scaledMesh.normals = mesh.normals;
		scaledMesh.tangents = mesh.tangents;
		scaledMesh.triangles = mesh.triangles;
		scaledMesh.uv = mesh.uv;
		scaledMesh.uv2 = mesh.uv2;
		scaledMesh.colors = mesh.colors;
		
		return scaledMesh;
	}
}
