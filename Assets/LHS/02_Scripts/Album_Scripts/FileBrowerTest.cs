using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileBrowerTest : MonoBehaviour
{
    // 경고: FileBrowser 대화 상자에서 반환되는 경로는 마지막 '\' 문자를 포함하지 않습니다.
    // 경고: FileBrowser는 한 번에 하나의 대화 상자만 표시할 수 있습니다.

    void Start()
    {

    }

    public void OnClickFileBrower()
    {
        // 필터 설정 (선택 사항)
        // 모든 대화 상자가 동일한 필터를 사용할 경우 대화 상자를 표시하기 전에 필터를 한 번만 설정하면 됩니다. json 파일도 받을 수 있음
        FileBrowser.SetFilters(true, new FileBrowser.Filter("이미지", ".jpg", ".png"), new FileBrowser.Filter("텍스트 파일", ".txt", ".pdf"));

        // 대화 상자가 표시될 때 선택되는 기본 필터 설정 (선택 사항)
        // 기본 필터가 성공적으로 설정되면 true를 반환합니다.
        // 이 경우 .jpg 확장자를 기본 필터로 설정합니다.
        FileBrowser.SetDefaultFilter(".jpg");

        // 제외할 파일 확장자 설정 (선택 사항) (기본적으로 .lnk 및 .tmp 확장자가 제외됩니다)
        // 이 함수를 사용할 때 .lnk 및 .tmp 확장자는 명시적으로 함수의 매개변수로 추가하지 않는 한 더 이상 제외되지 않습니다.
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // 브라우저에 새로운 빠른 링크 추가 (선택 사항) (빠른 링크가 성공적으로 추가되면 true를 반환합니다)
        // 빠른 링크를 한 번만 추가하면 됩니다.
        // 이름: 사용자
        // 경로: C:\Users
        // 아이콘: 기본 (폴더 아이콘)
        FileBrowser.AddQuickLink("사용자", "C:\\Users", null);

        // 저장 파일 대화 상자 표시
        // onSuccess 이벤트: 등록되지 않음 (이로 인해 이 대화 상자는 거의 쓸모 없습니다)
        // onCancel 이벤트: 등록되지 않음
        // 파일/폴더 저장: 파일, 다중 선택 허용: false
        // 초기 경로: "C:\", 초기 파일 이름: "Screenshot.png"
        // 제목: "다른 이름으로 저장", 제출 버튼 텍스트: "저장"
        // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "다른 이름으로 저장", "저장" );

        // 폴더 선택 대화 상자 표시
        // onSuccess 이벤트: 선택한 폴더의 경로를 출력
        // onCancel 이벤트: "취소"를 출력
        // 파일/폴더 불러오기: 폴더, 다중 선택 허용: false
        // 초기 경로: 기본값 (문서), 초기 파일 이름: 없음
        // 제목: "폴더 선택", 제출 버튼 텍스트: "선택"
        // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "선택됨: " + paths[0] ); },
        //						   () => { Debug.Log( "취소됨" ); },
        //						   FileBrowser.PickMode.Folders, false, null, null, "폴더 선택", "선택" );

        // 코루틴 예제
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
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
            // 선택한 파일의 경로를 출력 (FileBrowser.Result) (FileBrowser.Success가 false인 경우 null)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // FileBrowserHelpers를 사용하여 첫 번째 파일의 바이트를 읽음
            // File.ReadAllBytes와 달리 Android 10+에서도 작동합니다
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            string str = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(str);

            // 또는 첫 번째 파일을 persistentDataPath로 복사
            //문자열을 한 경로로 결합
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }

        else
        {
            print("불러올 사진 없음");
        }
    }

}
