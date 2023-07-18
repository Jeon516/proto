using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawn : MonoBehaviour
{
    public GameObject[] spawnableObject;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    private void Spawn()
    {
        int randomIndex = Random.Range(0, spawnableObject.Length); // 랜덤한 인덱스 선택
        GameObject randomObject = spawnableObject[randomIndex]; // 선택된 GameObject

        randomObject.SetActive(true); // GameObject 활성화
    }

}
