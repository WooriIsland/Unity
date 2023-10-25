using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileBrowerTest : MonoBehaviour
{
    // ���: FileBrowser ��ȭ ���ڿ��� ��ȯ�Ǵ� ��δ� ������ '\' ���ڸ� �������� �ʽ��ϴ�.
    // ���: FileBrowser�� �� ���� �ϳ��� ��ȭ ���ڸ� ǥ���� �� �ֽ��ϴ�.

    void Start()
    {

    }

    public void OnClickFileBrower()
    {
        // ���� ���� (���� ����)
        // ��� ��ȭ ���ڰ� ������ ���͸� ����� ��� ��ȭ ���ڸ� ǥ���ϱ� ���� ���͸� �� ���� �����ϸ� �˴ϴ�. json ���ϵ� ���� �� ����
        FileBrowser.SetFilters(true, new FileBrowser.Filter("�̹���", ".jpg", ".png"), new FileBrowser.Filter("�ؽ�Ʈ ����", ".txt", ".pdf"));

        // ��ȭ ���ڰ� ǥ�õ� �� ���õǴ� �⺻ ���� ���� (���� ����)
        // �⺻ ���Ͱ� ���������� �����Ǹ� true�� ��ȯ�մϴ�.
        // �� ��� .jpg Ȯ���ڸ� �⺻ ���ͷ� �����մϴ�.
        FileBrowser.SetDefaultFilter(".jpg");

        // ������ ���� Ȯ���� ���� (���� ����) (�⺻������ .lnk �� .tmp Ȯ���ڰ� ���ܵ˴ϴ�)
        // �� �Լ��� ����� �� .lnk �� .tmp Ȯ���ڴ� ��������� �Լ��� �Ű������� �߰����� �ʴ� �� �� �̻� ���ܵ��� �ʽ��ϴ�.
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        // �������� ���ο� ���� ��ũ �߰� (���� ����) (���� ��ũ�� ���������� �߰��Ǹ� true�� ��ȯ�մϴ�)
        // ���� ��ũ�� �� ���� �߰��ϸ� �˴ϴ�.
        // �̸�: �����
        // ���: C:\Users
        // ������: �⺻ (���� ������)
        FileBrowser.AddQuickLink("�����", "C:\\Users", null);

        // ���� ���� ��ȭ ���� ǥ��
        // onSuccess �̺�Ʈ: ��ϵ��� ���� (�̷� ���� �� ��ȭ ���ڴ� ���� ���� �����ϴ�)
        // onCancel �̺�Ʈ: ��ϵ��� ����
        // ����/���� ����: ����, ���� ���� ���: false
        // �ʱ� ���: "C:\", �ʱ� ���� �̸�: "Screenshot.png"
        // ����: "�ٸ� �̸����� ����", ���� ��ư �ؽ�Ʈ: "����"
        // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "�ٸ� �̸����� ����", "����" );

        // ���� ���� ��ȭ ���� ǥ��
        // onSuccess �̺�Ʈ: ������ ������ ��θ� ���
        // onCancel �̺�Ʈ: "���"�� ���
        // ����/���� �ҷ�����: ����, ���� ���� ���: false
        // �ʱ� ���: �⺻�� (����), �ʱ� ���� �̸�: ����
        // ����: "���� ����", ���� ��ư �ؽ�Ʈ: "����"
        // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "���õ�: " + paths[0] ); },
        //						   () => { Debug.Log( "��ҵ�" ); },
        //						   FileBrowser.PickMode.Folders, false, null, null, "���� ����", "����" );

        // �ڷ�ƾ ����
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
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
            // ������ ������ ��θ� ��� (FileBrowser.Result) (FileBrowser.Success�� false�� ��� null)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // FileBrowserHelpers�� ����Ͽ� ù ��° ������ ����Ʈ�� ����
            // File.ReadAllBytes�� �޸� Android 10+������ �۵��մϴ�
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            string str = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(str);

            // �Ǵ� ù ��° ������ persistentDataPath�� ����
            //���ڿ��� �� ��η� ����
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }

        else
        {
            print("�ҷ��� ���� ����");
        }
    }

}
