using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;
public class EditorMapas2 : MonoBehaviour
{
    private Quaternion newQuaternion;
    public Terrain aux;
    public Mesh x2;
    private Vector3[] vertices;

    private int con = 0;
    public float x = 0;
    public float z = 0;
    public int tope = 0;
    public int tope1 = 0;
    public int tope2 = 0;
    public int tope3 = 0;
    public int i = 0;


    void Awake()
    {
        newQuaternion = new Quaternion();
        x2 = new Mesh();
        aux = GetComponent<Terrain>();
        GetComponent<MeshFilter>().mesh = x2; // lo añades pero autogenera la "figura".
        vertices = x2.vertices;
        x2.RecalculateBounds();//calcular el volumen delimitador de la malla a partir de los vértices.
        StartCoroutine(CalulateShape());
    }
    void Start()
    {

        Dibujar();
        Pintar();
        

    }


    void Update()
    {
        UpdateDraw();
    }
    private void UpdateDraw()
    {
        x2.Clear();
        x2.vertices = vertices;
        x2.RecalculateBounds();

    }
    IEnumerator CalulateShape()
    {
        vertices = new Vector3[(int)aux.terrainData.size.x * (int)aux.terrainData.size.x]; // el +2 es porque la clase vector3 pcuneta como 3 posiciones en el array [1]={0,1,2},{3,4,5};
        for (x = 0; x < aux.terrainData.size.x; x++)
        {
            if (x % 10 == 0)
            {
                vertices[i++] = new Vector3(x, 5f, 1.6f);
                con++;
            }
        }
        tope = con;
        for (z = 0; z < aux.terrainData.size.z; z++)
        {
            if (z % 10 == 0)
            {
                vertices[i++] = new Vector3(1.6f, 5f, z);
                con++;
            }
        }
        tope1 = con;
        for (x = aux.terrainData.size.x; x > 0; x--)
        {
            if (x % 10 == 0)
            {
                vertices[i++] = new Vector3(x, 5f, aux.terrainData.size.z);
                con++;
            }
        }
        tope2 = con;
        for (z = aux.terrainData.size.z; z > 0; z--)
        {
            if (z % 10 == 0)
            {
                vertices[i++] = new Vector3(aux.terrainData.size.x, 5f, z);
                con++;
            }
        }
        tope3 = con;
        yield return new WaitForSeconds(.1f);
    }
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {

            return;
        }
        else
        {
            for (i = 0; i < vertices.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(vertices[i], new Vector3(2, 5, 1.5f));
            }
        }
    }
    private void Dibujar()
    {


        // cammbia el tiling de la textura o dara fallos de parpadeo
        var angles = transform.rotation.eulerAngles;
        angles.y = 90;
        newQuaternion = Quaternion.Euler(angles);

        for (i = 0; i < con; i++)
        {

            if (i <= tope)
            {

                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Pared.prefab", typeof(GameObject)), vertices[i], Quaternion.identity);
            }

            if (i <= tope1 && i > tope)
            {

                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Pared.prefab", typeof(GameObject)), vertices[i], newQuaternion);

            }
            if (i <= tope2 && i > tope1 && i > tope)
            {
                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Pared.prefab", typeof(GameObject)), vertices[i], Quaternion.identity);
            }
            if (i <= tope3 && i > tope2 && i > tope1 && i > tope)
            {

                Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Pared.prefab", typeof(GameObject)), vertices[i], newQuaternion);
            }
        }

    }
    private void Pintar()
    {
        float[,,] MapaTexturas = new float[aux.terrainData.alphamapWidth, aux.terrainData.alphamapHeight, aux.terrainData.alphamapLayers];
        for (int y = 0; y < aux.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < aux.terrainData.alphamapWidth; x++)
            {


                float normX = x * 1.0f / (aux.terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (aux.terrainData.alphamapHeight - 1);


                var angle = aux.terrainData.GetSteepness(normX, normY);

                var frac = angle / 90.0;
                MapaTexturas[x, y, 0] = (float)frac;
                MapaTexturas[x, y, 1] = (float)(1 - frac);
            }
        }
        aux.terrainData.SetAlphamaps(0, 0, MapaTexturas);
    }
    private void CrearMazmorra()
    {
        int x = 0;
        int y = 0;
        int[] arr = { 0, 1, 2 };
        Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Pasillo Variant Variant.prefab", typeof(GameObject)), new Vector3(7f, 5f, 20f), Quaternion.identity);
       switch(GeneracionAleatoriaSala(x,y))
        {
            case 0:

                break;
            case 1:
               
                break;
            case 2:

                break;
        }

       
    }
    private int GeneracionAleatoriaSala(int x,int y)
    {
        System.Random r = new System.Random();
        return r.Next(x, y);
    }
    private int GeneracionAleatoriaDireccion(int x, int y)
    {
        System.Random r = new System.Random();
        return r.Next(x, y);
    }
}


//Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Pasillo Variant Variant.prefab", typeof(GameObject)), vertices[i], Quaternion.identity);
        //Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Habitacion1 Variant.prefab", typeof(GameObject)), vertices[i], Quaternion.identity);
       // Instantiate(AssetDatabase.LoadAssetAtPath("Assets/3Dmap/Habitacion2 Variant.prefab", typeof(GameObject)), vertices[i], Quaternion.identity);
       // while (x<aux.terrainData.size.x && z<aux.terrainData.size.z)
       