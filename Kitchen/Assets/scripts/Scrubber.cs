using UnityEngine;
using System.Collections;

/// <summary>
/// Adds/removes information to/from another object's texture on contact
/// </summary>
public class Scrubber : MonoBehaviour {
    [Tooltip("Texture to 'paint' onto surfaces")]
    public Texture2D brush;
    
    /// <summary>
    /// "Scrubs" the given object's mask at the given texture coords
    /// </summary>
    /// <param name="scrubbable">Object to scrub</param>
    /// <param name="uv">Texture coordinates (on scrubbable) to apply brush</param>
    public void Scrub(Scrubbable scrubbable, Vector2 uv)
    {
        var tex = scrubbable.GetMask();
        if (tex == null)
            return;

        uv.x *= tex.width;
        uv.y *= tex.height;

        for (int i = 0; i < (int)(brush.width * scrubbable.brushScale); i++)
        {
            for (int j = 0; j < (int)(brush.height * scrubbable.brushScale); j++)
            {
                int texu = (int)uv.x - (int)(brush.width * scrubbable.brushScale) / 2 + i;
                int texv = (int)uv.y - (int)(brush.height * scrubbable.brushScale) / 2 + j;
                int brushu = (int)Mathf.Lerp(0, brush.width, (float)i / (brush.width * scrubbable.brushScale));
                int brushv = (int)Mathf.Lerp(0, brush.height, (float)j / (brush.height * scrubbable.brushScale));
                Color texColor = tex.GetPixel(texu, texv);
                Color brushColor = brush.GetPixel(brushu, brushv);
                // only paint onto tex if our brush will make it darker (so we can blend pixels from a stroke across multiple frames)
                if (brushColor.r < texColor.r)
                {
                    tex.SetPixel(texu, texv, brushColor);
                }
            }
        }

        tex.Apply();
    }
    
    /// <summary>
    /// "Scrubs" the given object's mask at the closest (U,V) point to a given 3D coordinate.
    /// Performs a raycast to find the texture coords closest to the intended target.
    /// </summary>
    /// <param name="scrubbable">Object to scrub</param>
    /// <param name="target">Physical location of intended scrubbing</param>
    public void Scrub(Scrubbable scrubbable, Vector3 target)
    {
        RaycastHit hit;
        if (!Physics.Raycast(new Ray(target, this.transform.position - scrubbable.transform.position), out hit, 5.5f))
        {
            return;
        }

        // we missed completely...
        if (hit.collider.gameObject != scrubbable.gameObject)
            return;

        Scrub(scrubbable, hit.textureCoord);
    }

    /// <summary>
    /// Attempts to scrub the given object at a point in space.
    /// Will silently fail if the given object is not Scrubbable
    /// </summary>
    /// <param name="obj">Object to scrub</param>
    /// <param name="target">Physical location of intended scrubbing (should be near surfae of obj)</param>
    public void Scrub(GameObject obj, Vector3 target)
    {
        var scrubbable = obj.GetComponent<Scrubbable>();
        if (scrubbable != null)
        {
            Scrub(scrubbable, target);
        }
    }
}
