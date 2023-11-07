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
        //�̹��� ���� (������ ����)
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo seleted = new FileInfo(file);
            //�뷮 ���� byte
            if (seleted.Length > 50000000)
            {
                return;
            }

            //�ҷ����� (������ �����ϸ� �ҷ�����)
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
       
        /*//�̹��� ���� (������ ����)
        NativeGallery.GetImagesFromGallery((files) =>
        {
            //���� ����
            FileBrowser.SetFilters(true, new FileBrowser.Filter("�̹���", ".jpg", ".png"));
            // �� ��� .jpg Ȯ���ڸ� �⺻ ���ͷ� �����մϴ�.
            FileBrowser.SetDefaultFilter(".jpg");

            *//*if (files == null || files.Length == 0)
            {
                //����ڰ� �ƹ� �̹����� �������� ���� ���
                return;
            }

            foreach(string file in files)
            {
                FileInfo selected = new FileInfo(file);

                //�뷮����
                if(selected.Length > 50000000)
                {
                    continue; //�ش� �̹����� ��ŵ�ϰ� ���� �̹����� ó���մϴ�.
                }

                //���� �����ϸ� �ҷ�����
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
        // CanSelectMultipleFilesFromGallery�� ����Ͽ� ���� ���� ������ �����ϴ��� Ȯ��
        if (NativeGallery.CanSelectMultipleFilesFromGallery())
        {
            // ����ڿ��� �̹����� �����϶�� �޽����� ǥ��
            NativeGallery.GetImagesFromGallery((paths) =>
            {
                if (paths != null && paths.Length > 0)
                {
                    // ����ڰ� �̹����� ������ ���
                    foreach (string path in paths)
                    {
                        // �̹��� ��ο� ���� �۾� ����
                        Debug.Log("Selected image path: " + path);
                        test.text = paths.Length + "Selected image path: " + path;
                    }
                }
                else
                {
                    // ����ڰ� �ƹ� �̹����� �������� ���� ���
                    Debug.Log("No images selected.");
                    test.text = "No images selected.";
                }
            }, "Select Images", "image/*");
        }
        else
        {
            // ���� ���� ������ �������� �ʴ� ��� ��� �޽��� ǥ��
            Debug.LogWarning("Multiple file selection is not supported on this device.");
        }
    }
}
