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
            startPositions.Add(item.transform.position);

        ShufflePieces();
        buttonRetry.SetActive( false );
    }


    private void Update( )
    {
        //�^�b�`����
        if (Input.GetMouseButtonUp(0))
        {
            HandleInput();
            CheckForWin();
        }
    }


    /// <summary>
    /// �s�[�X���V���b�t�����鏈��
    /// </summary>
    private void ShufflePieces()
    {
        for(int i = 0; i < shuffleCount; i++)
        {
            List<GameObject> movablePieces = pieces.FindAll(item => GetEmptyPiece(item) != null);
            int rnd = Random.Range(0 , movablePieces.Count);
            SwapPiece(movablePieces[rnd] , pieces[0]);
        }
    }


    /// <summary>
    /// ���͏���
    /// </summary>
    private void HandleInput()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit2d)
        {
            SwapPiece(hit2d.collider.gameObject, GetEmptyPiece(hit2d.collider.gameObject));
        }
    }


    /// <summary>
    /// ���������̃��b�Z�[�W���o��
    /// </summary>
    private void CheckForWin()
    {
        bool allPiecesInCorrentPosition = true;

        for (int i = 0; i < pieces.Count; i++)
        {
            if ((Vector2)pieces[i].transform.position != startPositions[i])
            {
                allPiecesInCorrentPosition = false;
                break;
            }
        }
        buttonRetry.SetActive(allPiecesInCorrentPosition);

        if (buttonRetry.activeSelf)
            Debug.Log("�N���A");
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
