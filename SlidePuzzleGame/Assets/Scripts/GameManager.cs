using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> pieces;
    [SerializeField] GameObject  buttonRetry;
    [SerializeField] int shuffleCount;

    List<Vector2> startPositions;

    private void Start( )
    {
        startPositions = new List<Vector2>();
        
        foreach(var item in pieces)
        {
            startPositions.Add(item.transform.position);
        }


        for(int i = 0; i < shuffleCount; i++)
        {
            List<GameObject> movablePieces = new List<GameObject>();
            foreach (var item in pieces)
            {
                if (GetEmptyPiece(item) != null)
                {
                    movablePieces.Add(item);
                }
            }

            //�אڂ���s�[�X�ƃ����_���œ���ւ���
            int rnd = Random.Range(0, movablePieces.Count);
            GameObject piece = movablePieces[rnd];
            SwapPiece(piece, pieces[0]);
        }

        buttonRetry.SetActive( false );
    }


    private void Update( )
    {
        //�^�b�`����
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPoint, Vector2.zero);

            //���C���΂��āA�������ׂ��I�u�W�F�N�g���擾
            if (hit2d)
            {
                GameObject hitPiece   = hit2d.collider.gameObject;
                GameObject emptyPiece = GetEmptyPiece(hitPiece);
                SwapPiece(hitPiece, emptyPiece);

                //�N���A����
                buttonRetry.SetActive( true );

                for(int i = 0; i < pieces.Count; i++)
                {
                    Vector2 position = pieces[i].transform.position;

                    if(position != startPositions[i])
                    {
                        buttonRetry.SetActive( false );
                    }
                }

                //�N���A���
                if(buttonRetry.activeSelf)
                {
                    Debug.Log("�N���A�I�I");
                }

            }
        }
    }


    /// <summary>
    /// �����̃s�[�X��0�Ԗڂ̃s�[�X�Ɨאڂ��Ă�����0�Ԗڂ̃s�[�X��Ԃ�
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    GameObject GetEmptyPiece(GameObject piece)
    {
        float direction = Vector2.Distance(piece.transform.position , pieces[0].transform.position);

        if (direction == 1) { return pieces[0]; }

        return null;
    }


    /// <summary>
    /// 2�̃s�[�X�̈ʒu�����ւ���
    /// </summary>
    /// <param name="pieceA"></param>
    /// <param name="pieceB"></param>
    private void SwapPiece(GameObject pieceA , GameObject pieceB)
    {
        //�ǂ��炩��Null�������ꍇ���������Ȃ�
        if (pieceA == null || pieceB == null) { return; }

        // A��B�̃|�W�V���������ւ���
         (pieceA.transform.position, pieceB.transform.position)
             = (pieceB.transform.position, pieceA.transform.position);
    }


    /// <summary>
    /// ���g���C�{�^��
    /// </summary>
    public void OnClickRetry()
    {
        SceneManager.LoadScene("SlidePuzzleScene");
    }

}
