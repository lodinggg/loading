using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Customizing : MonoBehaviour
{
    public GameObject avatar;           //ĳ���� ������Ʈ
    public GameObject mainCam;          //����ī�޶�
    public GameObject customCam;        //Ŀ���� ������ ī�޶�
    public GameObject customPage;       //Ŀ���͸���¡ �޴� ������
    public GameObject editPage;         //���� �޴� ������
    public GameObject editPartPage;       //ĳ���� ���� ������
    public GameObject editColorPage;    //ĳ���� ���� ���� ������

    public Text customPageTitleText;    //Ŀ���� ������ Ÿ��Ʋ �ؽ�Ʈ
    public Image displaySetList;        //������ Ŀ�� �̸����� ������
    public GameObject displayColorPage;

    public Sprite[] headImages;         //�Ӹ� ���� �̸����� �̹���
    public Sprite[] upBdyImages;        //���� �̸����� �̹���
    public Sprite[] lowBdyImages;       //���� �̸����� �̹���
                                        
    //Ŀ��
    public GameObject[] head;           //�Ӹ�
    public GameObject[] upBdy;          //����
    public GameObject[] lowBdy;         //����

    public Transform[] part;            //������ Ŀ�� ��� ��ġ��
    private int[] defaultStyles = { 0, 0, 0 };  //Ŀ�� ����Ʈ ��
    private string[] colors = { "ffffff", "DCDCDC", "D3D3D3", "808080", "000000",
                                                    "FFEBEE", "E57373", "EF5350", "D32F2F", "871C1C",
                                                     "FFCCBC", "FF8A65", "FF5722", "D84315", "BF360C",
                                                     "FFF8E1", "FFD54F", "FFB300", "FF8F00", "FF6F00",
                                                     "FFFDE7", "FFF59D", "FFEB3B", "FDD835", "DBAD3B",
                                                     "F0F4C3", "D4E157", "C0CA33", "9E9D24", "927717",
                                                     "DCEDC8", "AED581", "7CB342", "689F38", "33691E",
                                                     "E0F7FA", "4DD0E1", "00ACC1", "00838F", "006064",
                                                     "E3F2FD", "90CAF9", "42A5F5", "1565C0", "0D47A1",
                                                     "E8EAF6", "7986CB", "3F51B5", "303F9F", "1A237E",
                                                     "EDE7F6", "B39DD8", "6731B7", "512DA8", "311B92",
                                                     "F3E5F5", "EA80FC", "E040FB", "D500F9", "BE24AA",
                                                     "FCE4EC", "F8BBD0", "F06292", "C2185B", "880E4F"};
    private int[] defaultColors = { 0, 0, 0 };

    static Vector3 customCamDefaultPosition = new();   //Ŀ���������� ī�޶� ����Ʈ ��

    int selectedPart;                   //������ ���� ��
    int selected;                       //���� ������

    string fileName = "default.txt";        //����� Ŀ�� ������ ���ϸ�

    private void CamPositionSet()
    {
        customCamDefaultPosition = customCam.transform.position;
    }

    private void Start()
    {
        CamPositionSet();
    }
    //���� ������ Ŀ���� ����
    public string GetDefaultStyle()
    {
        string str = null;
        for(int style=0; style<defaultStyles.Length;style++)
        {
            str += defaultStyles[style].ToString();
        }
        for(int style=0; style<defaultColors.Length;style++)
        {
            if((defaultColors[style] / 10) == 0)
            {
                str += "0" + defaultColors[style].ToString();
            } else
            {
                str += defaultColors[style].ToString();
            }
        }
        Debug.Log(str);
        return str;
    }

    //������ Ŀ�� �⺻������ ����
    public void SetPartDefaultStyle(int _part, int _tmp)
    {
        defaultStyles[_part] = _tmp;
    }

    public void SetPartDefaultColor(int _part, int _tmp)
    {
        defaultColors[_part] = _tmp;
    }

    //Ŀ�� ������ ����
    public void SavePreset()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            File.WriteAllText(filePath, GetDefaultStyle().ToString());

            Debug.Log("������ ����");
        }
        catch (Exception e)
        {
            Debug.LogError("�ؽ�Ʈ ���� ���� ����: " + e.Message);
        }
    }

    //������ Ŀ�� ������ �ҷ�����
    public void LoadPreset()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                string subString;
                foreach (string line in lines)
                {
                    if (line.Contains("555"))
                    {
                        Debug.Log("����: " + line);
                    }
                    else
                    {
                        Debug.Log("����� ��Ÿ��" + line);
                        for(int i = 0; i<defaultStyles.Length;i++)
                        {
                            subString = line.Substring(i, 1);
                            int.TryParse(subString, out defaultStyles[i]);
                            Debug.Log(defaultStyles[i]);
                            SetPartDefaultStyle(i, defaultStyles[i]);
                            selectedPart = i;
                            selected = defaultStyles[i];
                            SetPart();
                            Debug.Log("���ÿϷ�");
                        }
                        
                        for(int i =0; i < defaultColors.Length; i++)
                        {
                            subString = (line.Substring(((i * 2)+3), 2));
                            int.TryParse(subString, out defaultColors[i]);
                            Debug.Log(defaultColors[i]);
                            SetPartDefaultColor(i, defaultColors[i]);
                            selectedPart = i;
                            selected = defaultColors[i];
                            Debug.Log("���õ� ����: " + selectedPart + "������ �÷�:  " + selected);
                            SetColor(defaultColors[i]);
                            Debug.Log("���ÿϷ�");
                        }
                    }
                }
                selected = 0;
            }
            else
            {
                Debug.LogError("������ �������� �ʽ��ϴ�.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("�ؽ�Ʈ ���� �б� ����: " + e.Message);
        }
    }
    //Ŀ�� ���� ���ý� â���
    public void EditPartPage(int _part)
    {
        editPage.SetActive(false);
        editPartPage.SetActive(true);
        displaySetList.GetComponent<Image>();                       //�̸�����â
        //customCamDefaultPosition = customCam.transform.position;    //Ŀ���Ҹ���¡�� ī�޶� �⺻ ��ġ�� ����
        customCam.transform.position = new Vector3(customCam.transform.position.x + 0.86f, 
            customCam.transform.position.y, customCam.transform.position.z);
        selected = 0;                                               //���� ������ �ʱ�ȭ

        switch (_part)
        {
            case 0:
                displaySetList.sprite = headImages[selected];       //�̸�����â�� �Ӹ����� �̸����� �̹��� ���
                customPageTitleText.text = "HEAD EDIT";             //Ŀ�� ������ Ÿ��Ʋ ���� ����
                customCam.transform.position = new Vector3(customCam.transform.position.x - 0.2f,
            customCam.transform.position.y + 0.65f, customCam.transform.position.z);
                selectedPart = 0;                                   //���� ������ ������
                
                break;
            case 1:
                displaySetList.sprite = upBdyImages[selected];
                customPageTitleText.text = "UPPER BODY EDIT";
                customCam.transform.position = new Vector3(customCam.transform.position.x - 0.2f,
            customCam.transform.position.y - 0.8f , customCam.transform.position.z);
                selectedPart = 1;
                break;
            case 2:
                displaySetList.sprite = lowBdyImages[selected];
                customPageTitleText.text = "LOWER BODY EDIT";
                customCam.transform.position = new Vector3(customCam.transform.position.x - 0.2f,
            customCam.transform.position.y - 2f, customCam.transform.position.z);
                selectedPart = 2;
                break;
        }
        
    }

    //Color ������ ���
    public void EditColorPage()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(true);
    }

    public void EditPartColorPage(int _part)
    {
        if (!displayColorPage.activeSelf || !(selectedPart == _part))
        {
            displayColorPage.SetActive(true);
            customCam.transform.position = new Vector3(customCamDefaultPosition.x,
            customCamDefaultPosition.y, customCamDefaultPosition.z);
            switch (_part)
            {
                case 0:
                    selectedPart = 0;
                    customCam.transform.position = new Vector3(customCam.transform.position.x +0.55f,
            customCam.transform.position.y + 0.65f, customCam.transform.position.z - 0.67f) ;
                    break;
                case 1:
                    selectedPart = 1;
                    customCam.transform.position = new Vector3(customCam.transform.position.x + 0.55f,
            customCam.transform.position.y - 1f, customCam.transform.position.z - 0.67f);
                    break;
                case 2:
                    selectedPart = 2;
                    customCam.transform.position = new Vector3(customCam.transform.position.x + 0.55f,
            customCam.transform.position.y -1.5f, customCam.transform.position.z - 0.67f);
                    break;
            }
        } else
        {
            displayColorPage.SetActive(false);
            customCam.transform.position = new Vector3(customCamDefaultPosition.x,
             customCamDefaultPosition.y, customCamDefaultPosition.z);
        }
    }

    public void SetColor(int _selected)
    {
        switch (selectedPart)
        {
            case 0:
                {
                    EditColorHead(_selected);
                    break;
                }
            case 1:
                {
                   
                    EditColorUpperBody(_selected);
                    break;
                }
            case 2:
                {
                    
                    EditColorLowerBody(_selected);
                    break;
                }
        }
    }

    //���õ� Ŀ�� ���� Edit ������ ������
    public void ExitPartPage()
    {
        editPage.SetActive(true);
        editPartPage.SetActive(false);
        customCam.transform.position = new Vector3(customCamDefaultPosition.x, 
            customCamDefaultPosition.y, customCamDefaultPosition.z);
        //EDIT �������� ���ư��� ī�޶� �� �⺻ ������ ����
        customPageTitleText.text = "CUSTOMIZING CHARATER";
        selected = 0;
    }

    //Edit ���������� ������
    public void ExitEditPage()
    {
        editPage.SetActive(true);
        editPartPage.SetActive(false);
        editColorPage.SetActive(false);
        displayColorPage.SetActive(false);
        customCam.transform.position = new Vector3(customCamDefaultPosition.x,
            customCamDefaultPosition.y, customCamDefaultPosition.z);
        //EDIT �������� ���ư��� ī�޶� �� �⺻ ������ ����
    }

    //�̸����� ������ ����
    public void MoveList(int direction)
    {
        if (direction > 0)  //direction�� ���� ���� selected ���� ����
        {
            selected++;
        } else
        {
            selected--;
        }
        
       switch(selectedPart)
        {
            case 0:
                //selected ���� �̸����� �̹��� �迭 ũ�⺸�� ũ�ų� 0���� �� ���
                if ((selected == headImages.Length) || (selected < 0)) 
                {
                    //selected�� 0���� ������ �̸����� �̹��� �迭�� ũ�⿡�� 1�� �� ���� ����
                    //selected�� �迭�� ũ�⺸�� Ŭ��� 0�� ���� 
                    selected = ((selected < 0) ? headImages.Length - 1 : 0);
                }
                displaySetList.sprite = headImages[selected];   //selected ������ �̹��� ����
                break;
            case 1:
                if ((selected == upBdyImages.Length) || (selected < 0))
                {
                    selected = ((selected < 0) ? upBdyImages.Length - 1 : 0);
                }
                displaySetList.sprite = upBdyImages[selected];
                break;
            case 2:
                if ((selected == lowBdyImages.Length) || (selected < 0))
                {
                    selected = ((selected < 0) ? lowBdyImages.Length - 1 : 0);
                }
                displaySetList.sprite = lowBdyImages[selected];
                break; 

        }
    }

    //Edit ���������� Apply�� ������ ���
    public void SetPart()
    {
        CheckOverlap(selectedPart);     //���ú����ߺ�Ȯ��
        if(selected == 0)   //������ ���� 0�ϰ�� ���� �ߴ��ϰ� return
        {
            defaultStyles[selectedPart] = 0;
            return;
        }
        switch(selectedPart)
        {
            case 0:
                {
                    EditHead(selected);
                    break;
                }
            case 1:
                {
                    EditUpperBody(selected);
                    break;
                }
            case 2:
                {
                    EditLowerBody(selected);
                    break;
                }
        }
    }

    //�Ӹ�
    public void EditHead(int i) //������ selected ���� ����
    {
         //�����ؾ��� ��ġ �� ����
        head[i].transform.position = new Vector3(part[selectedPart].transform.position.x,
        part[selectedPart].transform.position.y - 0.8f, part[selectedPart].transform.position.z - 0.1f);
        //������Ʈ�� ȸ�� �� ����
        head[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
        //prefab�� ������Ʈ �ν��Ͻ��� ����
        Instantiate(head[i]).transform.SetParent(part[selectedPart].transform, true);
        //������ Ŀ�� ��Ÿ�� �� �⺻ ������ ����
        SetPartDefaultStyle(selectedPart, i);
    }

    //��ü
    public void EditUpperBody(int i)
    {
        Vector3 pos = new Vector3(part[selectedPart].transform.position.x - 0.6f, part[selectedPart].transform.position.y + 1f, part[selectedPart].transform.position.z + 1f);
        upBdy[i].transform.position = pos;
        upBdy[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
        Instantiate(upBdy[i]).transform.SetParent(part[selectedPart].transform, true);
        SetPartDefaultStyle(selectedPart, i);
    }

    //��ü
    public void EditLowerBody(int i)
    {
         Vector3 pos = new Vector3(part[selectedPart].transform.position.x, part[selectedPart].transform.position.y - 0.2f, part[selectedPart].transform.position.z);
         lowBdy[i].transform.position = pos;
            //lowBdy[0].transform.rotation = Quaternion.Euler(chLowBdy.transform.rotation.eulerAngles);
         Instantiate(lowBdy[i]).transform.SetParent(part[selectedPart].transform, true);
         SetPartDefaultStyle(selectedPart, i);
    }

    //Ŀ�� ���� �ߺ� üũ �Լ�
    void CheckOverlap(int _part)                                //�ߺ� üũ�� ������ �޾ƿ�
    {
        if (!(part[_part].transform.childCount == 0))           //���� ��ġ�� �̹� ������ �ڽ� ��ü������Ʈ�� ���� ���
        {
            foreach (Transform child in part[_part].transform)  
            {
                Destroy(child.gameObject);                      //�����Ǿ� �ִ� �ڽ� ��ü�� ����
            }
        }
    }
    
    //Color ������ ���
    public void EditColor()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(true);
    }


    Color HexToColor(string hex)
    {
        if (hex.Length != 6)
        {
            Debug.LogError("�ùٸ� 6�ڸ� 16���� ���� �ڵ尡 �ʿ��մϴ�.");
            return Color.white; // �⺻���� ���
        }

        // R, G, B ���� ä�� ���� �� ����ȭ
        float r = (float)System.Convert.ToUInt32(hex.Substring(0, 2), 16) / 255.0f;
        float g = (float)System.Convert.ToUInt32(hex.Substring(2, 2), 16) / 255.0f;
        float b = (float)System.Convert.ToUInt32(hex.Substring(4, 2), 16) / 255.0f;

        return new Color(r, g, b);
    }

    //�Ӹ� ����
    public void EditColorHead(int _selected)
    {
        GameObject chHead = avatar.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        Renderer color = chHead.GetComponent<Renderer>();
        color.material.color = HexToColor(colors[_selected]);
        SetPartDefaultColor(0, _selected);
    }

    //��ü ����
    public void EditColorUpperBody(int _selected)
    {
        GameObject chUpBdy = avatar.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        GameObject chLHand = avatar.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).gameObject;
        GameObject chRHand = avatar.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(1).gameObject;
        Renderer upBdyColor = chUpBdy.GetComponent<Renderer>();
        Renderer lHandColor = chLHand.GetComponent<Renderer>();
        Renderer rHandColor = chRHand.GetComponent<Renderer>();
        upBdyColor.material.color = HexToColor(colors[_selected]);
        lHandColor.material.color = HexToColor(colors[_selected]);
        rHandColor.material.color = HexToColor(colors[_selected]);
        SetPartDefaultColor(1, _selected);
    }

    //��ü ����
    public void EditColorLowerBody(int _selected)
    {
        GameObject chLLeg = avatar.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        GameObject chRLeg = avatar.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).gameObject;
        Renderer lLegColor = chLLeg.GetComponent<Renderer>();
        Renderer RLegColor = chRLeg.GetComponent<Renderer>();
        lLegColor.material.color = HexToColor(colors[_selected]);
        RLegColor.material.color = HexToColor(colors[_selected]);
        SetPartDefaultColor(2, _selected);
    }

    //�׸��� �г�
    public void Paint()
    {
        Debug.Log("������. . . ");
    }

    //CUSTOMIZING �������� ���ư���
    public void BackToCusPage()
    {
        
        editPage.SetActive(false);
        editColorPage.SetActive(false);
        customPage.SetActive(true);
    }

}