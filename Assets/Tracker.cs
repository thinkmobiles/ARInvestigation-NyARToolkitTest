using UnityEngine;
using jp.nyatla.nyartoolkit.cs.markersystem;
using NyARUnityUtils;
using System.IO;

public class Tracker : MonoBehaviour
{
    public Transform MarckerObject;
    private NyARUnityMarkerSystem _ms;
    private NyARUnityWebCam _ss;
    private int mid;
    private GameObject _bg_panel;
    void Awake()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length <= 0)
        {
            Debug.LogError("No Webcam.");
            return;
        }

        WebCamTexture w = new WebCamTexture(320, 240, 15);
        _ss = NyARUnityWebCam.createInstance(w);
        NyARMarkerSystemConfig config = new NyARMarkerSystemConfig(_ss.width, _ss.height);

        _ms = new NyARUnityMarkerSystem(config);
        mid = _ms.addARMarker(
            new MemoryStream(((TextAsset)Resources.Load("patt_hiro", typeof(TextAsset))).bytes),
            16, 25, 80);
        _bg_panel = GameObject.Find("Plane");
        _bg_panel.GetComponent<Renderer>().material.mainTexture = w;
        _ms.setARBackgroundTransform(_bg_panel.transform);        
        _ms.setARCameraProjection(GetComponent<Camera>());
    }

    void Start()
    {
        _ss.start();
    }

    void Update()
    {
        _ss.update();
        _ms.update(_ss);
        if (_ms.isExist(mid))
        {
            MarckerObject.gameObject.SetActive(true);
            _ms.setTransform(mid, MarckerObject);
        }
        else
        {
            MarckerObject.gameObject.SetActive(false);
        }
    }
}
