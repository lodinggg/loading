using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customizing : MonoBehaviour
{
    public GameObject avatar;           //ĳ���� ������Ʈ
    public GameObject mainCam;          //����ī�޶�
    public GameObject customCam;        //Ŀ���� ������ ī�޶�
    public GameObject customPage;       //Ŀ���͸���¡ �޴� ������
    public GameObject editPage;         //���� �޴� ������
    public GameObject editChPage;       //ĳ���� ���� ������
    public GameObject editColorPage;    //ĳ���� ���� ���� ������

    public Text customPageTitleText;    //Ŀ���� ������ Ÿ��Ʋ �ؽ�Ʈ
    public Image displaySetList;        //������ Ŀ�� �̸����� ������

    public Sprite[] headImages;         //�Ӹ� ���� �̸����� �̹���
    public Sprite[] upBdyImages;        //���� �̸����� �̹���
    public Sprite[] lowBdyImages;       //���� �̸����� �̹���
                                        
    //Ŀ��
    public GameObject[] head;           //�Ӹ�
    public GameObject[] upBdy;          //����
    public GameObject[] lowBdy;         //����

    public Transform[] part;            //������ Ŀ�� ��� ��ġ��
    public static int[] default_styles = { 0, 0, 0 };   //Ŀ�� ����Ʈ ��

    Vector3 customCamDefaultPosition = new Vector3();   //Ŀ���������� ī�޶� ����Ʈ ��

    int selectedPart;                   //������ ���� ��
    int selected;                       //���� ������

    //Ŀ�� ���� ���ý� â���
    public void EditPartPage(int _part)
    {
        editPage.SetActive(false);
        editChPage.SetActive(true);
        displaySetList.GetComponent<Image>();                       //�̸�����â
        customCamDefaultPosition = customCam.transform.position;    //Ŀ���Ҹ���¡�� ī�޶� �⺻ ��ġ�� ����
        customCam.transform.position = new Vector3(customCam.transform.position.x + 0.86f, 
            customCam.transform.position.y, customCam.transform.position.z + 2.61f);
        selected = 0;                                               //���� ������ �ʱ�ȭ

        switch (_part)
        {
            case 0:
                displaySetList.sprite = headImages[selected];       //�̸�����â�� �Ӹ����� �̸����� �̹��� ���
                customPageTitleText.text = "HEAD EDIT";             //Ŀ�� ������ Ÿ��Ʋ ���� ����
                selectedPart = 0;                                   //���� ������ ������
                
                break;
            case 1:
                displaySetList.sprite = upBdyImages[selected];
                customPageTitleText.text = "UPPER BODY EDIT";
                selectedPart = 1;
                break;
            case 2:
                displaySetList.sprite = lowBdyImages[selected];
                customPageTitleText.text = "LOWER BODY EDIT";
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

    //���õ� Ŀ�� ���� Edit ������ ���
    public void ExitPartPage()
    {
        editPage.SetActive(true);
        editChPage.SetActive(false);
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
        editChPage.SetActive(false);
        editColorPage.SetActive(false);
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
    public void SetCustomizing()
    {
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
        if (CheckOverlap(selectedPart)) //CheckOverlap�� ���� true�� ���
        {
            //�����ؾ��� ��ġ �� ����
            head[i].transform.position = new Vector3(part[selectedPart].transform.position.x,
                part[selectedPart].transform.position.y - 0.8f, part[selectedPart].transform.position.z - 0.1f);
            //������Ʈ�� ȸ�� �� ����
            head[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
            //prefab�� ������Ʈ �ν��Ͻ��� ����
            Instantiate(head[i]).transform.SetParent(part[selectedPart].transform, true);
            //������ Ŀ�� ��Ÿ�� �� �⺻ ������ ����
            default_styles[selectedPart] = i;
        }
    }

    //��ü
    public void EditUpperBody(int i)
    {
        if(CheckOverlap(selectedPart))
        {
            Vector3 pos = new Vector3(part[selectedPart].transform.position.x - 0.6f, part[selectedPart].transform.position.y + 1f, part[selectedPart].transform.position.z + 1f);
            upBdy[i].transform.position = pos;
            upBdy[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
            Instantiate(upBdy[i]).transform.SetParent(part[selectedPart].transform, true);
            default_styles[selectedPart] = i;
        }
    }

    //��ü
    public void EditLowerBody(int i)
    {
        if(CheckOverlap(selectedPart)) {
            Vector3 pos = new Vector3(part[selectedPart].transform.position.x, part[selectedPart].transform.position.y - 0.2f, part[selectedPart].transform.position.z);
            lowBdy[i].transform.position = pos;
            //lowBdy[0].transform.rotation = Quaternion.Euler(chLowBdy.transform.rotation.eulerAngles);
            Instantiate(lowBdy[i]).transform.SetParent(part[selectedPart].transform, true);
            default_styles[selectedPart] = i;
        }
    }

    //Ŀ�� ���� �ߺ� üũ �Լ�
    bool CheckOverlap(int _part)                                //�ߺ� üũ�� ������ �޾ƿ�
    {
        if (!(part[_part].transform.childCount == 0))           //���� ��ġ�� �̹� ������ �ڽ� ��ü������Ʈ�� ���� ���
        {
            if (selected == default_styles[selectedPart])       //������ Ŀ�� ���� �⺻ ���� ���� ���
            {
                return false;                                   //false ���� �����ϰ� ����
            }

            foreach (Transform child in part[_part].transform)  
            {
                Destroy(child.gameObject);                      //�����Ǿ� �ִ� �ڽ� ��ü�� ����
            }
        }
        return true;
    }
    
    //Color ������ ���
    public void EditColor()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(true);
    }

    //�Ӹ� ����
    public void EditColorHead()
    {
        GameObject chHead = avatar.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        Renderer color = chHead.GetComponent<Renderer>();
        //customCam.transform.position = new Vector3(customCam.transform.position.x, customCam.transform.position.y, customCam.transform.position.z);
        color.material.color = Color.blue;
        Debug.Log("�Ӹ��� ����");
    }

    //��ü ����
    public void EditColorUpperBody()
    {
        GameObject chUpBdy = avatar.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        GameObject chLHand = avatar.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).gameObject;
        GameObject chRHand = avatar.transform.GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(1).gameObject;
        Renderer upBdyColor = chUpBdy.GetComponent<Renderer>();
        Renderer lHandColor = chLHand.GetComponent<Renderer>();
        Renderer rHandColor = chRHand.GetComponent<Renderer>();
        upBdyColor.material.color = Color.red;
        lHandColor.material.color = Color.red;
        rHandColor.material.color = Color.red;
        Debug.Log("��ü�� ����");
    }

    //��ü ����
    public void EditColorLowerBody()
    {
        GameObject chLLeg = avatar.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        GameObject chRLeg = avatar.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).gameObject;
        Renderer lLegColor = chLLeg.GetComponent<Renderer>();
        Renderer RLegColor = chRLeg.GetComponent<Renderer>();
        lLegColor.material.color = Color.black;
        RLegColor.material.color = Color.black;
        Debug.Log("��ü�� ����");
    }

    public void Paint()
    {

    }

    //CUSTOMIZING �������� ���ư���
    public void BackToCusPage()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(false);
        customPage.SetActive(true);
    }

}