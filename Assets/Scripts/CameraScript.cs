using UnityEditor.Callbacks;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - offset;
    }
  
}
