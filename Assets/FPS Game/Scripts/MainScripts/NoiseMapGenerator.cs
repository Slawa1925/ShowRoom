using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGenerator : MonoBehaviour {

	public int GridSide;
	public int HeightMultiplier;
	public float ScaleNoise;
	public Vector3 ColorScaleNoise;
	public GameObject ObjPref;
	public float PositionX;
	public float PositionZ;
	public bool Generated;
	public float PosIncrease;
	public bool Animate;
	public Vector4 ColorChange;
	List<Transform> ObjList = new List<Transform>();
	public int N;
    public float colorX_0;
    public float colorX_1;
    public float colorX_2;
    public float ColorX_0;
	public float ColorX_1;
	public float ColorX_2;
    public float t0;
    public float t1;
    public float t2;
    public Vector3 ColorPosTarg;
	public float ColorSpeed;


	void Update () {

        if (Input.GetKeyDown(KeyCode.H))
        {
            ColorPosTarg = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            GenerateMap();
        }

		if (Generated) {

			for (N = 0; N < ObjList.Count; N++) {

				if (Animate) {

                    if ( t0 <= 1 )
                    {
                        t0 += Time.deltaTime*ColorSpeed;
                        ColorX_0 = colorX_0 - colorX_0 * t0 + t0 * ColorPosTarg.x;
                    }
                    else
                    {
                        ColorPosTarg = new Vector3(Random.Range(0.0f, 1.0f), ColorPosTarg.y, ColorPosTarg.z);
                        colorX_0 = ColorX_0;
                        t0 = 0;
                    }
                    if (t1 <= 1)
                    {
                        t1 += Time.deltaTime * ColorSpeed;
                        ColorX_1 = colorX_1 - colorX_1 * t1 + t1 * ColorPosTarg.y;
                    }
                    else
                    {
                        ColorPosTarg = new Vector3(ColorPosTarg.x, Random.Range(0.0f, 1.0f), ColorPosTarg.z);
                        colorX_1 = ColorX_1;
                        t1 = 0;
                    }
                    if (t2 <= 1)
                    {
                        t2 += Time.deltaTime * ColorSpeed;
                        ColorX_2 = colorX_2 - colorX_2 * t2 + t2 * ColorPosTarg.z;
                    }
                    else
                    {
                        ColorPosTarg = new Vector3(ColorPosTarg.x, ColorPosTarg.y, Random.Range(0.0f, 1.0f));
                        colorX_2 = ColorX_2;
                        t2 = 0;
                    }


                    if (PositionX > 10000)
						PositionX = 0;
					if (PositionZ > 10000)
						PositionZ = 0;
					PositionX += Time.deltaTime * PosIncrease;
					PositionZ += Time.deltaTime * PosIncrease;
				}
				ColorChange.x = Mathf.PerlinNoise(ColorScaleNoise.x*ObjList[N].position.x, ColorScaleNoise.x*ObjList[N].position.z);
				ColorChange.y = Mathf.PerlinNoise(ColorScaleNoise.y*ObjList[N].position.x, ColorScaleNoise.y*ObjList[N].position.z);
				ColorChange.z = Mathf.PerlinNoise(ColorScaleNoise.z*ObjList[N].position.x, ColorScaleNoise.z*ObjList[N].position.z);
				float CurHeight = Mathf.PerlinNoise(ScaleNoise*ObjList[N].position.x + PositionX, ScaleNoise*ObjList[N].position.z + PositionZ);
				ObjList[N].position = new Vector3 (ObjList[N].position.x, CurHeight*HeightMultiplier, ObjList[N].position.z);
				ObjList[N].gameObject.GetComponent<Renderer>().material.color = new Vector4(CurHeight*ColorChange.x+ColorX_0,CurHeight*ColorChange.y+ColorX_1,CurHeight*ColorChange.z+ColorX_2,ColorChange.w);

			}

		}

	}

	void GenerateMap () {

		float CurHeight;

		for (int i = 0; i < GridSide; i++) {

			for (int j = 0; j < GridSide; j++) {

				ObjList.Add( Instantiate(ObjPref,new Vector3(i, 0, j),transform.rotation).transform );

			}
		}

		Generated = true;

	}

}
