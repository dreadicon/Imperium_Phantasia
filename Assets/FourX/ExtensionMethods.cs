using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

	public static class ExtensionMethods  {

		public static Color ColorFromVector3 (this Vector3 value) {
			return new Color (value.x, value.y, value.z, 1);
		}

		public static Vector3 Vector3FromColor (this Color value) {
			return new Vector3 (value.r, value.g, value.b);
		}

		public static SerializableVector3 VectorSerialize (this Vector3 value) {
			return new SerializableVector3(value.x, value.y, value.z);
		}

	}

	[Serializable]
	public class SerializableVector3 {
		public SerializableVector3 (float _x, float _y, float _z) {
			this.x = _x;
			this.y = _y;
			this.z = _z;
		}
		public float x;
		public float y;
		public float z;

		public Vector3 VectorDeserialize () {
			return new Vector3 (x, y, z);
		}
	}

	/// <summary>
	/// SERIALIZATION SURROGACY FOR UNITY'S FREAKING STUPID TYPES. SERIOUSLY, UNITY, WTH.
	/// </summary>

public class Vector3Surrogate : ISerializationSurrogate {
	/// <summary>
	/// Manually add objects to the <see cref="SerializationInfo"/> store.
	/// </summary>
	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context) {
		Vector3 vector = (Vector3) obj;
		info.AddValue("x", vector.x);
		info.AddValue("y", vector.y);
		info.AddValue("z", vector.z);
	}
	
	/// <summary>
	/// Retrieves objects from the <see cref="SerializationInfo"/> store.
	/// </summary>
	/// <returns></returns>
	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector) {
		Vector3 vector = (Vector3)obj;
		vector.x = info.GetSingle("x");
		vector.y = info.GetSingle("y");
		vector.z = info.GetSingle("z");
		return vector;
	}

}







