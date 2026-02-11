using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private GameObject cardPrefab;     //보드에 게임오브젝트 카드프리팹을 나중에 적용할것.




    void Start()
    {
        InitBoard();
    }

    void InitBoard()
    {
        float spaceY = 1.8f;        //카드y의간격



        int rowCount = 5;       //세로길이
        int colCount = 4;       //가로 길이

        for(int row=0; row<rowCount; row++)     
        {
            for(int col=0; col<colCount; col++)
            {
                float posY = (row - (int)(rowCount / 2)) * spaceY;      // 우리가 원하는 row y값=(세로길이-(정수형 변환)(가로길이/2))*y간격을 하면
                                                                        // 원하는y값이되고,그값울 3차원 백터의y값으로 넣는다.(가로 길이를 2로 나눈 나머지에서 반올림 함)...
                                                                        //형변환안하면 결과가이상해질수있음,int형으로 강제 형변환시 0.5보다 커도 소숫점 자리를 그냥 없애고 정수부분만 가져다가 씀..


                Vector3 pos=new Vector3(0f,posY,0f);          //3차원백터의 포지션=새로운 3차원백터의 0,0,0의좌표값을가짐.
                Instantiate(cardPrefab, pos, Quaternion.identity);         //회전값이 필요없음,새로운오브젝트Instantiate가 만들어짐,
                                                                        //5*4번수행됨
            }
        }
    }
   
}
