using System;
using UnityEngine;

public class Racer
{
    private string _name;
    public string Name { get { return _name; } set { _name = value; } }

    public bool isUserControlled;
    public DateTime finishTime;

    public GameObject Sphere;

    private Color c;
    private float s;
    private float r;

    private static int randomCount;

    public Racer(string name, Color c, float s, float r, bool isUser)
    {
        this._name = name;
        this.c = c;
        this.s = s;
        this.r = r;

        this.isUserControlled = isUser;
    }

    public void ApplyAppearanceToGameObject(GameObject obj)
    {
        Material m = obj.GetComponent<MeshRenderer>().material;
        m.SetFloat("_Glossiness", s);
        m.SetFloat("_Metallic", r);
        m.color = c;

        obj.GetComponent<MeshRenderer>().material = m;
    }

    public static Racer CreateRandomRacer()
    {
        Color c = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        float s = UnityEngine.Random.Range(0.0f, 1.0f);
        float r = UnityEngine.Random.Range(0.0f, 1.0f);
        Racer result = new Racer("Random " + ++randomCount, c, s, r, false);
        return result;
    }
}