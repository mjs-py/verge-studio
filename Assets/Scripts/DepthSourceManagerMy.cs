// SPECIAL THANKS TO https://github.com/izmhr/KinectV2DepthPoints for the   
// CODE [now modified] AND INSPIRATION                                                    

using System;
using UnityEngine;
using Windows.Kinect;

public class DepthSourceManagerMy : MonoBehaviour
{
	private KinectSensor _Sensor;
	private DepthFrameReader _Reader;

	private ushort[] _Data;
	private byte[] _RawData;
	//private byte[] _RawDataPre;
	private Texture2D _Texture;

	[SerializeField]
	private Material mat;

	public ushort[] GetData()
	{
		return _Data;
	}

	void Start()
	{
		_Sensor = KinectSensor.GetDefault();

		if (_Sensor != null)
		{
			_Reader = _Sensor.DepthFrameSource.OpenReader();
			var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
			_Data = new ushort[frameDesc.LengthInPixels];
			//_RawDataPre = new byte[frameDesc.LengthInPixels * 2];
			_RawData = new byte[frameDesc.LengthInPixels * 2];
			
			_Texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.R16, false);
			if (!_Sensor.IsOpen)
			{
				_Sensor.Open();
			}
		}
		mat.SetTexture("_MainTex", _Texture);
	}

	public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
	{
		byte[] _bytes =_texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(_fullPath, _bytes);
		Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
	}

	void Update()
	{
		if (_Reader != null)
		{
			var frame = _Reader.AcquireLatestFrame();
			var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
			if (frame != null)
			{
				frame.CopyFrameDataToArray(_Data);
				
				// Convert ushort[] into byte[] and back
				Buffer.BlockCopy(_Data, 0, _RawData, 0, _Data.Length * 2);
		
				_Texture.LoadRawTextureData(_RawData);
				_Texture.Apply();
				
				frame.Dispose();
				frame = null;
			}
		}
	}

	void OnApplicationQuit()
	{
		if (_Reader != null)
		{
			_Reader.Dispose();
			_Reader = null;
		}

		if (_Sensor != null)
		{
			if (_Sensor.IsOpen)
			{
				_Sensor.Close();
			}

			_Sensor = null;
		}
	}
}
