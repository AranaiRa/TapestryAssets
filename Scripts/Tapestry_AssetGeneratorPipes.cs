using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapestry_AssetGeneratorPipes : MonoBehaviour {

    public Tapestry_AssetGeneratorPipes_Segment
        prefabShort,
        prefabMedium,
        prefabLong,
        prefabBend15,
        prefabBend30,
        prefabBend45,
        prefabBend60,
        prefabBend75,
        prefabBend90;
    public List<Tapestry_AssetGeneratorPipes_Segment> segments;

	// Use this for initialization
	void Start () {
        segments = new List<Tapestry_AssetGeneratorPipes_Segment>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddSegment(Tapestry_AssetGeneratorPipes_Segment prefab)
    {
        Tapestry_AssetGeneratorPipes_Segment seg;
        if (GetCurrentSegment() == null)
            seg = Instantiate(prefab, this.transform);
        else
            seg = Instantiate(prefab, GetCurrentSegment().nextPartFixture);
        seg.name = prefab.name;
        segments.Add(seg);
    }

    public void RemoveSegment()
    {
        DestroyImmediate(segments[segments.Count - 1].gameObject);
        segments.RemoveAt(segments.Count - 1);
    }

    public Tapestry_AssetGeneratorPipes_Segment GetCurrentSegment()
    {
        if (segments.Count > 0)
            return segments[segments.Count - 1];
        else
            return null;
    }

    public void BakeSystem()
    {
        for (int i = segments.Count - 1; i >= 0; i--)
        {
            segments[i].transform.SetParent(this.transform);
            DestroyImmediate(segments[i].nextPartFixture.gameObject);
            DestroyImmediate(segments[i]);
        }
        DestroyImmediate(this);
    }
}
