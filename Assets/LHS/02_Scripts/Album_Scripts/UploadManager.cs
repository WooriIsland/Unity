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

        //���� ����
        FileBrowser.SetFilters(true, new FileBrowser.Filter("�̹���", ".jpg", ".png"));
        // �� ��� .jpg Ȯ���ڸ� �⺻ ���ͷ� �����մϴ�.
        FileBrowser.SetDefaultFilter(".jpg");

        // �������� ���ο� ���� ��ũ �߰� (���� ����) (���� ��ũ�� ���������� �߰��Ǹ� true�� ��ȯ�մϴ�)
        // ���� ��ũ�� �� ���� �߰��ϸ� �˴ϴ�.
        // �̸�: �����
        // ���: C:\Users
        // ������: �⺻ (���� ������)
        FileBrowser.AddQuickLink("�����", "C:\\Users", null);

        // �ڷ�ƾ
        StartCoroutine(ShowLoadDialogCoroutine(isFace));
    }

    IEnumerator ShowLoadDialogCoroutine(bool isFace)
    {
        // �ҷ����� ���� ��ȭ ���� ǥ���ϰ� ������� ������ ��ٸ��ϴ�.
        // ����/���� �ҷ�����: ���, ���� ���� ���: true
        // �ʱ� ���: �⺻�� (����), �ʱ� ���� �̸�: ����
        // ����: "���� �ҷ�����", ���� ��ư �ؽ�Ʈ: "�ҷ�����"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "���� �� ���� �ҷ�����", "�ҷ�����");

        // ��ȭ ���ڰ� ����
        // ����ڰ� ����/������ �����ߴ��� �Ǵ� �۾��� ����ߴ��� Ȯ�� (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {

            // ------------ �ҷ��� ���� ����ؾ� �� ---------//

            //�ȸ鵥���� ����
            if(!isFace)
            {
                print("���� �ȸ� ���");
                byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                PhotoManager.instance.OnFaceUpload(bytes);
            }

            //�������� ���
            else if(isFace)
            {
                print("���� ���� ���");
                // ������ ������ ��θ� ��� (FileBrowser.Result) (FileBrowser.Success�� false�� ��� null)
                for (int i = 0; i < FileBrowser.Result.Length; i++)
                {
                    Debug.Log(FileBrowser.Result[i]);
                    // FileBrowserHelpers�� ����Ͽ� ù ��° ������ ����Ʈ�� ����
                    // File.ReadAllBytes�� �޸� Android 10+������ �۵��մϴ�
                    byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[i]);
                    listByteArrays.Add(bytes);
                }

                PhotoManager.instance.OnPhotoCreate(listByteArrays);
            }

            //2D �̹��� ����� public static bool LoadImage(this Texture2D tex, byte[] data);
            /*Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(bytes);

            // ���� �̹��� List �� �߰�
            imgMgr.spritesList.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100));

            // UI ��ư(�̹���) Scroll View �� �־��ֱ�
            int idx = imgMgr.spritesList.Count - 1;
            GameObject go = Instantiate(imgPrefab);
            go.transform.parent = content.transform;
            go.GetComponent<Image>().sprite = imgMgr.spritesList[idx];

            // ��ư ������ �̹��� �ٲ��
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => ToDo(idx));*/

            //---------------------------------------------------//

            // �Ǵ� ù ��° ������ persistentDataPath�� ����
            //���ڿ��� �� ��η� ����
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }

        else
        {
            print("�ҷ��� ���� ����");
        }


        void ToDo(int idx)
        {
            imgMgr.SetImageChange(idx);
        }
    }

}
