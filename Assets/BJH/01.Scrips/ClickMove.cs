using UnityEngine;
using UnityEngine.AI;

public class ClickMove : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform trCam;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        trCam.position = transform.position + new Vector3(0, 1.15f, -3.69f);
    }
}
