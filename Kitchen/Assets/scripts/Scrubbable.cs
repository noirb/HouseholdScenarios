using UnityEngine;
using System.Collections;

public class Scrubbable : MonoBehaviour {
    public int maskWidth = 512;
    public int maskHeight = 512;
    public float brushScale = 1.0f;
    Texture2D mask;

	// Use this for initialization
	void Start () {
        var renderer = this.GetComponent<MeshRenderer>();

        // In case multiple materials are defined, find the one supporting masking
        foreach (var mat in renderer.materials)
        {
            if (mat.shader.name.Contains("MaskedSurface")) /// TODO: Maaaybe shouldn't have this name hardcoded...
            {
                // Generate a new texture just for this shape so we don't stomp on anyone else's surface
                mask = new Texture2D(maskWidth, maskHeight, TextureFormat.RGB24, false);
                for (int i = 0; i < mask.width; i++)
                {
                    for (int j = 0; j < mask.height; j++)
                    {
                        mask.SetPixel(i, j, Color.white);
                    }
                }
                mask.Apply();
                mat.mainTexture = mask;
            }
            else if (mat.shader.name.Contains("MaskedTexture"))
            {
                mask = new Texture2D(maskWidth, maskHeight, TextureFormat.RGB24, false);
                for (int i = 0; i < mask.width; i++)
                {
                    for (int j = 0; j < mask.height; j++)
                    {
                        mask.SetPixel(i, j, Color.white);
                    }
                }
                mask.Apply();
                mat.SetTexture("_Mask", mask);
            }
        }
	}
	
    public Texture2D GetMask()
    {
        return mask;
    }
}
