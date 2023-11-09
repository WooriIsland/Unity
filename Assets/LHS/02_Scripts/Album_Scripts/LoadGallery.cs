using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System;

public class LoadGallery : MonoBehaviour
{
    //public RawImage img;
    //public Text test;

    List<byte[]> listByteArrays = new List<byte[]>();

    //안면등록
    public void OnClickImageLoad()
    {
        //이미지 열기 (갤러리 접근)
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo seleted = new FileInfo(file);
            //용량 제한 byte
            if (seleted.Length > 50000000)
            {
                return;
            }

            //불러오기 (파일이 존재하면 불러오기)
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(LoadImage(file));
            }

        });
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);

        PhotoManager.instance.OnFaceUpload(fileData);

        /*string filename = Path.GetFileName(path).Split('.')[0];

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(fileData);

        img.texture = tex;*/
    }

    //사진등록
    public void OnClickSelectImages()
    {
        // CanSelectMultipleFilesFromGallery를 사용하여 여러 파일 선택을 지원하는지 확인
        if (NativeGallery.CanSelectMultipleFilesFromGallery())
        {
            StartCoroutine(SelectAndProcessImages());
        }

        else
        {
            // 여러 파일 선택을 지원하지 않는 경우 경고 메시지 표시
            Debug.LogWarning("Multiple file selection is not supported on this device.");
        }
    }

    private IEnumerator SelectAndProcessImages()
    {
        //프레임 끝가지 대기
        yield return new WaitForEndOfFrame();

        // 사용자에게 이미지를 선택하라는 메시지를 표시
        NativeGallery.GetImagesFromGallery((paths) =>
        {
            //불러오기 (파일이 존재하면 불러오기)
            if (paths != null && paths.Length > 0)
            {
                // 사용자가 이미지를 선택한 경우
                foreach (string path in paths)
                {
                    // 이미지 경로에 대한 작업 수행
                    Debug.Log("Selected image path: " + path);

                    byte[] bytes = File.ReadAllBytes(path);
                    listByteArrays.Add(bytes);
                }

                PhotoManager.instance.OnPhotoCreate(listByteArrays);
            }

            else
            {
                // 사용자가 아무 이미지도 선택하지 않은 경우
                Debug.Log("No images selected.");
            }
        }, "Select Images", "image/*");
    }
}
