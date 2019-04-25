using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HeartControl : MonoBehaviour
{
    public Image heart;
    public Text ShowText;
    public List<Image> PlantList;
    public Transform concent;
    private float value = 0;
    private int[] num = {1, 3, 5};
    private List<int> plantSeqence = new List<int>();

    // Use this for initialization
	void Start ()
    {
        if (!PlayerPrefs.HasKey("Heart"))
        {
            PlayerPrefs.SetFloat("Heart",value);
        }
        else
        {
            value = PlayerPrefs.GetFloat("Heart");
        }
        char[] sequence = ReadXml()[0].ToCharArray();
        foreach (char c in sequence)
        {
            Debug.Log(c);
            Init(int.Parse(c.ToString()));
        }

        for (int i = 0; i < num.Length; i++)
        {
            num[i] = int.Parse(ReadXml()[i + 1]);
        }
    }


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            try
            {
                if (EventSystem.current.currentSelectedGameObject.tag == "plant")
                {
                    return;
                }
            }
            catch
            {
                heart.color = Color.white;
                heart.gameObject.SetActive(true);
                heart.transform.position = Input.mousePosition;
                heart.transform.DOMoveY(Input.mousePosition.y + 40, 0.4f);
                heart.DOFade(0, 0.4f);
                value++;
            }
        }
        ShowText.text = value.ToString();
        for (int i = 0; i < PlantList.Count; i++)
        {
            PlantList[i].transform.GetChild(0).GetComponent<Text>().text = num[i].ToString();
            if (value < num[i])
            {
                PlantList[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                PlantList[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    public void PlantClick(int index)
    {
        if (value >= num[index])
        {
            Image img = Image.Instantiate(PlantList[index]);
            img.transform.SetParent(concent);
            img.transform.GetChild(0).gameObject.SetActive(false);
            value -= num[index];
            num[index] = num[index] * 2;
            plantSeqence.Add(index);
        }
    }

    void Init(int index)
    {
        Image img = Image.Instantiate(PlantList[index]);
        img.transform.SetParent(concent);
        img.transform.GetChild(0).gameObject.SetActive(false);
        plantSeqence.Add(index);
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(10,10,100,50),"Label");
        //GUI.Box(new Rect(20,10,100,50),"Box" );
        //GUI.Button(new Rect(30, 10, 100, 50), "Button");
    }

    void Mark()
    {
        this.GetComponent<Image>().DOFillAmount(1, 2.5f);
    }

    void OnDestroy()
    {
        PlayerPrefs.SetFloat("Heart", value);
        WriteXml();
        Debug.Log("OnDestroy");
    }

    string[] ReadXml()
    {
        Debug.Log(Application.persistentDataPath + "/123.xml");
        XDocument document = XDocument.Load(Application.persistentDataPath + "/123.xml");
        XElement root = document.Root;
        XElement ele1 = root.Element("plant");
        XElement ele2 = root.Element("num1");
        XElement ele3 = root.Element("num2");
        XElement ele4 = root.Element("num3");
        XElement sequence = ele1.Element("sequence");
        XElement numElement1 = ele2.Element("num1");
        XElement numElement2 = ele3.Element("num2");
        XElement numElement3 = ele4.Element("num3");
        string[]  str = new string[4];
        str[0] = sequence.Value;
        str[1] = numElement1.Value;
        str[2] = numElement2.Value;
        str[3] = numElement3.Value;
        return str;
    }

    void WriteXml()
    {
        XDocument document = new XDocument();
        XElement root = new XElement("package");
        XElement plant = new XElement("plant");
        XElement number1 = new XElement("num1");
        XElement number2 = new XElement("num2");
        XElement number3 = new XElement("num3");
        plant.SetElementValue("sequence", plantSeqence);
        number1.SetElementValue("num1",num[0]);
        number2.SetElementValue("num2",num[1]);
        number3.SetElementValue("num3",num[2]);
        root.Add(plant);
        root.Add(number1);
        root.Add(number2);
        root.Add(number3);
        root.Save(Application.persistentDataPath+"/123.xml");
    }
}
