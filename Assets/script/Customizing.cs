using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customizing : MonoBehaviour
{
    public GameObject avatar;           //캐릭터 오브젝트
    public GameObject mainCam;          //메인카메라
    public GameObject customCam;        //커스텀 페이지 카메라
    public GameObject customPage;       //커스터마이징 메뉴 페이지
    public GameObject editPage;         //에딧 메뉴 페이지
    public GameObject editChPage;       //캐릭터 에딧 페이지
    public GameObject editColorPage;    //캐릭터 색깔 편집 페이지

    public Text customPageTitleText;    //커스텀 페이지 타이틀 텍스트
    public Image displaySetList;        //선택한 커마 미리보기 페이지

    public Sprite[] headImages;         //머리 부위 미리보기 이미지
    public Sprite[] upBdyImages;        //상의 미리보기 이미지
    public Sprite[] lowBdyImages;       //하의 미리보기 이미지
                                        
    //커마
    public GameObject[] head;           //머리
    public GameObject[] upBdy;          //상의
    public GameObject[] lowBdy;         //하의

    public Transform[] part;            //선택한 커마 출력 위치용
    public static int[] default_styles = { 0, 0, 0 };   //커마 디폴트 값

    Vector3 customCamDefaultPosition = new Vector3();   //커마페이지용 카메라 디폴트 값

    int selectedPart;                   //선택한 부위 값
    int selected;                       //현재 선택지

    //커마 부위 선택시 창출력
    public void EditPartPage(int _part)
    {
        editPage.SetActive(false);
        editChPage.SetActive(true);
        displaySetList.GetComponent<Image>();                       //미리보기창
        customCamDefaultPosition = customCam.transform.position;    //커스텀마이징용 카메라 기본 위치값 저장
        customCam.transform.position = new Vector3(customCam.transform.position.x + 0.86f, 
            customCam.transform.position.y, customCam.transform.position.z + 2.61f);
        selected = 0;                                               //현재 선택지 초기화

        switch (_part)
        {
            case 0:
                displaySetList.sprite = headImages[selected];       //미리보기창에 머리부위 미리보기 이미지 출력
                customPageTitleText.text = "HEAD EDIT";             //커마 페이지 타이틀 내용 수정
                selectedPart = 0;                                   //현재 선택한 부위값
                
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

    //Color 페이지 출력
    public void EditColorPage()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(true);
    }

    //선택된 커마 부위 Edit 페이지 출력
    public void ExitPartPage()
    {
        editPage.SetActive(true);
        editChPage.SetActive(false);
        customCam.transform.position = new Vector3(customCamDefaultPosition.x, 
            customCamDefaultPosition.y, customCamDefaultPosition.z);
        //EDIT 페이지로 돌아갈시 카메라 값 기본 값으로 변경
        customPageTitleText.text = "CUSTOMIZING CHARATER";
        selected = 0;
    }

    //Edit 페이지에서 나가기
    public void ExitEditPage()
    {
        editPage.SetActive(true);
        editChPage.SetActive(false);
        editColorPage.SetActive(false);
    }

    //미리보기 페이지 동작
    public void MoveList(int direction)
    {
        if (direction > 0)  //direction의 값에 따라 selected 증감 결정
        {
            selected++;
        } else
        {
            selected--;
        }
        
       switch(selectedPart)
        {
            case 0:
                //selected 값이 미리보기 이미지 배열 크기보다 크거나 0이하 일 경우
                if ((selected == headImages.Length) || (selected < 0)) 
                {
                    //selected가 0보다 작을시 미리보기 이미지 배열의 크기에서 1을 뺀 값을 대입
                    //selected가 배열의 크기보다 클경우 0을 대입 
                    selected = ((selected < 0) ? headImages.Length - 1 : 0);
                }
                displaySetList.sprite = headImages[selected];   //selected 값으로 이미지 변경
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

    //Edit 페이지에서 Apply를 눌렀을 경우
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

    //머리
    public void EditHead(int i) //선택한 selected 값을 전달
    {
        if (CheckOverlap(selectedPart)) //CheckOverlap의 값이 true일 경우
        {
            //생성해야할 위치 값 대입
            head[i].transform.position = new Vector3(part[selectedPart].transform.position.x,
                part[selectedPart].transform.position.y - 0.8f, part[selectedPart].transform.position.z - 0.1f);
            //오브젝트에 회전 값 대입
            head[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
            //prefab의 오브젝트 인스턴스로 생성
            Instantiate(head[i]).transform.SetParent(part[selectedPart].transform, true);
            //선택한 커마 스타일 값 기본 값으로 변경
            default_styles[selectedPart] = i;
        }
    }

    //상체
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

    //하체
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

    //커마 부위 중복 체크 함수
    bool CheckOverlap(int _part)                                //중복 체크할 부위를 받아옴
    {
        if (!(part[_part].transform.childCount == 0))           //생성 위치에 이미 생성된 자식 객체오브젝트가 있을 경우
        {
            if (selected == default_styles[selectedPart])       //생성할 커마 값이 기본 값과 같을 경우
            {
                return false;                                   //false 값을 전달하고 종료
            }

            foreach (Transform child in part[_part].transform)  
            {
                Destroy(child.gameObject);                      //생성되어 있는 자식 객체들 삭제
            }
        }
        return true;
    }
    
    //Color 페이지 출력
    public void EditColor()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(true);
    }

    //머리 색깔
    public void EditColorHead()
    {
        GameObject chHead = avatar.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        Renderer color = chHead.GetComponent<Renderer>();
        //customCam.transform.position = new Vector3(customCam.transform.position.x, customCam.transform.position.y, customCam.transform.position.z);
        color.material.color = Color.blue;
        Debug.Log("머리색 변경");
    }

    //상체 색깔
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
        Debug.Log("상체색 변경");
    }

    //하체 색깔
    public void EditColorLowerBody()
    {
        GameObject chLLeg = avatar.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        GameObject chRLeg = avatar.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).gameObject;
        Renderer lLegColor = chLLeg.GetComponent<Renderer>();
        Renderer RLegColor = chRLeg.GetComponent<Renderer>();
        lLegColor.material.color = Color.black;
        RLegColor.material.color = Color.black;
        Debug.Log("하체에 착용");
    }

    public void Paint()
    {

    }

    //CUSTOMIZING 페이지로 돌아가기
    public void BackToCusPage()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(false);
        customPage.SetActive(true);
    }

}