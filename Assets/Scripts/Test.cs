using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static DollarRecognizer;

public class Test : MonoBehaviour
{
    public Drawer drawer;
    private DollarRecognizer recog;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recog = new DollarRecognizer();
        //Debug.Log(ReadVector2ListFromXML("Assets/Scripts/circle.xml").Count);
        recog.SavePattern("Circle", ReadVector2ListFromXML("Assets/Scripts/circle.xml"));
        recog.SavePattern("Triangle", ReadVector2ListFromXML("Assets/Scripts/triangle.xml"));
        recog.SavePattern("Caret", ReadVector2ListFromXML("Assets/Scripts/caret.xml"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            drawer.DrawLine();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Result res = recog.Recognize(drawer.points);
            Debug.Log($"Match: {res.Match} | Score: {res.Score}");
            drawer.ResetPoints();           
        }
    }

    List<Vector2> ReadVector2ListFromXML(string filePath)
    {
        List<Vector2> vectorList = new List<Vector2>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);

        XmlNodeList vectorNodes = xmlDoc.SelectNodes("/Gesture/Point");

        foreach (XmlNode node in vectorNodes)
        {
            float x = float.Parse(node.Attributes["X"].Value);
            float y = float.Parse(node.Attributes["Y"].Value);
            Vector2 vec = new Vector2(x, y);
            vectorList.Add(vec);
        }

        return vectorList;
    }
}
