using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Drawer drawer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            drawer.DrawLine();
        }
        else if(Input.GetMouseButtonUp(0))
        {
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
