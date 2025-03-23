using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{
    public int vertexNumber = 3;
    private int lastVertexNumber = 0;

    public GameObject circle;
    private float circleSize = 1f;
    public float radius = 2.5f;
    public float padding = 0f;

    private bool change = true;

    public int shapeCount = 1;

    public Color lineColor;

    public List<GameObject> originalVertexes = new List<GameObject>();
    private List<Vector2> workingVertexes = new List<Vector2>();
    private List<Vector2> auxiliarVertexes = new List<Vector2>();

    // Update is called once per frame
    void Update()
    {
        if(vertexNumber < 3 || shapeCount < 1) return;

        if( vertexNumber != lastVertexNumber ) change = true;
        lastVertexNumber = vertexNumber;

        if(change){
            GeneratePoints();
            change = false;
        }



        for(int i=0; i<vertexNumber; i++){
            workingVertexes.Add(originalVertexes[i].transform.position);
        }
        for(int i=0; i<shapeCount; i++){
            DrawShape();
            workingVertexes = new List<Vector2>(auxiliarVertexes);
        }
        workingVertexes.Clear();
    }

    void GeneratePoints(){
        foreach(Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        originalVertexes.Clear();

        float number = 3f; // Pseudo-Radius
        circleSize = Mathf.Sqrt(2 * number * radius * (1 - Mathf.Cos(6.2830f / vertexNumber)));
        circleSize *= padding;
        radius = 5 - circleSize / 2 - .5f;
        for( int index = 0; index < vertexNumber; index++ ){
            float alpha = index * (6.2830f / vertexNumber) + 3.1415f / 2;
            Vector2 position = radius * new Vector2(Mathf.Cos(alpha), Mathf.Sin(alpha));
            GameObject newCircle = Instantiate(circle, position, Quaternion.identity);
            newCircle.transform.localScale = circleSize * new Vector3(1f, 1f, 1f);
            newCircle.transform.SetParent(gameObject.transform);

            originalVertexes.Add(newCircle.transform.GetChild(1).gameObject);
        }
    }

    void DrawShape(){
        auxiliarVertexes.Clear();
        for( int i=0; i<vertexNumber - 1; i++ ){
            DrawLine(workingVertexes[i], workingVertexes[i + 1]);
            auxiliarVertexes.Add((workingVertexes[i] + workingVertexes[i + 1]) / 2);
        }
        DrawLine(workingVertexes[vertexNumber - 1], workingVertexes[0]);
        auxiliarVertexes.Add((workingVertexes[vertexNumber - 1] + workingVertexes[0]) / 2);
    }

    public void DrawLine(Vector2 start, Vector2 end)
    {
        GameObject lineObj = new GameObject("TemporaryLine");
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Set line properties
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Ensures it is visible
        // lineRenderer.startColor = lineColor;
        // lineRenderer.endColor = lineColor;
        lineRenderer.sortingOrder = 12;

        // Set explicit color gradient (fix for transparency issue)
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        colorKeys[0] = new GradientColorKey(lineColor, 0f);
        colorKeys[1] = new GradientColorKey(lineColor, 1f);

        alphaKeys[0] = new GradientAlphaKey(1f, 0f); // Fully opaque at start
        alphaKeys[1] = new GradientAlphaKey(1f, 1f); // Fully opaque at end

        gradient.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = gradient; // Apply fixed gradient

        // Destroy the line after 'lineDuration' seconds
        Destroy(lineObj, Time.deltaTime + 0.02f);
    }
}
