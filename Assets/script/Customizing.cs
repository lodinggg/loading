using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Customizing : MonoBehaviour
{
    public GameObject avatar;           //캐릭터 오브젝트
    public GameObject mainCam;          //메인카메라
    public GameObject customCam;        //커스텀 페이지 카메라
    public GameObject customPage;       //커스터마이징 메뉴 페이지
    public GameObject editPage;         //에딧 메뉴 페이지
    public GameObject editPartPage;       //캐릭터 에딧 페이지
    public GameObject editColorPage;    //캐릭터 색깔 편집 페이지

    public Text customPageTitleText;    //커스텀 페이지 타이틀 텍스트
    public Image displaySetList;        //선택한 커마 미리보기 페이지
    public GameObject displayColorPage;

    public Sprite[] headImages;         //머리 부위 미리보기 이미지
    public Sprite[] upBdyImages;        //상의 미리보기 이미지
    public Sprite[] lowBdyImages;       //하의 미리보기 이미지
                                        
    //커마
    public GameObject[] head;           //머리
    public GameObject[] upBdy;          //상의
    public GameObject[] lowBdy;         //하의

    public Transform[] part;            //선택한 커마 출력 위치용
    private int[] defaultStyles = { 0, 0, 0 };  //커마 디폴트 값
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

    static Vector3 customCamDefaultPosition = new();   //커마페이지용 카메라 디폴트 값

    int selectedPart;                   //선택한 부위 값
    int selected;                       //현재 선택지

    string fileName = "default.txt";        //저장된 커마 프리셋 파일명

    private void CamPositionSet()
    {
        customCamDefaultPosition = customCam.transform.position;
    }

    private void Start()
    {
        CamPositionSet();
    }
    //현재 선택한 커마값 전달
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

    //지정한 커마 기본값으로 저장
    public void SetPartDefaultStyle(int _part, int _tmp)
    {
        defaultStyles[_part] = _tmp;
    }

    public void SetPartDefaultColor(int _part, int _tmp)
    {
        defaultColors[_part] = _tmp;
    }

    //커마 프리셋 저장
    public void SavePreset()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            File.WriteAllText(filePath, GetDefaultStyle().ToString());

            Debug.Log("프리셋 저장");
        }
        catch (Exception e)
        {
            Debug.LogError("텍스트 파일 저장 오류: " + e.Message);
        }
    }

    //저장한 커마 프리셋 불러오기
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
                        Debug.Log("내용: " + line);
                    }
                    else
                    {
                        Debug.Log("저장된 스타일" + line);
                        for(int i = 0; i<defaultStyles.Length;i++)
                        {
                            subString = line.Substring(i, 1);
                            int.TryParse(subString, out defaultStyles[i]);
                            Debug.Log(defaultStyles[i]);
                            SetPartDefaultStyle(i, defaultStyles[i]);
                            selectedPart = i;
                            selected = defaultStyles[i];
                            SetPart();
                            Debug.Log("셋팅완료");
                        }
                        
                        for(int i =0; i < defaultColors.Length; i++)
                        {
                            subString = (line.Substring(((i * 2)+3), 2));
                            int.TryParse(subString, out defaultColors[i]);
                            Debug.Log(defaultColors[i]);
                            SetPartDefaultColor(i, defaultColors[i]);
                            selectedPart = i;
                            selected = defaultColors[i];
                            Debug.Log("선택된 부위: " + selectedPart + "선택한 컬러:  " + selected);
                            SetColor(defaultColors[i]);
                            Debug.Log("셋팅완료");
                        }
                    }
                }
                selected = 0;
            }
            else
            {
                Debug.LogError("파일이 존재하지 않습니다.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("텍스트 파일 읽기 오류: " + e.Message);
        }
    }
    //커마 부위 선택시 창출력
    public void EditPartPage(int _part)
    {
        editPage.SetActive(false);
        editPartPage.SetActive(true);
        displaySetList.GetComponent<Image>();                       //미리보기창
        //customCamDefaultPosition = customCam.transform.position;    //커스텀마이징용 카메라 기본 위치값 저장
        customCam.transform.position = new Vector3(customCam.transform.position.x + 0.86f, 
            customCam.transform.position.y, customCam.transform.position.z);
        selected = 0;                                               //현재 선택지 초기화

        switch (_part)
        {
            case 0:
                displaySetList.sprite = headImages[selected];       //미리보기창에 머리부위 미리보기 이미지 출력
                customPageTitleText.text = "HEAD EDIT";             //커마 페이지 타이틀 내용 수정
                customCam.transform.position = new Vector3(customCam.transform.position.x - 0.2f,
            customCam.transform.position.y + 0.65f, customCam.transform.position.z);
                selectedPart = 0;                                   //현재 선택한 부위값
                
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

    //Color 페이지 출력
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

    //선택된 커마 부위 Edit 페이지 나가기
    public void ExitPartPage()
    {
        editPage.SetActive(true);
        editPartPage.SetActive(false);
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
        editPartPage.SetActive(false);
        editColorPage.SetActive(false);
        displayColorPage.SetActive(false);
        customCam.transform.position = new Vector3(customCamDefaultPosition.x,
            customCamDefaultPosition.y, customCamDefaultPosition.z);
        //EDIT 페이지로 돌아갈시 카메라 값 기본 값으로 변경
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
    public void SetPart()
    {
        CheckOverlap(selectedPart);     //선택부위중복확인
        if(selected == 0)   //선택한 값이 0일경우 실행 중단하고 return
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

    //머리
    public void EditHead(int i) //선택한 selected 값을 전달
    {
         //생성해야할 위치 값 대입
        head[i].transform.position = new Vector3(part[selectedPart].transform.position.x,
        part[selectedPart].transform.position.y - 0.8f, part[selectedPart].transform.position.z - 0.1f);
        //오브젝트에 회전 값 대입
        head[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
        //prefab의 오브젝트 인스턴스로 생성
        Instantiate(head[i]).transform.SetParent(part[selectedPart].transform, true);
        //선택한 커마 스타일 값 기본 값으로 변경
        SetPartDefaultStyle(selectedPart, i);
    }

    //상체
    public void EditUpperBody(int i)
    {
        Vector3 pos = new Vector3(part[selectedPart].transform.position.x - 0.6f, part[selectedPart].transform.position.y + 1f, part[selectedPart].transform.position.z + 1f);
        upBdy[i].transform.position = pos;
        upBdy[i].transform.rotation = Quaternion.Euler(part[selectedPart].transform.rotation.eulerAngles);
        Instantiate(upBdy[i]).transform.SetParent(part[selectedPart].transform, true);
        SetPartDefaultStyle(selectedPart, i);
    }

    //하체
    public void EditLowerBody(int i)
    {
         Vector3 pos = new Vector3(part[selectedPart].transform.position.x, part[selectedPart].transform.position.y - 0.2f, part[selectedPart].transform.position.z);
         lowBdy[i].transform.position = pos;
            //lowBdy[0].transform.rotation = Quaternion.Euler(chLowBdy.transform.rotation.eulerAngles);
         Instantiate(lowBdy[i]).transform.SetParent(part[selectedPart].transform, true);
         SetPartDefaultStyle(selectedPart, i);
    }

    //커마 부위 중복 체크 함수
    void CheckOverlap(int _part)                                //중복 체크할 부위를 받아옴
    {
        if (!(part[_part].transform.childCount == 0))           //생성 위치에 이미 생성된 자식 객체오브젝트가 있을 경우
        {
            foreach (Transform child in part[_part].transform)  
            {
                Destroy(child.gameObject);                      //생성되어 있는 자식 객체들 삭제
            }
        }
    }
    
    //Color 페이지 출력
    public void EditColor()
    {
        editPage.SetActive(false);
        editColorPage.SetActive(true);
    }


    Color HexToColor(string hex)
    {
        if (hex.Length != 6)
        {
            Debug.LogError("올바른 6자리 16진수 색상 코드가 필요합니다.");
            return Color.white; // 기본값은 흰색
        }

        // R, G, B 색상 채널 추출 및 정규화
        float r = (float)System.Convert.ToUInt32(hex.Substring(0, 2), 16) / 255.0f;
        float g = (float)System.Convert.ToUInt32(hex.Substring(2, 2), 16) / 255.0f;
        float b = (float)System.Convert.ToUInt32(hex.Substring(4, 2), 16) / 255.0f;

        return new Color(r, g, b);
    }

    //머리 색깔
    public void EditColorHead(int _selected)
    {
        GameObject chHead = avatar.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).gameObject;
        Renderer color = chHead.GetComponent<Renderer>();
        color.material.color = HexToColor(colors[_selected]);
        SetPartDefaultColor(0, _selected);
    }

    //상체 색깔
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

    //하체 색깔
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

    //그리기 패널
    public void Paint()
    {
        Debug.Log("개발중. . . ");
    }

    //CUSTOMIZING 페이지로 돌아가기
    public void BackToCusPage()
    {
        
        editPage.SetActive(false);
        editColorPage.SetActive(false);
        customPage.SetActive(true);
    }

}