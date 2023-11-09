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

    //�ȸ���
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

        PhotoManager.instance.OnFaceUpload(fileData);

        /*string filename = Path.GetFileName(path).Split('.')[0];

        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(fileData);

        img.texture = tex;*/
    }

    //�������
    public void OnClickSelectImages()
    {
        // CanSelectMultipleFilesFromGallery�� ����Ͽ� ���� ���� ������ �����ϴ��� Ȯ��
        if (NativeGallery.CanSelectMultipleFilesFromGallery())
        {
            StartCoroutine(SelectAndProcessImages());
        }

        else
        {
            // ���� ���� ������ �������� �ʴ� ��� ��� �޽��� ǥ��
            Debug.LogWarning("Multiple file selection is not supported on this device.");
        }
    }

    private IEnumerator SelectAndProcessImages()
    {
        //������ ������ ���
        yield return new WaitForEndOfFrame();

        // ����ڿ��� �̹����� �����϶�� �޽����� ǥ��
        NativeGallery.GetImagesFromGallery((paths) =>
        {
            //�ҷ����� (������ �����ϸ� �ҷ�����)
            if (paths != null && paths.Length > 0)
            {
                // ����ڰ� �̹����� ������ ���
                foreach (string path in paths)
                {
                    // �̹��� ��ο� ���� �۾� ����
                    Debug.Log("Selected image path: " + path);

                    byte[] bytes = File.ReadAllBytes(path);
                    listByteArrays.Add(bytes);
                }

                PhotoManager.instance.OnPhotoCreate(listByteArrays);
            }

            else
            {
                // ����ڰ� �ƹ� �̹����� �������� ���� ���
                Debug.Log("No images selected.");
            }
        }, "Select Images", "image/*");
    }
}
