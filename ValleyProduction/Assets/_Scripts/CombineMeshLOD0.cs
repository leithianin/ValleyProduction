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
public class CombineMeshLOD0 : MonoBehaviour
{
    public Transform parent;      //PlantVizualiser

    [Button(90)]
    void CombineLOD0()
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>(transform.parent.parent.GetComponentsInChildren<MeshFilter>());
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>(transform.parent.parent.GetComponentsInChildren<MeshRenderer>());

        List<MeshFilter> meshFiltersToRemove = new List<MeshFilter>();
        List<MeshRenderer> meshRenderersToRemove = new List<MeshRenderer>();

        Debug.Log("MeshFilter Count : " + meshFilters.Count);
        Debug.Log("MeshRenderer Count : " + meshRenderers.Count);

        //Debug.Log(type.ToString());

        for (int i = 0; i < meshFilters.Count; i++)
        {
            if(!meshFilters[i].gameObject.name.Contains("LOD0"))
            {
                meshFiltersToRemove.Add((meshFilters[i]));
            }
        }

        Debug.Log("MeshFilterToRemove : " + meshFiltersToRemove.Count);

        foreach (MeshFilter mf in meshFiltersToRemove)
        {
            meshFilters.Remove(mf);
        }

        Debug.Log("MeshFilter Count After Delete : " + meshFilters.Count);

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            if (!meshRenderers[i].gameObject.name.Contains("LOD0"))
            {
                meshRenderersToRemove.Add((meshRenderers[i]));
            }
        }

        Debug.Log("MeshRendererToRemove : " + meshRenderersToRemove.Count);

        foreach (MeshRenderer mr in meshRenderersToRemove)
        {
            meshRenderers.Remove(mr);
        }

        Debug.Log("MeshRenderer Count After Delete : " + meshRenderers.Count);

        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        //Material[] SharedMats = meshRenderers[1].sharedMaterials;
        Material MainMaterial = meshRenderers[1].sharedMaterials[0];
        Material[] SubMaterial = new Material[meshRenderers[1].sharedMaterials.Length - 1];
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
        goFilter.sharedMesh = new Mesh();
        goFilter.sharedMesh.CombineMeshes(combine);
        goFilter.sharedMesh.name = "Testouille";
        go.SetActive(true);

        go.transform.parent = parent;

        //goRenderer.materials = Mats;
        goRenderer.sharedMaterial = MainMaterial;

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

        //parent.parent.GetComponent<TreeBehavior>().GetAllMeshes();


        Debug.Log(gameObject.name);
        gameObject.SetActive(false);
        //DestroyImmediate(gameObject);
    }
}