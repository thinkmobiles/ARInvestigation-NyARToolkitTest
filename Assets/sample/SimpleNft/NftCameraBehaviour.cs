using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;
using jp.nyatla.nyartoolkit.cs.markersystem;
using jp.nyatla.nyartoolkit.cs.core;
using NyARUnityUtils;
using System.IO;

/// <summary>
/// AR camera behaviour.
/// This sample shows simpleLite demo.
/// 1.Connect webcam to your computer.
/// 2.Start sample program
/// 3.Take a "HIRO" marker on capture image
/// 
/// </summary>
public class NftCameraBehaviour : MonoBehaviour
{
	private NyARUnityNftSystem _ns;
	private NyARUnityWebCam _ss;
	private int mid;//marker id
	private GameObject _bg_panel;
	void Awake()
	{
		//setup unity webcam
		WebCamDevice[] devices= WebCamTexture.devices;
		if (devices.Length<=0){
			UnityEngine.Debug.LogError("No Webcam.");
			return;
		}
		WebCamTexture w=new WebCamTexture(640,480,30);
		//Make WebcamTexture wrapped Sensor.
		this._ss=NyARUnityWebCam.createInstance(w);
        //Make configulation by Sensor size.
        NyARNftSystemConfig config = new NyARNftSystemConfig(
            new MemoryStream(((TextAsset)Resources.Load("camera_para5", typeof(TextAsset))).bytes),
            this._ss.width,this._ss.height);

		this._ns = new NyARUnityNftSystem(config);
		//This line loads a marker from texture
		mid=this._ns.addNftTarget(new MemoryStream(((TextAsset)Resources.Load("infinitycat", typeof(TextAsset))).bytes),160);

		//setup background
		this._bg_panel=GameObject.Find("Plane");
		this._bg_panel.GetComponent<Renderer>().material.mainTexture=w;
		this._ns.setARBackgroundTransform(this._bg_panel.transform);
		
		//setup camera projection
		this._ns.setARCameraProjection(this.GetComponent<Camera>());
		return;
	}
	// Use this for initialization
	void Start ()
	{
		//start sensor
		this._ss.start();
	}
	// Update is called once per frame
	void Update ()
	{
		//Update SensourSystem
		this._ss.update();
		//Update marker system by ss
		this._ns.update(this._ss);
		//update Gameobject transform
		if(this._ns.isExist(mid)){
			this._ns.setTransform(mid,GameObject.Find("MarkerObject").transform);
		}else{
            // hide Game object
            GameObject.Find("Cube").transform.localPosition = new Vector3(-80,60,20);
            GameObject.Find("MarkerObject").transform.localPosition=new Vector3(0,0,0);
		}
	}
    void OnDestory()
    {
        this._ns.shutdown();
    }
}
