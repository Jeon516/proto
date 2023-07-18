using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogWait : MonoBehaviour
{
    [SerializeField]
    Image Front;
    [SerializeField]
    Image Side1;
    [SerializeField]
    Image Side2;

    Queue<int> DogOrder = new Queue<int>(); // 강아지의 순서

    public static DogWait Instance { get; private set; } = null;

    private void Start()
    {
        AddData();
        SelectImage();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void AddData()
    {
        DogOrder.Enqueue(Random.Range(0, 7));
        DogOrder.Enqueue(Random.Range(0, 7));
        DogOrder.Enqueue(Random.Range(0, 7));
    } // 첫 시작할 때 기다리는 강아지 배치

    private void SelectImage()
    {
        Front.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ClientDog/Behind/" + DogOrder.Peek().ToString());
        QueueChange();

        Side1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ClientDog/Side/" + DogOrder.Peek().ToString());
        QueueChange();

        Side2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ClientDog/Side/" + DogOrder.Peek().ToString());
        QueueChange();
    } // 강아지 적용

    private void QueueChange()
    {
        int temp= DogOrder.Dequeue();
        DogOrder.Enqueue(temp);
    } // 순서 유지

    public void ChangeImage()
    {
        DogOrder.Dequeue();

        Front.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ClientDog/Behind/" + DogOrder.Peek().ToString());
        QueueChange();

        Side1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ClientDog/Side/" + DogOrder.Peek().ToString());
        QueueChange();

        int last = Random.Range(0, 7);
        DogOrder.Enqueue(last);
        Side2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ClientDog/Side/" + last.ToString());
    } // 클릭할 때 강아지 순서 바꾸기
}
