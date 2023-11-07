using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UploadManager : MonoBehaviour
{
    public GameObject imgPrefab;
    public GameObject content;
    public ImageChangeManager imgMgr;

    List<byte[]> listByteArrays = new List<byte[]>();

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void OnClickFileBrower(bool isFace)
    {

        //필터 설정
        FileBrowser.SetFilters(true, new FileBrowser.Filter("이미지", ".jpg", ".png"));
        // 이 경우 .jpg 확장자를 기본 필터로 설정합니다.
        FileBrowser.SetDefaultFilter(".jpg");

        // 브라우저에 새로운 빠른 링크 추가 (선택 사항) (빠른 링크가 성공적으로 추가되면 true를 반환합니다)
        // 빠른 링크를 한 번만 추가하면 됩니다.
        // 이름: 사용자
        // 경로: C:\Users
        // 아이콘: 기본 (폴더 아이콘)
        FileBrowser.AddQuickLink("사용자", "C:\\Users", null);

        // 코루틴
        StartCoroutine(ShowLoadDialogCoroutine(isFace));
    }

    IEnumerator ShowLoadDialogCoroutine(bool isFace)
    {
        // 불러오기 파일 대화 상자 표시하고 사용자의 응답을 기다립니다.
        // 파일/폴더 불러오기: 모두, 다중 선택 허용: true
        // 초기 경로: 기본값 (문서), 초기 파일 이름: 없음
        // 제목: "파일 불러오기", 제출 버튼 텍스트: "불러오기"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "파일 및 폴더 불러오기", "불러오기");

        // 대화 상자가 닫힘
        // 사용자가 파일/폴더를 선택했는지 또는 작업을 취소했는지 확인 (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {

            // ------------ 불러온 사진 통신해야 함 ---------//

            //안면데이터 저장
            if(!isFace)
            {
                print("사진 안면 등록");
                byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                PhotoManager.instance.OnFaceUpload(bytes);
            }

            //가족사진 등록
            else if(isFace)
            {
                print("사진 가족 등록");
                // 선택한 파일의 경로를 출력 (FileBrowser.Result) (FileBrowser.Success가 false인 경우 null)
                for (int i = 0; i < FileBrowser.Result.Length; i++)
                {
                    Debug.Log(FileBrowser.Result[i]);
                    // FileBrowserHelpers를 사용하여 첫 번째 파일의 바이트를 읽음
                    // File.ReadAllBytes와 달리 Android 10+에서도 작동합니다
                    byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[i]);
                    listByteArrays.Add(bytes);
                }

                PhotoManager.instance.OnPhotoCreate(listByteArrays);
            }

            //2D 이미지 만들기 public static bool LoadImage(this Texture2D tex, byte[] data);
            /*Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(bytes);

            // 만든 이미지 List 에 추가
            imgMgr.spritesList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100));

            // UI 버튼(이미지) Scroll View 에 넣어주기
            int idx = imgMgr.spritesList.Count - 1;
            GameObject go = Instantiate(imgPrefab);
            go.transform.parent = content.transform;
            go.GetComponent<Image>().sprite = imgMgr.spritesList[idx];

            // 버튼 누르면 이미지 바뀌게
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => ToDo(idx));*/

            //---------------------------------------------------//

            // 또는 첫 번째 파일을 persistentDataPath로 복사
            //문자열을 한 경로로 결합
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }

        else
        {
            print("불러올 사진 없음");
        }


        void ToDo(int idx)
        {
            imgMgr.SetImageChange(idx);
        }
    }

}
