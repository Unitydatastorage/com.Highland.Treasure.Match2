using System.Collections.Generic;
using DG.Tweening;
using HTMIV.UI;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HTMIV
{
    public partial class HTMIVGridCalculator : MonoBehaviour
    {
        [SerializeField] private AudioClip HTMIVWinClip;
        [SerializeField] private AudioClip HTMIVLoseClip;
        [SerializeField] private int HTMIVSizeY;
        [SerializeField] private AudioClip HTMIVDestroyClip;
        [SerializeField] private AudioSource HTMIVSFXSource;
        [SerializeField] private HTMIVUIGameplay HTMIVuiUiForGamePlay;
        [SerializeField] private AudioClip HTMIVEndMoveClip;
        [SerializeField] private AudioClip HTMIVBadMoveClip;
        [SerializeField] private List<HTMIVToken> HTMIVTokensPrefab;
        [SerializeField] private TMP_Text HTMIVScoreTxt;
        [SerializeField] private HTMIVtile HTMIVTilesPrefab;
        [SerializeField] private AudioClip HTMIVScoreClip;
        [SerializeField] private int HTMIVSIZEx;

        private bool HTMIVIsGameOver;
        private int HTMIVScore;
        private int HTMIVLevelCount;
        private int _HTMIVGoal;
        private Dictionary<Vector2Int, HTMIVtile> HTMIVTilesInGrid = new Dictionary<Vector2Int, HTMIVtile>();
        private HTMIVtile HTMIVTileCur;
        private float HTMIVTime = 90f;
        private List<HTMIVToken> HTMIVTokensInSession = new List<HTMIVToken>();

        private void HTMIVSpawnUpperTokens()
        {
            HTMIVHelpers.HTMIVIgnoreInput = true;
            var HTMIVHasSpawn = false;
            for (var x = 0; x < HTMIVSIZEx; x++)
            {
                var HTMIVtileFather = HTMIVTilesInGrid[new Vector2Int(x, HTMIVSizeY - 1)];
                if (HTMIVtileFather.ImpImpTokenItem != null)
                    continue;

                HTMIVHasSpawn = true;
                var HTMIVrand = Random.Range(0, HTMIVTokensInSession.Count);
                var HTMIVtile = Instantiate(HTMIVTokensInSession[HTMIVrand], transform);
                var HTMIVindex = HTMIVTokensPrefab.FindIndex(x => x == HTMIVTokensInSession[HTMIVrand]);
                HTMIVtile.HTMIVID = HTMIVindex;
                HTMIVtile.transform.localPosition = HTMIVtileFather.transform.localPosition;
                HTMIVtile.HTMIVPOS = HTMIVtileFather.HTMIVPOSITION;
                HTMIVtileFather.ImpImpTokenItem = HTMIVtile;

                HTMIVtile.HTMIVShake();
            }

            if (!HTMIVHasSpawn)
            {
                HTMIVChecker();
                return;
            }

            HTMIVCalculateMove();
        }
        
        private void Start()
        {
            HTMIVLevelCount = PlayerPrefs.GetInt("HTMIVCurLvl", 0) + 1;
            Random.InitState(HTMIVLevelCount);
            
            _HTMIVGoal = Random.Range(150, 301);
            _HTMIVGoal = Mathf.RoundToInt(_HTMIVGoal / 10f) * 10;
            HTMIVUpdaterText();

            HTMIVInitialField();
            HTMIVSpawnUpperTokens();

            return;

            void HTMIVInitialField()
            {
                var HTMIVpos = transform.position;

                for (var HTMIVX = 0; HTMIVX < HTMIVSIZEx; HTMIVX++)
                {
                    for (var HTMIVY = 0; HTMIVY < HTMIVSizeY; HTMIVY++)
                    {
                        var HTMIVtile = Instantiate(HTMIVTilesPrefab, transform);
                        HTMIVtile.transform.position = new Vector3(HTMIVpos.x + HTMIVX, HTMIVpos.y + HTMIVY, 0);
                        HTMIVtile.HTMIVPOSITION = new Vector2Int(HTMIVX, HTMIVY);
                        HTMIVTilesInGrid.Add(HTMIVtile.HTMIVPOSITION, HTMIVtile);
                        HTMIVtile.HTMIVONTileDown += HTMIVStartMove;
                        HTMIVtile.HTMIVONTileEnter += HTMIVEnter;
                        HTMIVtile.HTMIVONTileUP += HTMIVCancelMove;
                    }
                }

                while (HTMIVTokensInSession.Count < 4)
                {
                    var HTMIVRand = HTMIVTokensPrefab[Random.Range(0, HTMIVTokensPrefab.Count)];
                    if (HTMIVTokensInSession.Contains(HTMIVRand))
                        continue;

                    HTMIVTokensInSession.Add(HTMIVRand);
                }

                return;

                void HTMIVEnter(HTMIVtile HTMIVtiLE)
                {
                    if (HTMIVIsGameOver)
                        return;

                    if (HTMIVHelpers.HTMIVStop) return;
                    var HTMIVCurTile = HTMIVTileCur;
                    if (HTMIVCurTile == null)
                        return;
                    if (HTMIVCurTile == HTMIVtiLE)
                        return;
                    if (HTMIVHelpers.HTMIVIgnoreInput)
                    {
                        HTMIVTileCur = null;
                        return;
                    }

                    var HTMIVDir = ((Vector2)(HTMIVCurTile.HTMIVPOSITION - HTMIVtiLE.HTMIVPOSITION)).normalized;
                    if (Mathf.Abs(HTMIVDir.x) != 1f && Mathf.Abs(HTMIVDir.y) != 1f)
                        return;
                    var HTMIVseq = DOTween.Sequence();
                    (HTMIVCurTile.ImpImpTokenItem.HTMIVPOS, HTMIVtiLE.ImpImpTokenItem.HTMIVPOS) =
                        (HTMIVtiLE.ImpImpTokenItem.HTMIVPOS, HTMIVCurTile.ImpImpTokenItem.HTMIVPOS);
                    (HTMIVCurTile.ImpImpTokenItem, HTMIVtiLE.ImpImpTokenItem) =
                        (HTMIVtiLE.ImpImpTokenItem, HTMIVCurTile.ImpImpTokenItem);
                    var HTMIVCheck = HTMIVChecker();
                    HTMIVseq.Append(HTMIVCurTile.ImpImpTokenItem.HTMIVMovement())
                        .Join(HTMIVtiLE.ImpImpTokenItem.HTMIVMovement());
                    if (HTMIVCheck)
                        HTMIVseq.AppendInterval(0.2f);
                    else
                    {
                        HTMIVseq.AppendInterval(0.1f)
                            .AppendCallback(() =>
                            {
                                HTMIVSFXSource.HTMIVPitching(HTMIVBadMoveClip, true);
                                (HTMIVCurTile.ImpImpTokenItem.HTMIVPOS, HTMIVtiLE.ImpImpTokenItem.HTMIVPOS) =
                                    (HTMIVtiLE.ImpImpTokenItem.HTMIVPOS, HTMIVCurTile.ImpImpTokenItem.HTMIVPOS);
                                (HTMIVCurTile.ImpImpTokenItem, HTMIVtiLE.ImpImpTokenItem) =
                                    (HTMIVtiLE.ImpImpTokenItem, HTMIVCurTile.ImpImpTokenItem);
                                HTMIVCurTile.ImpImpTokenItem.HTMIVMovement();
                                HTMIVtiLE.ImpImpTokenItem.HTMIVMovement();
                            });
                    }

                    HTMIVTileCur = null;
                }

                void HTMIVCancelMove() => HTMIVTileCur = null;
            }
        }
    }
}