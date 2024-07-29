using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace HTMIV
{
    public partial class HTMIVGridCalculator
    {
        private void HTMIVCalculateMove()
        {
            HTMIVHelpers.HTMIVIgnoreInput = true;

            var HTMIVHasMove = false;
            var HTMIVeptyTiles = new Dictionary<int, HTMIVInfo>();
            for (var x = 0; x < HTMIVSIZEx; x++)
            {
                for (var y = 0; y < HTMIVSizeY; y++)
                {
                    var HTMIVFtile = HTMIVTilesInGrid[new Vector2Int(x, y)];

                    if (HTMIVFtile.ImpImpTokenItem != null)
                    {
                        if (HTMIVeptyTiles.TryGetValue(x, out var HTMIVgettile))
                        {
                            HTMIVHasMove = true;
                            HTMIVgettile.HTMIVMovableGeTile = HTMIVFtile;
                            break;
                        }

                        continue;
                    }

                    if (!HTMIVeptyTiles.ContainsKey(x))
                        HTMIVeptyTiles.Add(x, new HTMIVInfo() { HTMIVEmpty = HTMIVFtile, HTMIVMovableGeTile = null });
                }
            }

            var HTMIVSeq = DOTween.Sequence();
            HTMIVSeq.AppendInterval(0f);
            foreach (var HTMIVtileData in HTMIVeptyTiles.Values)
            {
                if (HTMIVtileData.HTMIVMovableGeTile == null)
                    continue;

                var HTMIVtile = HTMIVtileData.HTMIVMovableGeTile.ImpImpTokenItem;
                HTMIVtileData.HTMIVMovableGeTile.ImpImpTokenItem = null;
                HTMIVtileData.HTMIVEmpty.ImpImpTokenItem = HTMIVtile;

                HTMIVtile.HTMIVPOS = HTMIVtileData.HTMIVEmpty.HTMIVPOSITION;

                HTMIVSeq.Join(HTMIVtile.HTMIVMovement().OnComplete(() =>
                {
                    HTMIVSFXSource.HTMIVPitching(HTMIVEndMoveClip, true);
                    HTMIVtile.HTMIVShake();
                }));
            }

            if (!HTMIVHasMove)
                HTMIVSeq.AppendInterval(0.0f)
                    .AppendCallback(HTMIVSpawnUpperTokens);
            else
                HTMIVSeq.AppendInterval(0.0f)
                    .AppendCallback(HTMIVCalculateMove);
        }
        
        private bool HTMIVCheckVert(out List<HTMIVToken> HTMIVTilesList)
        {
            HTMIVTilesList = new List<HTMIVToken>();
            for (var x = 0; x < HTMIVSIZEx; x++)
            {
                var HTMIVFCur = -1;
                var HTMIVToDelete = new List<HTMIVToken>();

                for (var y = 0; y < HTMIVSizeY; y++)
                {
                    var HTMIVtile = HTMIVTilesInGrid[new Vector2Int(x, y)].ImpImpTokenItem;
                    if (HTMIVtile == null)
                        continue;

                    if (HTMIVFCur == -1)
                    {
                        HTMIVFCur = HTMIVtile.HTMIVID;
                        HTMIVToDelete.Add(HTMIVtile);
                        continue;
                    }

                    if (HTMIVFCur == HTMIVtile.HTMIVID)
                        HTMIVToDelete.Add(HTMIVtile);
                    else
                    {
                        if (HTMIVToDelete.Count >= 3)
                            HTMIVTilesList.AddRange(HTMIVToDelete);

                        HTMIVToDelete.Clear();
                        HTMIVFCur = HTMIVtile.HTMIVID;
                        HTMIVToDelete.Add(HTMIVtile);
                    }
                }

                if (HTMIVToDelete.Count >= 3)
                    HTMIVTilesList.AddRange(HTMIVToDelete);
            }

            return HTMIVTilesList.Count > 0;
        }

        private void HTMIVStartMove(HTMIVtile dpa)
        {
            if (HTMIVIsGameOver)
                return;
            if (HTMIVHelpers.HTMIVStop) return;
            HTMIVTileCur = dpa;
        }

        private void Update()
        {
            if (HTMIVHelpers.HTMIVStop)
                return;

            if (HTMIVIsGameOver)
                return;

            HTMIVTime -= Time.deltaTime;

            if (HTMIVTime <= 0)
            {
                HTMIVIsGameOver = true;
                HTMIVTime = 0f;
            }
            
            HTMIVUpdaterText();
            
            if (HTMIVHelpers.HTMIVIgnoreInput)
                return;
            
            if (HTMIVScore >= _HTMIVGoal)
            {
                HTMIVuiUiForGamePlay.HTMIVWinGame(HTMIVScore, _HTMIVGoal, HTMIVTime);
                HTMIVSFXSource.HTMIVPitching(HTMIVWinClip);
            }
            else if (HTMIVIsGameOver)
            {
                HTMIVuiUiForGamePlay.HTMIVLoseGame(HTMIVScore, _HTMIVGoal, HTMIVTime);
                HTMIVSFXSource.HTMIVPitching(HTMIVLoseClip);
            }
        }

        private void HTMIVUpdaterText()
        {
            HTMIVScoreTxt.text = $"Level{HTMIVLevelCount}\nScore: {HTMIVScore}/{_HTMIVGoal}\nTime: {HTMIVTime.HTMIVTimeToString()}s";
        }

        private bool HTMIVChecker()
        {
            var HTMIVHor = HTMIVChekHor(out var HTMIVHorTiles);
            var HTMIVVer = HTMIVCheckVert(out var HTMIVVerTiles);
            var HTMIVTiles = new List<HTMIVToken>();
            HTMIVTiles.AddRange(HTMIVHorTiles);
            HTMIVTiles.AddRange(HTMIVVerTiles);
            foreach (var HTMIVTile in HTMIVTiles)
                HTMIVTile.HTMIVDESTROY();

            if (HTMIVHor || HTMIVVer)
            {
                HTMIVSFXSource.HTMIVPitching(HTMIVDestroyClip, true);

                var HTMIVseq = DOTween.Sequence();
                HTMIVseq.AppendInterval(0.35f)
                    .AppendCallback(() =>
                    {
                        HTMIVScore += HTMIVTiles.Distinct().Count();
                        HTMIVUpdaterText();
                        HTMIVScoreTxt.transform.DOShakeScale(0.1f).SetEase(Ease.Linear);
                        HTMIVSFXSource.HTMIVPitching(HTMIVScoreClip, true);
                    })
                    .AppendCallback(HTMIVCalculateMove);
            }
            else
            {
                HTMIVHelpers.HTMIVIgnoreInput = false;

                if (HTMIVScore >= _HTMIVGoal)
                {
                    HTMIVIsGameOver = true;

                    HTMIVuiUiForGamePlay.HTMIVWinGame(HTMIVScore, _HTMIVGoal, HTMIVTime);
                    HTMIVSFXSource.HTMIVPitching(HTMIVWinClip);
                }
                else if (HTMIVIsGameOver)
                {
                    HTMIVuiUiForGamePlay.HTMIVLoseGame(HTMIVScore, _HTMIVGoal, HTMIVTime);
                    HTMIVSFXSource.HTMIVPitching(HTMIVLoseClip);
                }
            }

            return HTMIVHor || HTMIVVer;

            bool HTMIVChekHor(out List<HTMIVToken> HTMIVmapTiles)
            {
                HTMIVmapTiles = new List<HTMIVToken>();

                for (var y = 0; y < HTMIVSizeY; y++)
                {
                    var HTMIVCur = -1;
                    var HTMIVToDelete = new List<HTMIVToken>();
                    for (var x = 0; x < HTMIVSIZEx; x++)
                    {
                        var HTMIVtile = HTMIVTilesInGrid[new Vector2Int(x, y)].ImpImpTokenItem;
                        if (HTMIVtile == null)
                            continue;

                        if (HTMIVCur == -1)
                        {
                            HTMIVCur = HTMIVtile.HTMIVID;
                            HTMIVToDelete.Add(HTMIVtile);
                            continue;
                        }

                        if (HTMIVCur == HTMIVtile.HTMIVID)
                            HTMIVToDelete.Add(HTMIVtile);
                        else
                        {
                            if (HTMIVToDelete.Count >= 3)
                                HTMIVmapTiles.AddRange(HTMIVToDelete);

                            HTMIVToDelete.Clear();
                            HTMIVCur = HTMIVtile.HTMIVID;
                            HTMIVToDelete.Add(HTMIVtile);
                        }
                    }

                    if (HTMIVToDelete.Count >= 3)
                        HTMIVmapTiles.AddRange(HTMIVToDelete);
                }

                return HTMIVmapTiles.Count > 0;
            }
        }
    }
}