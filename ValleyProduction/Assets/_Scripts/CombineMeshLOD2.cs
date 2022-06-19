using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

// Copy meshes from children into the parent's Mesh.
// CombineInstance stores the list of meshes.  These are combined
// and assigned to the attached Mesh.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshLOD2 : MonoBehaviour
{
    public Transform parent;      //PlantVizualiser

    [Button(90)]
    void CombineLOD2()
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>(transform.parent.parent.GetComponentsInChildren<MeshFilter>());
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>(transform.parent.parent.GetComponentsInChildren<MeshRenderer>());

        List<MeshFilter> meshFiltersToRemove = new List<MeshFilter>();
        List<MeshRenderer> meshRenderersToRemove = new List<MeshRenderer>();

        for (int i = 0; i < meshFilters.Count; i++)
        {
            if(!meshFilters[i].gameObject.name.Contains("LOD2"))
            {
                meshFiltersToRemove.Add((meshFilters[i]));
            }
        }

        foreach(MeshFilter mf in meshFiltersToRemove)
        {
            meshFilters.Remove(mf);
        }

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            if (!meshRenderers[i].gameObject.name.Contains("LOD2"))
            {
                meshRenderersToRemove.Add((meshRenderers[i]));
            }
        }

        foreach (MeshRenderer mr in meshRenderersToRemove)
        {
            meshRenderers.Remove(mr);
        }

        if (meshRenderers.Count < 2 || meshFilters.Count < 2)
        {
            Debug.Log("No LOD2");
            return;
        }

        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        //Material[] SharedMats = meshRenderers[1].sharedMaterials;
        Material MainMaterial = meshRenderers[1].sharedMaterials[0];
        Material[] SubMaterial = new Material[meshRenderers[1].materials.Length - 1];
        List<CombineInstance> combine2 = new List<CombineInstance>();

        Debug.Log(meshFilters.Count);
        for (int i = 0; i < meshFilters.Count; i++)
        {
            if (meshFilters[i] != null)
            {
                Mesh mShared = meshFilters[i].sharedMesh;

                combine[i].mesh = mShared;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

                /*if (mShared.subMeshCount > 1)
                {
                    // combine submeshes
                    for (int j = 1; j < mShared.subMeshCount; j++)
                    {
                        SubMaterial[j - 1] = meshRenderers[1].sharedMaterials[j];
                        CombineInstance ci = new CombineInstance();

                        ci.mesh = mShared;
                        ci.subMeshIndex = j;
                        ci.transform = meshFilters[i].transform.localToWorldMatrix;

                        combine2.Add(ci);
                    }
                }*/

                meshFilters[i].gameObject.SetActive(false);
            }
        }
        GameObject go = new GameObject(gameObject.name);
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();

        var goFilter = go.GetComponent<MeshFilter>();
        var goRenderer = go.GetComponent<MeshRenderer>();

        go.SetActive(false);
        goFilter.mesh = new Mesh();
        goFilter.mesh.CombineMeshes(combine);
        go.SetActive(true);

        go.transform.parent = parent;
        go.name = "LOD2";

        //goRenderer.materials = Mats;
        goRenderer.material = MainMaterial;

        if (SubMaterial.Length >= 1)
        {
            GameObject go2 = new GameObject(gameObject.name + "Sub");
            go2.AddComponent<MeshFilter>();
            go2.AddComponent<MeshRenderer>();

            var goFilter2 = go2.GetComponent<MeshFilter>();
            var goRenderer2 = go2.GetComponent<MeshRenderer>();

            go2.SetActive(false);
            goFilter2.mesh = new Mesh();
            goFilter2.mesh.CombineMeshes(combine2.ToArray());
            go2.SetActive(true);

            //goRenderer.materials = Mats;
            goRenderer2.material = SubMaterial[0];
        }

        parent.parent.GetComponent<TreeBehavior>().GetAllMeshes();

        Debug.Log(gameObject.name);
        gameObject.SetActive(false);
        //DestroyImmediate(gameObject);
    }
}