using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

namespace Models
{
    public class Pig : MonoBehaviour
    {
        [SerializeField] private GameObject _bombsContainer;
        [SerializeField] private Tilemap _spawnTileMap;
        [SerializeField] private Camera _camera;
        [SerializeField] private int _bombsMax;
        [SerializeField] private int _bombsMaxDistance;
        [SerializeField] private GameObject _bomb;
        private ObjectPool<GameObject> _bombsPool;
        private void Awake()
        {
            _bombsPool = new ObjectPool<GameObject>(() => Instantiate(_bomb));
        }

        public void SetBomb()
        {
            var cellPosDefault = _spawnTileMap.WorldToCell(transform.position);
            var cellPosCenter = _spawnTileMap.GetCellCenterWorld(cellPosDefault);
            if (_spawnTileMap.GetColliderType(cellPosDefault) == Tile.ColliderType.Sprite)
            {
                Debug.Log("Bomb has been planted");
                var bomb = Instantiate(_bomb, _bombsContainer.transform);
                GameObject.Destroy(bomb, 5f);
                bomb.OnDestroyAsObservable().Subscribe(_ =>
                {
                    _spawnTileMap.SetColliderType(cellPosDefault, Tile.ColliderType.Sprite);
                });
                
                bomb.transform.position = cellPosCenter;
                _spawnTileMap.SetColliderType(cellPosDefault, Tile.ColliderType.None);
            }
        }
    }
}
