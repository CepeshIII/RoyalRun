using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathGenerator: MonoBehaviour
{
    private PoolManager _poolManager;

    [SerializeField] private List<GameObject> _tilePrefabs;
    [SerializeField] private List<GameObject> _collectableItemPrefabs;
    [SerializeField] private GameObject _checkPointPrefab;

    [SerializeField] private float _distanceForDeleteTiles = 6f;
    [SerializeField] private float _distanceForCreateTiles = 8f;
    [SerializeField] private float _countOfTileBetweenCheckPoint = 5;

    [SerializeField] private Vector3Int _tileSize = new Vector3Int(2, 0, 2);
    [SerializeField] private ObstacleGenerator _obstacleGenerator;
    [SerializeField] private Player _player;

    private int numberOfTilesAfterLastCheckPoint = 0;

    private readonly Vector3Int[] _moveDirections =
    {
        Vector3Int.forward,
        Vector3Int.forward,
    };

    public void Initialize(Player player, PoolManager poolManager, ObstacleGenerator obstacleGenerator)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));
        if (poolManager == null) throw new ArgumentNullException(nameof(poolManager));
        if (obstacleGenerator == null) throw new ArgumentNullException(nameof(obstacleGenerator));

        _player = player;
        _poolManager = poolManager;
        _obstacleGenerator = obstacleGenerator;

        var startPos = Vector3Int.zero;
        startPos.y = 0;

        GenerateLine(startPos, _moveDirections[1], 10, GetRandomInList(_tilePrefabs));
    }

    public void Update()
    {
        if (_poolManager != null && _player != null)
        {
            ClearPathProcess();
            GeneratePathProcess();
        }
    }

    public void ClearPathProcess()
    {
        if (_poolManager.FirstActivateTile != null && 
            _player != null)
        {
            var distantToPathStart = Vector3.Distance(_poolManager.FirstActivateTile.position,
                                        _player.transform.position);
            if (distantToPathStart > _distanceForDeleteTiles)
            {
                _poolManager.HideFirstTile();
            }
        }
    }

    public void GeneratePathProcess()
    {
        if (_poolManager.LastActivateTile != null)
        {
            var distantToEndStart = Vector3.Distance(_poolManager.LastActivateTile.position,
                                        _player.transform.position);

            if (distantToEndStart < _distanceForCreateTiles)
            {
                GenerateTilesRandom(1);
            }
        }
    }

    public void GenerateLine(Vector3Int startPos, Vector3Int direction, int length, GameObject prefab)
    {
        for(int i = 0; i < length; i++)
        {
            var position = startPos + _tileSize * (i * direction);

            AddTile(position, prefab);
        }
    }

    public void AddTile(Vector3Int pos, GameObject prefab)
    {
        var lastTile = _poolManager.AddTile(pos, prefab);
        _obstacleGenerator.GenerateObstacles(lastTile);

        if(numberOfTilesAfterLastCheckPoint >= _countOfTileBetweenCheckPoint)
        {
            numberOfTilesAfterLastCheckPoint = 0;
            _poolManager.AddCachedObject(lastTile, _checkPointPrefab, _checkPointPrefab.name);
        }
        else
        {
            numberOfTilesAfterLastCheckPoint++;
        }
    }

    public void GenerateTilesRandom(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var lastPos = _poolManager.LastActivateTile.position;
            var newPos = lastPos + _moveDirections[Random.Range(0, 2)] * _tileSize;

            AddTile(newPos, GetRandomInList(_tilePrefabs));
        }
    }

    public T GetRandomInList<T>(List<T> list)
    {
        if(list.Count > 0)
            return list[Random.Range(0, list.Count)];
        else
            return default(T);
    }

    public void Destroy()
    {
        _poolManager.Clear();

        if( _poolManager != null)
        {
            _poolManager.gameObject.SetActive(false);
            Destroy(_poolManager.gameObject);
        }

        this.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public Tile GetLastTile()
    {
        return _poolManager.LastActivateTile;
    }

    public Tile GetTileInPosition(Vector3 position)
    {
        var tileX = Mathf.CeilToInt(position.x / (float)_tileSize.x) * _tileSize.x;
        var tileY = Mathf.CeilToInt(position.y / (float)_tileSize.y) * _tileSize.y;
        var tileZ = Mathf.CeilToInt(position.z / (float)_tileSize.z) * _tileSize.z;

        var tile = _poolManager.GetTileByPosition(new Vector3Int(tileX, tileY, tileZ));
        return tile;
    }

    private void OnDestroy()
    {
        if(_poolManager!= null)
            Destroy(_poolManager.gameObject);

    }
}
