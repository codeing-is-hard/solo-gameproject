using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private GameObject cardPrefab;     //보드에 게임오브젝트 카드프리팹을 나중에 적용할것.

    [SerializeField] private Sprite[] cardSprites;      //사진 속 동물들 담아둘 배열


    void Start()
    {
        InitBoard();
    }

    void InitBoard()
    {
        float spaceY = 1.8f;        //카드y의간격

        float spaceX = 1.3f;           //카드 x의 간격

        int rowCount = 5;       //세로길이
        int colCount = 4;       //가로 길이

        int cardIndex = 0;      //카드순서

        for(int row=0; row<rowCount; row++)     
        {
            for(int col=0; col<colCount; col++)
            {
                float posX = (col - (colCount / 2)) * spaceX + (spaceX / 2);
                //우리가 원하는 가로 정렬된 x값=가로길이-(전체 가로 길이/2))*x만큼의 간격+(x만큼의 간격/2)를 더한 값




                float posY = (row - (int)(rowCount / 2)) * spaceY;      // 우리가 원하는 row y값=(세로길이-(정수형 변환)(가로길이/2))*y간격을 하면
                                                                        // 원하는y값이되고,그값울 3차원 백터의y값으로 넣는다.(가로 길이를 2로 나눈 나머지에서 반올림 함)...
                                                                        //형변환안하면 결과가이상해질수있음,int형으로 강제 형변환시 0.5보다 커도 소숫점 자리를 그냥 없애고 정수부분만 가져다가 씀..
                        //가운데를 기준으로 rowCount만큼 카드가 퍼짐.

                Vector3 pos=new Vector3(posX,posY,0f);          //3차원백터의 포지션=새로운 3차원백터의 0,0,0의좌표값을가짐.
                GameObject cardObject=Instantiate(cardPrefab, pos, Quaternion.identity);         //회전값이 필요없음,새로운오브젝트Instantiate가 만들어짐,
                Card card = cardObject.GetComponent<Card>();
                card.SetAnimalSprite(cardSprites[cardIndex++]);       //처음에는 0번이므로 0번째동물사진이 setanimalsprite에들어감.

                //5*4번수행됨
                if (cardIndex >= cardSprites.Length)        //후반부에 사라질 임시 오류 방지 코드
                {
                    return;
                }
            }
        }
    }
   
}
