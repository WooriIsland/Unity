using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;


public class LoadGallery : MonoBehaviour
{
    public RawImage img;

    List<byte[]> listByteArrays = new List<byte[]>();

    public Text test;

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
        string filename = Path.GetFileName(path).Split('.')[0];

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(fileData);

        img.texture = tex;
    }

    public void OnClickImagesLoad()
    {
       
        /*//이미지 열기 (갤러리 접근)
        NativeGallery.GetImagesFromGallery((files) =>
        {
            //필터 설정
            FileBrowser.SetFilters(true, new FileBrowser.Filter("이미지", ".jpg", ".png"));
            // 이 경우 .jpg 확장자를 기본 필터로 설정합니다.
            FileBrowser.SetDefaultFilter(".jpg");

            *//*if (files == null || files.Length == 0)
            {
                //사용자가 아무 이미지도 선택하지 않은 경우
                return;
            }

            foreach(string file in files)
            {
                FileInfo selected = new FileInfo(file);

                //용량제한
                if(selected.Length > 50000000)
                {
                    continue; //해당 이미지는 스킵하고 다음 이미지를 처리합니다.
                }

                //파일 존재하면 불러오기
                StartCoroutine(LoadImages(file));
            }*//*
        });*/
    }

    IEnumerator LoadImages(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split('.')[0];

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(fileData);

        img.texture = tex;
    }

    public void OnClickSelectImages()
    {
        // CanSelectMultipleFilesFromGallery를 사용하여 여러 파일 선택을 지원하는지 확인
        if (NativeGallery.CanSelectMultipleFilesFromGallery())
        {
            // 사용자에게 이미지를 선택하라는 메시지를 표시
            NativeGallery.GetImagesFromGallery((paths) =>
            {
                if (paths != null && paths.Length > 0)
                {
                    // 사용자가 이미지를 선택한 경우
                    foreach (string path in paths)
                    {
                        // 이미지 경로에 대한 작업 수행
                        Debug.Log("Selected image path: " + path);
                        test.text = paths.Length + "Selected image path: " + path;
                    }
                }
                else
                {
                    // 사용자가 아무 이미지도 선택하지 않은 경우
                    Debug.Log("No images selected.");
                    test.text = "No images selected.";
                }
            }, "Select Images", "image/*");
        }
        else
        {
            // 여러 파일 선택을 지원하지 않는 경우 경고 메시지 표시
            Debug.LogWarning("Multiple file selection is not supported on this device.");
        }
    }
}
