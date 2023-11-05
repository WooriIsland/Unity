using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    private List<GameObject> models;
    public int selectionIdx = 0;

    private void Start()
    {
        models = new List<GameObject>();
        foreach(Transform t in transform)
        {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        models[selectionIdx].gameObject.SetActive(true);


    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string characterName = hit.collider.name;
                PlayerPrefs.SetString("CharacterName", characterName);
            }
        }
    }

    private void Select(int idx)
    {
        if(selectionIdx == idx)
        {
            return;
        }

        if(selectionIdx < 0 || selectionIdx >= models.Count)
        {
            return;
        }

        models[selectionIdx].SetActive(false);
        selectionIdx = idx;
        models[selectionIdx].SetActive(true);
    }

    private void OnMouseUp()
    {
        print("캐릭터 클릭1");
    }

    private void OnMouseUpAsButton()
    {

    }
}
