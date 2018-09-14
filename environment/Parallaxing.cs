using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public List<Transform> backgrounds;

    [SerializeField]
    float[] parallaxScales;

    public float smoothing = 0.5f;

    private Vector3 previousCameraPosition;

    void Awake(){
        previousCameraPosition = transform.position;
    }

	// Use this for initialization
	void Start () {
        
        parallaxScales = new float[backgrounds.Count];

        for (int i = 0; i < parallaxScales.Length; i++){
            parallaxScales[i] = backgrounds[i].position.z;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // updates the movement after camera updates its movement (in its update())
    private void LateUpdate()
    {
        for (int i = 0; i < backgrounds.Count; i++) {
            //print("updating updating");
            Vector3 parallax = (previousCameraPosition - transform.position) * (parallaxScales[i]/smoothing);
            backgrounds[i].position = new Vector3(backgrounds[i].position.x + parallax.x, backgrounds[i].position.y + parallax.y, backgrounds[i].position.z);
        }

        previousCameraPosition = transform.position;
    }
}
