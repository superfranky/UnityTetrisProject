using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using MyClasses;
using UnityEngine;
using Random = System.Random;
using UnityEngine.InputSystem;

namespace Tetris
{
    public enum GridPoints
    {
        None,
        PivotPoint,
        ShapePoint,
        BottomPoint
    }

    public class TetrisGrid : MonoBehaviour
    {
        
        [SerializeField] private GameObject _blockMesh;
        [SerializeField] private GameObject _blockMeshParent;
        [SerializeField] private GameObject _sceneMeshParent;
        [SerializeField] private Sprite[] _blockSprites;
        [SerializeField] private InputActionReference _actionReference;
        [SerializeField] private StringActionChannelISO soundClipChannel;
        [SerializeField] private IntActionChannelISO scoreBoardChannel;
        [SerializeField] private IntActionChannelISO lineBoardChannel;
        [SerializeField] private IntActionChannelISO levelBoardChannel;
        [SerializeField] private BlockActionChannelISO statBoardChannel;
        [SerializeField] private StringActionChannelISO previewBoardChannel;
        private readonly Vector2Int _left = new Vector2Int(-1, 0);
        private readonly Vector2Int _right = new Vector2Int(1, 0);
        private readonly Vector2Int _down = new Vector2Int(0, 1);
        private readonly GridPoints[,] _pivotGrid = new GridPoints[10, 22];
        private readonly GridPoints[,] _shapeGrid = new GridPoints[10, 22];
        private readonly GridPoints[,] _bottomGrid = new GridPoints[10, 23];
        private readonly List<Block> _allPieces = new List<Block>();
        private Block _playableBlock;
        private float _movementTime = 0.5f;
        private readonly float _movementTimeMultiplier = 6;
        private int _rowsTraveled;
        private bool _travelingRows;
        private int _nextRandomNumber = -1;
        private readonly Random _random = new Random();
        private int _level = 0;
        private int _clearedLines;
        private int _calculatedScore;
        private void Start()
        {
            AssignControls();

            AddPiecesToList();
            FillBottomRow();
            PlayNextPiece();
            StartCoroutine(MoveBlock(_down));
            VisualizeShapePointsIn3D();
        }
        private void AssignControls()
        {
            _actionReference.action.started += MoveDown;
            _actionReference.action.canceled += MoveDown;
        }

        private void MoveDown(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _travelingRows = true;
                _movementTime /= _movementTimeMultiplier;
            }else if (context.canceled)
            {
                _travelingRows = false;
                _movementTime *= _movementTimeMultiplier;
            }
        }

        private void AddPiecesToList()
        {
            _allPieces.Add(new TPiece());
            _allPieces.Add(new BPiece());
            _allPieces.Add(new IPiece());
            _allPieces.Add(new LPieceLeft());
            _allPieces.Add(new LPieceRight());
            _allPieces.Add(new ZPieceLeft());
            _allPieces.Add(new ZPieceRight());
        }
       
        private void PreviewNextPiece()
        {
            var previewBlock = _allPieces[_nextRandomNumber];
            previewBoardChannel.Raise(previewBlock.GetType().Name);
        }

        private void PlayNextPiece()
        {
            _rowsTraveled = 0;

            Array.Clear(_pivotGrid,0, _pivotGrid.Length);
            _pivotGrid[4, 2] = GridPoints.PivotPoint;


            if (_nextRandomNumber == -1)
            {
                var randomNumber = _random.Next(_allPieces.Count);
                _playableBlock = _allPieces[randomNumber];
                _nextRandomNumber = _random.Next(_allPieces.Count);
            }
            else
            {
                _playableBlock = _allPieces[_nextRandomNumber];
                _nextRandomNumber = _random.Next(_allPieces.Count);
            }

            statBoardChannel.Raise(_playableBlock);


            PreviewNextPiece();

            _playableBlock.ShapeIndex = -1;
            var pivotPos = _pivotGrid.VectorOf(GridPoints.PivotPoint);
            var nextShape = _playableBlock.ReturnNextShape();
            foreach (var shape in nextShape)
            {
                _shapeGrid[pivotPos.x + shape.x, pivotPos.y + shape.y] = GridPoints.ShapePoint;
            }
        }
        private void FillBottomRow()
        {

            for (var i = 0; i < _bottomGrid.GetLength(0); i++)
            {
                _bottomGrid[i, _bottomGrid.GetLength(1) - 1] = GridPoints.BottomPoint;
            }
        }


        private void VisualizeShapePointsIn3D()
        {

            var shapePoints = _shapeGrid.AllCoordinatesOf(GridPoints.ShapePoint);
            foreach (var (x, y) in shapePoints)
            {
                var obj = Instantiate(_blockMesh, new Vector3(x, y), Quaternion.identity);
                obj.transform.parent = _blockMeshParent.transform;
                obj.GetComponent<SpriteRenderer>().sprite = _blockSprites[_playableBlock.SpriteIndex];
            }
        }
        //Control events
        private void OnLeft()
        {
            StartCoroutine(MoveBlock(_left));
            soundClipChannel.Raise("movement");
        }

        private void OnRight()
        {
            StartCoroutine(MoveBlock(_right));
            soundClipChannel.Raise("movement");
        }




        private void OnRMB()
        {
            RotateBlock();
            soundClipChannel.Raise("rotation");
        }

        private void RotateBlock()
        {
            var startingPivotPos = _pivotGrid.VectorOf(GridPoints.PivotPoint);
            var startingShapeIndex = _playableBlock.ShapeIndex;
            var nextShape = _playableBlock.ReturnNextShape();
            Array.Clear(_shapeGrid, 0, _shapeGrid.Length);
            try
            {
                foreach (var shapePoint in nextShape)
                {
                    if (_bottomGrid[startingPivotPos.x + shapePoint.x, startingPivotPos.y + shapePoint.y] == GridPoints.BottomPoint)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    _shapeGrid[startingPivotPos.x + shapePoint.x, startingPivotPos.y + shapePoint.y] = GridPoints.ShapePoint;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                _playableBlock.ShapeIndex = startingShapeIndex -= 1;
                var lastShape = _playableBlock.ReturnNextShape();
                Array.Clear(_shapeGrid, 0, _shapeGrid.Length);
                foreach (var point in lastShape)
                {
                    _shapeGrid[startingPivotPos.x + point.x, startingPivotPos.y + point.y] = GridPoints.ShapePoint;
                }
            }
            finally
            {
                RepositionBlockMeshes();
            }

        }

        private IEnumerator MoveBlock(Vector2Int direction)
        {
            if (direction == _down)
            {
               yield return new WaitForSeconds(_movementTime);
            }
            var startingPivotPos = _pivotGrid.VectorOf(GridPoints.PivotPoint);
            var newPivotPos = startingPivotPos + direction;

            Array.Clear(_pivotGrid, 0, _pivotGrid.Length);
            Array.Clear(_shapeGrid, 0, _shapeGrid.Length);
            _playableBlock.ShapeIndex -= 1;
            var nextShape = _playableBlock.ReturnNextShape();

            var foundBottom = false;

            try
            {

                if (_bottomGrid[newPivotPos.x, newPivotPos.y] == GridPoints.BottomPoint)
                {
                    if (direction == _down)
                    {
                        foundBottom = true;
                        throw new IndexOutOfRangeException();
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }

                _pivotGrid[newPivotPos.x, newPivotPos.y] = GridPoints.PivotPoint;

                //if it gets here it means the pivot has been placed within the range and so we can test for shape
                foreach (var shapePoint in nextShape)
                {
                    if (_bottomGrid[newPivotPos.x + shapePoint.x, newPivotPos.y + shapePoint.y] == GridPoints.BottomPoint)
                    {
                        if (direction == _down)
                        {
                            foundBottom = true;
                            throw new IndexOutOfRangeException();
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                    _shapeGrid[newPivotPos.x + shapePoint.x, newPivotPos.y + shapePoint.y] = GridPoints.ShapePoint;
                }

                //only gets here when it moved and shaped
                if (_travelingRows && _rowsTraveled <= 19)
                {
                  _rowsTraveled++;
                }

            }
            catch (IndexOutOfRangeException e)
            {
                //backtracking
                Array.Clear(_pivotGrid, 0, _pivotGrid.Length);
                Array.Clear(_shapeGrid, 0, _shapeGrid.Length);
                _pivotGrid[startingPivotPos.x, startingPivotPos.y] = GridPoints.PivotPoint;
                foreach (var shapePoint in nextShape)
                {
                    _shapeGrid[startingPivotPos.x + shapePoint.x, startingPivotPos.y + shapePoint.y] = GridPoints.ShapePoint;
                }
            }
            finally
            {
                RepositionBlockMeshes();
                if (foundBottom)
                {
                    Debug.Log("found bottom");

                    soundClipChannel.Raise("bottom");

                    _calculatedScore += _rowsTraveled;
                    scoreBoardChannel.Raise(_calculatedScore);
                    

                    StopCoroutine(MoveBlock(_down));
                    ConvertShapesToBottoms();
                    ChangeMeshParent();
                    StartCoroutine(RemoveFilledRows());
                    PlayNextPiece();
                    VisualizeShapePointsIn3D();
                }
                if (direction == _down)
                {
                    StopCoroutine(MoveBlock(_down));
                    StartCoroutine(MoveBlock(_down));
                }
                
            }


        }

        private void ChangeMeshParent()
        {
            var children = new List<Transform>();
            foreach (Transform child in _blockMeshParent.transform)
            {
                children.Add(child);
            }
            _blockMeshParent.transform.DetachChildren();
            foreach (var child in children)
            {
                child.SetParent(_sceneMeshParent.transform);
            }
        }

        private void ConvertShapesToBottoms()
        {
            var shapePoints = _shapeGrid.AllCoordinatesOf(GridPoints.ShapePoint);
            foreach (var point in shapePoints)
            {
                _bottomGrid[point.Item1, point.Item2] = GridPoints.BottomPoint;
            }
            Array.Clear(_shapeGrid, 0, _shapeGrid.Length);

        }


        private void RepositionBlockMeshes()
        {

            var shapePoints = _shapeGrid.AllCoordinatesOf(GridPoints.ShapePoint);
            var i = 0;
            foreach (Transform child in _blockMeshParent.transform)
            {
                child.SetPositionAndRotation(new Vector3(shapePoints[i].Item1, shapePoints[i].Item2), Quaternion.identity);
                i++;
            }

        }

       
        private IEnumerator RemoveFilledRows()
        {
            var filledRows = new List<int>();
            for (var y = 0; y < _bottomGrid.GetLength(1) - 1; y++)
            {
                var hits = 0;
                for (var x = 0; x < _bottomGrid.GetLength(0); x++)
                {
                    if (_bottomGrid[x,y] == GridPoints.BottomPoint)
                    {
                        hits++;
                    }

                    if (hits == 10)
                    {
                        filledRows.Add(y);
                    }

                }
            }
            

            if (filledRows.Count > 0)
            {

                soundClipChannel.Raise("clearance");

                var order = new int[]
                {
                    4, 5, 3, 6, 2, 7, 1, 8, 0, 9
                };
                var next = 0;
                var blockList = new List<Vector2Int>();
                for (var i = 0; i < 5; i++)
                {
                    foreach (var y in filledRows)
                    {
                        for (var x = 0; x < _bottomGrid.GetLength(0); x++)
                        {
                            if (x == order[next])
                            {
                                blockList.Add(new Vector2Int(x, y));
                            }
                            if (x == order[next + 1])
                            {
                                blockList.Add(new Vector2Int(x, y));
                                break;
                            }
                        }
                    }
                    next += 2;
                }


                var someCount = 0;
                foreach (var point in blockList)
                {
                    foreach (Transform child in _sceneMeshParent.transform)
                    {
                        if (child.position == new Vector3(point.x,point.y))
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                    someCount++;
                    if (someCount >= filledRows.Count * 2)
                    {
                        someCount = 0;
                        yield return new WaitForSeconds(0.06f);
                    }
                }



                var bottomPoints = _bottomGrid.AllCoordinatesOf(GridPoints.BottomPoint);
                Array.Clear(_bottomGrid,0,_bottomGrid.Length);

                foreach (var (x, y) in bottomPoints)
                {
                    if (y < filledRows[0])
                    {
                        _bottomGrid[x, y + filledRows.Count] = GridPoints.BottomPoint;
                    }
                    else if(y > filledRows[filledRows.Count - 1])
                    {
                        _bottomGrid[x, y] = GridPoints.BottomPoint;
                    }
                }




                foreach (Transform child in _sceneMeshParent.transform)
                {
                    if (child.position.y < filledRows[0])
                    {
                        child.SetPositionAndRotation(new Vector3(child.position.x, child.position.y + filledRows.Count), Quaternion.identity);
                    }
                }


                _clearedLines += filledRows.Count;
                lineBoardChannel.Raise(_clearedLines);

                _=filledRows.Count switch
                {
                    1 => _calculatedScore += 40 * (_level + 1),
                    2 => _calculatedScore += 100 * (_level + 1),
                    3 => _calculatedScore += 300 * (_level + 1),
                    4 => _calculatedScore += 1200 * (_level + 1),
                    _ => throw new NotImplementedException(),
                };

                scoreBoardChannel.Raise(_calculatedScore);

                if (_clearedLines == 10)
                {
                    _level++;
                    _clearedLines = 0;
                    levelBoardChannel.Raise(_level);
                    
                }

            }


        }



    }

}
