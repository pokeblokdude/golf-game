using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Terrain : MonoBehaviour {

    public bool generateMesh = false;

    void OnValidate() {
        if(generateMesh) {
            GenerateMesh();
            generateMesh = false;
        }
    }

    void GenerateMesh() {
        Vector3 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] terrainPieces = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combine = new CombineInstance[terrainPieces.Length];
        for(int i = 0; i < terrainPieces.Length; i++) {
            if(terrainPieces[i].transform == transform) {
                continue;
            }
            combine[i].subMeshIndex = 0;
            combine[i].mesh = terrainPieces[i].sharedMesh;
            combine[i].transform = terrainPieces[i].transform.localToWorldMatrix;
            terrainPieces[i].gameObject.SetActive(false);
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);

        MeshFilter m = GetComponent<MeshFilter>();
        m.sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.SetActive(true);

        transform.rotation = oldRot;
        transform.position = oldPos;
    }

}
