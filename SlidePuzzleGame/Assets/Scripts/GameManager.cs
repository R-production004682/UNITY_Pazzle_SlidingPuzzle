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

            //隣接するピースとランダムで入れ替える
            int rnd = Random.Range(0, movablePieces.Count);
            GameObject piece = movablePieces[rnd];
            SwapPiece(piece, pieces[0]);
        }

        buttonRetry.SetActive( false );
    }


    private void Update( )
    {
        //タッチ処理
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPoint, Vector2.zero);

            //レイを飛ばして、動かすべきオブジェクトを取得
            if (hit2d)
            {
                GameObject hitPiece   = hit2d.collider.gameObject;
                GameObject emptyPiece = GetEmptyPiece(hitPiece);
                SwapPiece(hitPiece, emptyPiece);

                //クリア判定
                buttonRetry.SetActive( true );

                for(int i = 0; i < pieces.Count; i++)
                {
                    Vector2 position = pieces[i].transform.position;

                    if(position != startPositions[i])
                    {
                        buttonRetry.SetActive( false );
                    }
                }

                //クリア状態
                if(buttonRetry.activeSelf)
                {
                    Debug.Log("クリア！！");
                }

            }
        }
    }


    /// <summary>
    /// 引数のピースが0番目のピースと隣接していたら0番目のピースを返す
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
    /// 2つのピースの位置を入れ替える
    /// </summary>
    /// <param name="pieceA"></param>
    /// <param name="pieceB"></param>
    private void SwapPiece(GameObject pieceA , GameObject pieceB)
    {
        //どちらかがNullだった場合処理させない
        if (pieceA == null || pieceB == null) { return; }

        // AとBのポジションを入れ替える
         (pieceA.transform.position, pieceB.transform.position)
             = (pieceB.transform.position, pieceA.transform.position);
    }


    /// <summary>
    /// リトライボタン
    /// </summary>
    public void OnClickRetry()
    {
        SceneManager.LoadScene("SlidePuzzleScene");
    }

}
